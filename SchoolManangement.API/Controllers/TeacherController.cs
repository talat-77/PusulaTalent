using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Teachers.Commands.CreateTeacher;
using SchoolManangement.Business.CQRS.Teachers.Commands.DeleteTeacher;
using SchoolManangement.Business.CQRS.Teachers.Queries.GetStudentByOwnCourse;

namespace SchoolManangement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : BaseController
    {
        [Authorize(Roles ="Admin")]
        [HttpPost("create-teacher")]
        public async Task<IActionResult> CreateTeacher(CreateTeacherCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var command = new DeleteTeacherCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("GetStudentByOwnCourse/{teacherId}")]
        public async Task<IActionResult> GetStudentByOwnCourse(Guid teacherId)
        {
            var result = await Mediator.Send(new GetStudentByOwnCourseQuery { TeacherId = teacherId });
            return Ok(result);
        }
    }
}
