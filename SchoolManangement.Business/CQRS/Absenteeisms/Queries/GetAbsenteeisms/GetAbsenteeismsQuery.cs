using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Absenteeisms.Queries.GetAbsenteeisms
{
    public class GetAbsenteeismsQuery:IQuery<List<AbsenteeismDto>>
    {
        public Guid StudentId { get; set; }
    }
}
