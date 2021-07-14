using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using FluentValidation;

namespace Movies.Data.Models.Validators
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleSet("RegisterUpdate", () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(30)
                    .Matches(@"^[\w]+$")
                    .When(x => !x.Name.IsNullOrEmpty());
            });

            RuleFor(x => x.Login)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(30)
                .Matches(@"^[\w\d]+$")
                .When(x => !x.Login.IsNullOrEmpty());

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(30)
                .Matches(@"^[\S]+$")
                .When(x => !x.Password.IsNullOrEmpty());
        }
    }
}
