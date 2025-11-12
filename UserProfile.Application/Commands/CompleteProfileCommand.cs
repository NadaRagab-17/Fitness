using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.Application.DTOs;

namespace UserProfile.Application.Commands
{
    public record CompleteProfileCommand(ProfileDto Profile) : IRequest;
}
