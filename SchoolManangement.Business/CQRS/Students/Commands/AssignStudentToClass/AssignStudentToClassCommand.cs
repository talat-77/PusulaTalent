using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Students.Commands.AssignStudentToClass
{
    public class AssignStudentToClassCommand:ICommand<bool>
    {
        public Guid StudentId { get; set; }
        public Guid SchoolClassId { get; set; }
    }
}
