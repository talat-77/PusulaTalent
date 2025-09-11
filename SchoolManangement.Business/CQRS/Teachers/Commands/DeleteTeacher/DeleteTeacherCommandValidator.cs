using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.CQRS.Teachers.Commands.DeleteTeacher
{
    public class DeleteTeacherCommandValidator:AbstractValidator<DeleteTeacherCommand>
    {
        public DeleteTeacherCommandValidator()
        {
            RuleFor(x=>x.Id).NotNull().WithMessage("Id is required")
                .NotEmpty().WithMessage("Id cannot be empty");
        }
    }
}
