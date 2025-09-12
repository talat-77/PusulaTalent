using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.SchoolClasses.Commands.CreateSchoolClass;
using SchoolManangement.Business.CQRS.SchoolClasses.Commands.DeleteSchoolClass;
using SchoolManangement.Business.CQRS.SchoolClasses.Queries.GetSchoolClasses;

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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolClass(Guid id)
        {
            var result = await Mediator.Send(new DeleteSchoolClassCommand { Id=id});
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("GetAllSchoolClasses")]
        public async Task<IActionResult> GetAllSchoolClasses(int pageSize,int pageNumber)
        {
            var result = await Mediator.Send(new GetSchoolClassesQuery{PageSize=pageSize,PageNumber=pageNumber});
            return Ok(result);
        }

    }
}
