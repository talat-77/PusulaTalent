using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class StudentCourse:BaseEntity
    {
        public Student Student { get; set; }
        public Guid StudentId { get; set; }
        public Course Course { get; set; }
        public Guid CourseId { get; set; }
    }
}
