using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Teachers.Commands.CreateTeacher;

namespace SchoolManangement.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        [HttpPost("create-teacher")]
        public async Task<IActionResult> CreateTeacher(CreateTeacherCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
