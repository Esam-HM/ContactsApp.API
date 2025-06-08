using ContactsApp.API.Models.Domain;
using ContactsApp.API.Models.DTO.AuthDtos;
using ContactsApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly ILogger<AuthController> logger1;

        public AuthController(UserManager<AppIdentityUser> userManager,
            ITokenRepository tokenRepository,
            ILogger<AuthController> logger1)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.logger1 = logger1;
        }

        // POST: https://localhost:7195/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto request)
        {
            try
            {
                var user = new AppIdentityUser()
                {
                    FullName = request.FullName,
                    UserName = request.Email,
                    Email = request.Email
                };

                var identityResult = await userManager.CreateAsync(user, request.Password);

                if (identityResult.Succeeded)
                {
                    var roles = new List<string>()
                    {
                        "Reader", "Writer"
                    };

                    identityResult = await userManager.AddToRolesAsync(user, roles);

                    if(
                        identityResult.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        // redundant email or password validation errors
                        if (identityResult.Errors.Any())
                        {
                            foreach(var error in identityResult.Errors)
                            {
                                ModelState.AddModelError("error", error.Description);
                            }
                        }
                    }
                }
                else
                {
                    // redundant email or password validation errors
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("error", error.Description);
                        }
                    }
                }

                return ValidationProblem(ModelState);

            }
            catch(Exception ex)
            {
                logger1.LogError(ex, "Error in register user request");
                return StatusCode(500, new { error = "Unexpected Error happened" });
            }
        }

        // POST: https://localhost:7195/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto request)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if(user != null)
                {
                    var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
                    if (isPasswordValid)
                    {
                        var roles = await userManager.GetRolesAsync(user);

                        var token = tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto()
                        {
                            FullName = user.FullName,
                            Email = request.Email,
                            Roles = roles.ToList(),
                            Token = token
                        };
                        return Ok(response);
                    }
                }

                ModelState.AddModelError("error", "Email Or Password is incorrect");

                return ValidationProblem(ModelState);
            }
            catch(Exception ex)
            {
                logger1.LogError(ex, "Error Happened!!");
                return StatusCode(500, new { error = "Unexpected Error happened" });
            }
        }
    }
}
