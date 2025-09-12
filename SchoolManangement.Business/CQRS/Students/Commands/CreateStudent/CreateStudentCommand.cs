using SchoolManangement.Business.Dto;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Students.Commands.CreateStudent
{
    public class CreateStudentCommand : ICommand<StudentDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }          
        public string PhoneNumber { get; set; }    
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ClassNumber ClassNumber { get; set; }
        public string StudentNumber { get; set; }  
        public Guid? ClassId { get; set; }

    }
}
