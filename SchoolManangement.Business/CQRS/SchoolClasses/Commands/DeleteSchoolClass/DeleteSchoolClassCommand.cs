using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.SchoolClasses.Commands.DeleteSchoolClass
{
    public class DeleteSchoolClassCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
