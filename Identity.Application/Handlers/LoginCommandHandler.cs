using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;

        public LoginCommandHandler(
            IGenericRepository<User> userRepo,
            IPasswordHasher<User> passwordHasher,
            IConfiguration config)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _config = config;
        }


        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = (await _userRepo.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
            if (user == null)
                throw new ApplicationException("Invalid email or password.");

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
                throw new ApplicationException("Invalid email or password.");

            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", $"{user.FirstName} {user.LastName}")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResultDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = token.ValidTo,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}"
            };
        }
    }
}

    

