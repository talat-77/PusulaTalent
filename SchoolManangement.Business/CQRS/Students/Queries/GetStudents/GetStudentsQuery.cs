using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Students.Queries.GetStudents
{
    public class GetStudentsQuery:IQuery<List<StudentDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
