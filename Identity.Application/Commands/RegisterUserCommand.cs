using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Commands
{
    public record RegisterUserCommand(
       string FirstName,
       string LastName,
       string Email,
       string Password,
       string Phone,
       string Gender
   ) : IRequest<Guid>;
}
