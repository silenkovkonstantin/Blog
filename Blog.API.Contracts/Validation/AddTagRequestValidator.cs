using BlogAPI.Contracts.Models.Tags;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Contracts.Validation
{
    public class AddTagRequestValidator : AbstractValidator<AddTagRequest>
    {
        public AddTagRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
