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

namespace SchoolManangement.Business.CQRS.Courses.Commands.DeleteAssignCourse
{
    public class DeleteAssignCourseCommandHandler : ICommandHandler<DeleteAssignCourseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CourseAssignment> _assignmentsRepository;
        public DeleteAssignCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _assignmentsRepository = _unitOfWork.GetRepository<CourseAssignment>();
        }

        public async Task<bool> Handle(DeleteAssignCourseCommand request, CancellationToken cancellationToken)
        {
           var assignment = await _assignmentsRepository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new InvalidDataException("Atama bulunamadı");

            await _assignmentsRepository.DeleteAsync(assignment);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
