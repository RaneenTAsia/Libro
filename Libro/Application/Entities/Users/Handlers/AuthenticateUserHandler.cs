using Application.Abstractions.Repositories;
using Application.DTOs;
using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Handlers
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, (string, Result)>
    {
        public readonly IUserRepository _userRepository;
        public readonly ILogger<AuthenticateUserHandler> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticateUserHandler(IUserRepository userRepository, ILogger<AuthenticateUserHandler> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<(string, Result)> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User attempts logging in with email {request.Email}, and password {request.Password}");

            var (user, result) = await _userRepository.ValidateUserCredentials(request.Email, request.Password);

            if (result == Result.Failed)
            {
                return ("Incorrect email or password", Result.Failed);
            }

            _logger.LogInformation($"Login success");

            // Step 2: create a token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("username", user.Username));
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("passwordHash", user.PasswordHash));
            claimsForToken.Add(new Claim("passwordSalt", user.PasswordSalt));
            claimsForToken.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return (tokenToReturn, Result.Completed);
        }
    }
}
