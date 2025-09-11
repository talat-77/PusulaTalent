using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Courses.Commands.AssignCourseToClass
{
    public class AssignCourseToClassCommand:ICommand<bool>
    {
        public Guid ClassId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
    }
}
