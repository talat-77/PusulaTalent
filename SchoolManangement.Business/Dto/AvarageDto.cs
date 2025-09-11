using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Dto
{
    public class AvarageDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public decimal AvarageValue { get; set; }
        public string StudentName  { get; set; }
    }
}
