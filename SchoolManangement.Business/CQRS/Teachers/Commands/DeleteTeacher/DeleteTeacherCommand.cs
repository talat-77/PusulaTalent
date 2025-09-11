using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Teachers.Commands.DeleteTeacher
{
    public class DeleteTeacherCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
