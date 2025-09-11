using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Courses.Commands.AssignCourseToClass;
using SchoolManangement.Business.CQRS.Courses.Commands.CreateCourse;
using SchoolManangement.Business.CQRS.Courses.Commands.DeleteAssignCourse;
using SchoolManangement.Business.CQRS.Courses.Commands.DeleteCourse;

namespace SchoolManangement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseController
    {
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCourseCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign")]
        public async Task<IActionResult> AssignCourse(AssignCourseToClassCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("assign/{id}")]
        public async Task<IActionResult> DeleteAssignCourse(Guid id)
        {
            var command = new DeleteAssignCourseCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

    }
}
