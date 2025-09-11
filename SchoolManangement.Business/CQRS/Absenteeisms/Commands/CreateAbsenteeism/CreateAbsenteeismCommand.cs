using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Absenteeisms.Commands.CreateAbsenteeism
{
    public class CreateAbsenteeismCommand:ICommand<bool>
    {
        public Guid UserId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
        public bool IsExcused { get; set; } = false;
    }
}
