using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Entity
{
    public class Absenteeism:BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime Date     { get; set; }
        public string? Reason { get; set; }
        public bool IsExcused { get; set; } = false;
    }
}
