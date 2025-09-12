using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.SchoolClasses.Queries.GetSchoolClasses
{
    public class GetSchoolClassesQuery:IQuery<List<SchoolClassDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
