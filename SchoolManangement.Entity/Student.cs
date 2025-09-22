using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class Student:BaseEntity
    {
        public string  Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ClassNumber ClassNumber { get; set; }
        public string StudentNumber { get; set; } = null!;
        public ICollection<StudentCourse> StudentCourses { get; set; }

        [ForeignKey("SchoolClassId")]
        public SchoolClass? SchoolClass { get; set; }
        public ICollection<Note> Notes { get; set; }
        public Guid? SchoolClassId { get; set; }
    }

    public enum ClassNumber
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12
    }   
}
