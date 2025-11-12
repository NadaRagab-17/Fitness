using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.Application.Commands;

namespace UserProfile.Application.Validators
{
    public class CompleteProfileCommandValidator : AbstractValidator<CompleteProfileCommand>
    {
        public CompleteProfileCommandValidator()
        {
            RuleFor(x => x.Profile.UserId).NotEmpty();
            RuleFor(x => x.Profile.Weight).GreaterThan(0).When(x => x.Profile.Weight.HasValue);
            RuleFor(x => x.Profile.Height).GreaterThan(0).When(x => x.Profile.Height.HasValue);
        }
    }
}
