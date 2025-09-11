using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class CourseAssignment:BaseEntity
    {

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Guid SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
    }
}
