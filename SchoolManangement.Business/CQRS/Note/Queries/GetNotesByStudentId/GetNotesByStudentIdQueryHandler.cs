using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManangement.Business.Dto;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Note.Queries.GetNotesByStudentId
{
    public class GetNotesByStudentIdQueryHandler : IQueryHandler<GetNotesByStudentIdQuery, List<NoteDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolManangement.Entity.Note> _repository;
        public GetNotesByStudentIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<SchoolManangement.Entity.Note>();
        }

        public async Task<List<NoteDto>> Handle(GetNotesByStudentIdQuery request, CancellationToken cancellationToken)
        {
            var note = await _repository.GetAllAsync(
                filter: x => x.StudentId == request.StudentId,
                include: i => i.Include(c => c.Course)
                )?? throw new InvalidDataException("Not found any notes");

                return _mapper.Map<List<NoteDto>>(note);
            
        }
    }
}
