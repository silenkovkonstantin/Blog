using BlogAPI.Contracts.Models.Posts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Contracts.Validation
{
    public class AddPostRequestValidator : AbstractValidator<AddPostRequest>
    {
        public AddPostRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Text).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
