﻿using ContactsApp.API.Models.Domain;
using ContactsApp.API.Repositories.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactsApp.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(AppIdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(
                roles.Select(
                    role => new Claim(ClaimTypes.Role, role)
                )
            );

            // Setting JWT Security Token Parameters
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            // Return Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
