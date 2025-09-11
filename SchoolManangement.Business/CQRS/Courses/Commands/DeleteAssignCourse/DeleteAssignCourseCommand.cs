using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Courses.Commands.DeleteAssignCourse
{
    public class DeleteAssignCourseCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
