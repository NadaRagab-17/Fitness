using Identity.Application.Commands;
using Identity.Domain.Entities;
using Identity.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Handlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordCommandHandler(
            IGenericRepository<User> userRepo,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = (await _userRepo.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
            if (user == null)
                throw new ApplicationException("User not found.");

            if (user.OtpCode != request.OtpCode || user.OtpExpiration < DateTime.UtcNow)
                throw new ApplicationException("Invalid or expired OTP code.");

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.OtpCode = null;
            user.OtpExpiration = null;

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            return Unit.Value;
        }

        
    }
}
