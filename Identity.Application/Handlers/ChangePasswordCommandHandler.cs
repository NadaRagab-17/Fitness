using Identity.Application.Commands;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordCommandHandler(
            IGenericRepository<User> userRepo,
            IPasswordHasher<User> passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new ApplicationException("Unauthorized.");

            var userId = Guid.Parse(userIdClaim.Value);

            var user = (await _userRepo.FindAsync(u => u.Id == userId)).FirstOrDefault();
            if (user == null)
                throw new ApplicationException("User not found.");

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
            if (verify == PasswordVerificationResult.Failed)
                throw new ApplicationException("Current password is incorrect.");

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
