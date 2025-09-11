using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class Course:BaseEntity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credit { get; set; }
        public string? Description { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }

    }
}
