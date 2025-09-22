using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class Note:BaseEntity
    {
        public Course Course { get; set; } = null!;
        public decimal Value { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid StudentId { get; set; }
    }
}
