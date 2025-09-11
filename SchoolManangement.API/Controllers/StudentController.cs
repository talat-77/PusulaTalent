using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Students.Commands.AssignStudentToClass;
using SchoolManangement.Business.CQRS.Students.Commands.CreateStudent;

namespace SchoolManangement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController
    {
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-student-class")]
        public async Task<IActionResult> AssignStudentToClass(AssignStudentToClassCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}