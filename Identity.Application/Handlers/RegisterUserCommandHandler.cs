using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Commands;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(
            IGenericRepository<User> userRepo,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var exists = await _userRepo.FindAsync(u => u.Email == request.Email);
            if (exists.Any())
                throw new ApplicationException("Email already in use");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Gender = request.Gender
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return user.Id;
        }
    }
}
