using SchoolManangement.Business.Dto;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.SchoolClasses.Commands.CreateSchoolClass
{
    public class CreateSchoolClassCommand:ICommand<SchoolClassDto>
    {
        public string Name { get; set; }
        public string? Description { get; set; }

    }
}
