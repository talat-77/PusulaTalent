using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Emails.Commands.SendEmail;
using SchoolManangement.Business.CQRS.Emails.Commands.SendEmailMultiples.Commands.SendEmailMultiple;

namespace SchoolManangement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : BaseController
    {
        [HttpPost]
        [Consumes("multipart/form-data")]//tek bir kişi için action
        public async Task<IActionResult> SendEmail([FromForm] SendEmailCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("send-multiple")]
        public async Task<IActionResult> SendToMultipleUsers([FromForm] SendEmailToMultipleUsersCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

    }
}
