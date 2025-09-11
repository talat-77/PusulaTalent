using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class StudentAssignment:BaseEntity
    {
        public Student Student { get; set; }
        public Guid StudentId { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public Guid SchoolClassId { get; set; }
    }
}
