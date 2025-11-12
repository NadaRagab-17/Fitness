using Identity.Shared.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.Application.Commands;
using UserProfile.Application.DTOs;

namespace UserProfile.Application.IntegrationHandlers
{
    public class UserRegisteredEventHandler
    {
        private readonly IMediator _mediator;
        public UserRegisteredEventHandler(IMediator mediator) => _mediator = mediator;

        public async Task Handle(UserRegisteredEvent @event)
        {
            
            var dto = new ProfileDto
            {
                UserId = @event.UserId
            };

            await _mediator.Send(new CompleteProfileCommand(dto));
        }
    }
}
