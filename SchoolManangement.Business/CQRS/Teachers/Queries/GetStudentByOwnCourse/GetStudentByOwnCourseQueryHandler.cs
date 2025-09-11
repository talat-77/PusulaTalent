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
        public GetStudentByOwnCourseQueryHandler(IUnitOfWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _teacherRepository = _unitofWork.GetRepository<Teacher>();
            _assignmentCourseRepository = _unitofWork.GetRepository<CourseAssignment>();
        }

        public async Task<List<StudentDto>> Handle(GetStudentByOwnCourseQuery request, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.GetFirstOrDefaultAsync(x => x.Id == request.TeacherId) ??
                throw new InvalidDataException("Teacher not found");

            var query = await _assignmentCourseRepository.GetAllAsync(
                filter: x => x.TeacherId == request.TeacherId,
                include: i => i.Include(i => i.SchoolClass)
                .ThenInclude(a => a.Students)
                );

            var studentDtos = query
               .SelectMany(ca => ca.SchoolClass.Students)
               .Select(s => new StudentDto {
                   Id = s.Id,
                   FirstName = s.FirstName,
                   LastName = s.LastName,
                   StudentNumber=s.StudentNumber 
               })
               .Distinct()
               .ToList();


            return studentDtos;
        }
    }
}
