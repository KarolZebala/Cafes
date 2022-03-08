using CafeApi.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Models.Validators
{
    //Aby korzystać z Fluent Validation to w Starupoe trzeba dodatkowo dopisać po Services.AddControlers()
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(CafeDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                 .Custom((value, context) =>
                 {
                     var emailInUse = dbContext.Users.Any(u => u.Email == value);
                     if (emailInUse)
                     {
                         context.AddFailure("Email", "That email is taken");
                     }
                 });

        }
    }
}
