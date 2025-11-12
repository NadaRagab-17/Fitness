using Identity.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.Application.Commands;
using UserProfile.Application.Services;
using UserProfile.Domain.Entities;

namespace UserProfile.Application.Handlers
{
    public class CompleteProfileCommandHandler : IRequestHandler<CompleteProfileCommand>
    {
        private readonly IGenericRepository<UserProfileClass> _repo;
        private readonly ProfileService _service;

        public CompleteProfileCommandHandler(IGenericRepository<UserProfileClass> repo, ProfileService service)
        {
            _repo = repo;
            _service = service;
        }

        public async Task<Unit> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Profile;

            var existing = (await _repo.FindAsync(p => p.UserId == dto.UserId)).FirstOrDefault();

            if (existing == null)
            {
                var profile = new UserProfileClass
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.UserId,
                    Weight = dto.Weight,
                    Height = dto.Height,
                    Goal = dto.Goal,
                    ActivityLevel = dto.ActivityLevel,
                    DateOfBirth = dto.DateOfBirth,
                    CreatedAt = DateTime.UtcNow
                };
                await _repo.AddAsync(profile);
            }
            else
            {
                existing.Weight = dto.Weight;
                existing.Height = dto.Height;
                existing.Goal = dto.Goal;
                existing.ActivityLevel = dto.ActivityLevel;
                existing.DateOfBirth = dto.DateOfBirth;
                existing.UpdatedAt = DateTime.UtcNow;
                _repo.Update(existing);
            }

            await _repo.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
