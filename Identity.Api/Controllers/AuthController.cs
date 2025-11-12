using Identity.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Register), new { id }, new { id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand command)
        {
            var otp = await _mediator.Send(command);
            return Ok(new { Message = "OTP sent successfully", OtpForTest = otp });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Password reset successful" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Password changed successfully" });
        }


        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // stateless token,, logout logic (e.g., token invalidation) can be implemented here if needed

            return Ok(new { Message = "User logged out successfully." });
        }


    }
}
