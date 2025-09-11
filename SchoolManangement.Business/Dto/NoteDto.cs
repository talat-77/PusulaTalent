using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Dto
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid StudentId { get; set; }
    }
}
