using ContactsApp.API.Data;
using ContactsApp.API.Models.Domain;
using ContactsApp.API.Repositories.Implementations;
using ContactsApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsAppConnectionString"));
    });

    builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsAppConnectionString"));
    });

    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<ITokenRepository, TokenRepository>();

    builder.Services.AddIdentityCore<AppIdentityUser>()
        .AddRoles<IdentityRole>()
        .AddTokenProvider<DataProtectorTokenProvider<AppIdentityUser>>("ContactsApp")
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                AuthenticationType = "Jwt",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    var app = builder.Build();

    //app.Use(async (context, next) =>
    //{
    //    await next(); // Run the rest of the middleware pipeline

    //    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    //    if (context.Response.StatusCode == 401)
    //    {
    //        logger.LogWarning("401 Unauthorized returned for request to {Path}", context.Request.Path);
    //    }
    //    else if (context.Response.StatusCode == 403)
    //    {
    //        logger.LogWarning("403 Forbidden returned for request to {Path}", context.Request.Path);
    //    }
    //    else if (context.Response.StatusCode == 500)
    //    {
    //        logger.LogError("500 Internal Server Error at {Path}", context.Request.Path);
    //    }
    //});

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors(options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyOrigin();
        options.AllowAnyMethod();
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}