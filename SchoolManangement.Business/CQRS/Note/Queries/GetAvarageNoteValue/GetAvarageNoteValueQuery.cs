using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Note.Queries.GetAvarageNoteValue
{
    public class GetAvarageNoteValueQuery:IQuery<AvarageDto>
    {
        public Guid StudentId { get; set; }
    }
}
