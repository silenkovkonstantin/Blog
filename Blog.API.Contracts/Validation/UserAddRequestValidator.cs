using BlogAPI.Contracts.Models.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Contracts.Validation
{
    public class UserAddRequestValidator : AbstractValidator<AddUserRequest>
    {
        public UserAddRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.PasswordReg).NotEmpty();
            RuleFor(x => x.PasswordConfirm).NotEmpty();
        }
    }
}
