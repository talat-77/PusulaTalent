using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Students.Commands.AssignStudentToClass;
using SchoolManangement.Business.CQRS.Students.Commands.CreateStudent;
using SchoolManangement.Business.CQRS.Students.Queries.GetStudents;

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

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllStudents(int pageSize = 100, int pageNumber = 1)
        {
            var result = await Mediator.Send(new GetStudentsQuery { PageSize = pageSize, PageNumber = pageNumber });
            return Ok(result);
        }


    }
}