﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Domain.ConfigOptions;
using Portfolio.API.Domain.DataTransferObjects;
using Portfolio.API.Domain.RepositoryInterfaces;
using Portfolio.API.Domain.ServiceInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.API.AppLogic.Services
{
    internal class AuthService(IOptions<JwtTokenOptions> options, IPasswordRepository passwordRepository) : IAuthService
    {
        private readonly JwtSecurityTokenHandler _jwtTokenHandler = new();
        private readonly JwtTokenOptions _tokenOptions = options.Value;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        public Task<bool> IsValid(AuthenticationDTO authenticationDTO, CancellationToken cancellationToken)
        {
            string hashValue = HashWithSHA256(authenticationDTO.Password);

            return _passwordRepository.HasPassword(hashValue, cancellationToken);
        }

        public AuthTokenDTO GenerateJwtToken()
        {
            SigningCredentials signingCredentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            DateTime expirationTime = DateTime.UtcNow.AddHours(_tokenOptions.ExpireAfterH);

            JwtSecurityToken token = new(_tokenOptions.Issuer, _tokenOptions.Audience, null, null, expirationTime, signingCredentials);

            string tokenValue = _jwtTokenHandler.WriteToken(token);

            return new AuthTokenDTO(tokenValue, expirationTime);
        }

        private static string HashWithSHA256(string value)
        {
            var byteArray = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(byteArray);
        }
    }
}
