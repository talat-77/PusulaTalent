using AutoMapper;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Teachers.Commands.DeleteTeacher
{
    public class DeleteTeacherCommandHandler : ICommandHandler<DeleteTeacherCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Teacher> _teacherRepository;
        
        public DeleteTeacherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _teacherRepository = _unitOfWork.GetRepository<Teacher>();
        }

        public async Task<bool> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id) ??
                 throw new InvalidDataException("Teacher Not found");

            await _teacherRepository.DeleteAsync(teacher);  
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
