using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Absenteeisms.Commands.CreateAbsenteeism;
using SchoolManangement.Business.CQRS.Absenteeisms.Queries.GetAbsenteeisms;

namespace SchoolManangement.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenteeismController : BaseController
    {
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAbsenteeismCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("GetByStudentId/{studentId}")]
        public async Task<IActionResult> GetByStudentId(Guid studentId)
        {
            var result = await Mediator.Send(new GetAbsenteeismsQuery { StudentId = studentId });
            return Ok(result);
        }

    }
}
