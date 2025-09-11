using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Dto
{
    public class StudentDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ClassNumber ClassNumber { get; set; }
        public string StudentNumber { get; set; } = null!;
        public Guid? ClassId { get; set; }
    }
}
