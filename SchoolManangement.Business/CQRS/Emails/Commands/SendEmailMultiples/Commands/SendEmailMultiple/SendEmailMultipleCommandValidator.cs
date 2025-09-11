using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.CQRS.Emails.Commands.SendEmailMultiples.Commands.SendEmailMultiple
{
    public class SendEmailToMultipleUsersCommandValidator : AbstractValidator<SendEmailToMultipleUsersCommand>
    {
        public SendEmailToMultipleUsersCommandValidator()
        {
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
            RuleFor(x => x.Tos).NotEmpty();

        }
    }
}
