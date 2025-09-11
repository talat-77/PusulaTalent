using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class SchoolClass:BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }


    }
}
