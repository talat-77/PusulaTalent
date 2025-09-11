using AutoMapper;
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

namespace SchoolManangement.Business.CQRS.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, CourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
       
        public CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _courseRepository = _unitOfWork.GetRepository<Course>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
           var user = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id==request.UserId)??
                throw new InvalidDataException("User not found");

            var newCourse = _mapper.Map<Course>(request);
            await _courseRepository.CreateAsync(newCourse);
            await _unitOfWork.CommitAsync(cancellationToken);
            return _mapper.Map<CourseDto>(newCourse);
        }
    }
}
