using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.SchoolClasses.Commands.CreateSchoolClass;

namespace SchoolManangement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolClassController : BaseController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateSchoolClass")]
        public async Task<IActionResult> CreateSchoolClass(CreateSchoolClassCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
