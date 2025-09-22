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

namespace SchoolManangement.Business.CQRS.Courses.Commands.AssignCourseToClass
{
    public class AssignCourseToClassCommandHandler : ICommandHandler<AssignCourseToClassCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IRepository<SchoolClass> _classRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<CourseAssignment> _assignmentRepository;
        public AssignCourseToClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _teacherRepository = _unitOfWork.GetRepository<Teacher>();
            _classRepository = _unitOfWork.GetRepository<SchoolClass>();
            _courseRepository = _unitOfWork.GetRepository<Course>();
            _assignmentRepository = _unitOfWork.GetRepository<CourseAssignment>();
        }

        public async Task<bool> Handle(AssignCourseToClassCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.GetFirstOrDefaultAsync(x => x.Id == request.TeacherId)??
                throw new InvalidDataException("Invalid TeacherId");

            var classEntity = await _classRepository.GetFirstOrDefaultAsync(x => x.Id == request.SchoolClassId,false,null) ??
                throw new InvalidDataException("Invalid ClassId");

            var course = await _courseRepository.GetFirstOrDefaultAsync(x => x.Id == request.CourseId) ??
                throw new InvalidDataException("Invalid CourseId");

            var existingAssignment = await _assignmentRepository.GetFirstOrDefaultAsync(
                filter: x => x.SchoolClassId == request.SchoolClassId && x.CourseId == request.CourseId && x.TeacherId == request.TeacherId, false, null);

            if (existingAssignment != null)
                return false;

            var assignments = _mapper.Map<CourseAssignment>(request);
            await _assignmentRepository.CreateAsync(assignments);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
