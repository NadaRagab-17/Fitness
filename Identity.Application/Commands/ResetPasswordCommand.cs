using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Commands
{
    public record ResetPasswordCommand(string Email, string OtpCode, string NewPassword) : IRequest;
}
