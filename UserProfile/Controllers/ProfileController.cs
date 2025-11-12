using Identity.Shared.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserProfile.Application.Commands;
using UserProfile.Application.DTOs;
using UserProfile.Domain.Entities;

namespace UserProfile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator) => _mediator = mediator;

        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody] ProfileDto dto)
        {
            await _mediator.Send(new CompleteProfileCommand(dto));
            return Ok(new { Message = "Profile saved" });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId, [FromServices] IGenericRepository<UserProfileClass> repo)
        {
            var profile = (await repo.FindAsync(p => p.UserId == userId)).FirstOrDefault();
            if (profile == null) return NotFound();
            return Ok(profile);
        }
    }
}
