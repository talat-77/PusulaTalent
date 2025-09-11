using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManangement.Business.CQRS.Note.Commands.CreateNote;
using SchoolManangement.Business.CQRS.Note.Queries.GetAvarageNoteValue;

namespace SchoolManangement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : BaseController
    {
        [Authorize(Roles ="Admin,Teacher")]
        [HttpPost("AddNote")]
        public async Task<IActionResult> CreateNote(CreateNoteCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("GetAvarageNoteByStudentId/{studentId}")]
        public async Task<IActionResult> GetAvarageNoteByStudentId(Guid studentId)
        {
            var result = await Mediator.Send(new GetAvarageNoteValueQuery { StudentId = studentId });
            return Ok(result);
        }
    }
}
