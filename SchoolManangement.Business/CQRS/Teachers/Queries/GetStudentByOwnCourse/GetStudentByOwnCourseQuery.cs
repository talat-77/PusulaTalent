using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Teachers.Queries.GetStudentByOwnCourse
{
    public class GetStudentByOwnCourseQuery:IQuery<List<StudentDto>>
    {
        public Guid TeacherId { get; set; }
    }
}
