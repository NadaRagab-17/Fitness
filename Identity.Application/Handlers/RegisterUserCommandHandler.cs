using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Commands;
using Identity.Domain.Entities;
using Identity.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Identity.Application.Common;
using Identity.Domain.Events;

namespace Identity.Application.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEventPublisher _eventPublisher;

        public RegisterUserCommandHandler(
            IGenericRepository<User> userRepo,
            IPasswordHasher<User> passwordHasher,
            IEventPublisher eventPublisher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _eventPublisher = eventPublisher;
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

            await _userRepo.SaveChangesAsync();

            await _eventPublisher.PublishAsync(new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}"
            });

            return user.Id;
        }
    }
}
