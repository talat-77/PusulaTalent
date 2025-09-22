using SchoolManangement.Business.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Note.Commands.CreateNote
{
    public class CreateNoteCommand:ICommand<NoteDto>
    {
        public decimal Value { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid StudentId { get; set; }
    }
}
