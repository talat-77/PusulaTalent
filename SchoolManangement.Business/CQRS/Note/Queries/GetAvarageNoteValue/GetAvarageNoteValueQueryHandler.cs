using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManangement.Business.Dto;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Note.Queries.GetAvarageNoteValue
{
    public class GetAvarageNoteValueQueryHandler : IQueryHandler<GetAvarageNoteValueQuery, AvarageDto>
    {
        private readonly IUnitOfWork  _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolManangement.Entity.Note> _noteRepository;
        private readonly IRepository<Student> _studentRepository;

        public GetAvarageNoteValueQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noteRepository = _unitOfWork.GetRepository<SchoolManangement.Entity.Note>();
            _studentRepository = _unitOfWork.GetRepository<Student>();
        }

        public async Task<AvarageDto> Handle(GetAvarageNoteValueQuery request, CancellationToken cancellationToken)
        {
            var student =await _studentRepository.GetFirstOrDefaultAsync(x=>x.Id==request.StudentId)??
                throw new InvalidDataException("Student not found");

            var notes = await _noteRepository.GetAllAsync(filter:x=>x.StudentId==request.StudentId,include:x=>x.Include(i=>i.Course))??
                throw new InvalidDataException("Notes not found for the student");
            var valueList = notes.Select(x => (x.Course.Credit, x.Value));

            var avarageValue = CalculateAvarage(valueList);
            var avarageDto = new AvarageDto
            {
                StudentId = student.Id,
                StudentName = student.FirstName,
                AvarageValue = avarageValue
            };

            return avarageDto;  
        }


        public decimal CalculateAvarage(IEnumerable<(int credit,decimal value)> values)
        {
            if (values == null || !values.Any())
                return 0;

            decimal totalWeight = values.Sum(v => v.credit * v.value);
            int totalCredits = values.Sum(v => v.credit);
            return totalCredits == 0 ? 0 : totalWeight / totalCredits;


        }
    }
}
