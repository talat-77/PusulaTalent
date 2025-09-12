using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Note.Queries.GetNotesByStudentId
{
    public class GetNotesByStudentIdQuery:IQuery<List<NoteDto>>
    {
        public Guid StudentId { get; set; }
    }
}
