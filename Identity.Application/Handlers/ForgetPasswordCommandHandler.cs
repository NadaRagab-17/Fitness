using Identity.Application.Commands;
using Identity.Domain.Entities;
using Identity.Shared.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Handlers
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, string>
    {
        private readonly IGenericRepository<User> _userRepo;

        public ForgetPasswordCommandHandler(IGenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<string> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = (await _userRepo.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
            if (user == null)
                throw new ApplicationException("User not found");

            // generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otp;
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(10);

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();

            return otp;
        }

    }
}
    

