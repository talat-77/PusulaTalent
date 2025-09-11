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

namespace SchoolManangement.Business.CQRS.Courses.Commands.DeleteCourse
{
    public class DeleteCourseCommandHandler : ICommandHandler<DeleteCourseCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Course> _courseRepository;
        public DeleteCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _courseRepository = _unitOfWork.GetRepository<Course>();
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
           var course = await _courseRepository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new InvalidDataException("Not found course");


            await _courseRepository.DeleteAsync(course);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
