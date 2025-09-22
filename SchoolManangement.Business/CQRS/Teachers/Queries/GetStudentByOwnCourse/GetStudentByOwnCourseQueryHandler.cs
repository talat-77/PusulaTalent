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

namespace SchoolManangement.Business.CQRS.Teachers.Queries.GetStudentByOwnCourse
{
    public class GetStudentByOwnCourseQueryHandler : IQueryHandler<GetStudentByOwnCourseQuery, List<StudentDto>>
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IRepository<CourseAssignment> _assignmentCourseRepository;
        private readonly IRepository<StudentCourse> _studentCourseRepository;

        public GetStudentByOwnCourseQueryHandler(IUnitOfWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _teacherRepository = _unitofWork.GetRepository<Teacher>();
            _assignmentCourseRepository = _unitofWork.GetRepository<CourseAssignment>();
            _studentCourseRepository = _unitofWork.GetRepository<StudentCourse>();
        }

        public async Task<List<StudentDto>> Handle(GetStudentByOwnCourseQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.GetFirstOrDefaultAsync(x => x.Id == request.TeacherId) ??
                throw new InvalidDataException("Teacher not found");

            var teacherCourseAssignments = await _assignmentCourseRepository.GetAllAsync(
                filter: x => x.TeacherId == request.TeacherId
            );

            if (!teacherCourseAssignments.Any())
            {
                return new List<StudentDto>();
            }

            var courseIds = teacherCourseAssignments.Select(ca => ca.CourseId).ToList();

            var studentCourses = await _studentCourseRepository.GetAllAsync(
                filter: sc => courseIds.Contains(sc.CourseId),
                include: i => i.Include(sc => sc.Student)
            );

            var studentDtos = _mapper.Map<List<StudentDto>>(studentCourses.Select(sc => sc.Student).Distinct().ToList());

            return studentDtos;
        }
    }
}