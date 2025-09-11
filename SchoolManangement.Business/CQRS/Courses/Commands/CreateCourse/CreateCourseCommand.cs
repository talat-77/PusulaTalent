using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Courses.Commands.CreateCourse
{
    public class CreateCourseCommand:ICommand<CourseDto>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credit { get; set; }
        public string? Description { get; set; }
    }
}
