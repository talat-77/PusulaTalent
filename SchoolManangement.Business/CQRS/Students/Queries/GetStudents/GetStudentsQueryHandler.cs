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

namespace SchoolManangement.Business.CQRS.Students.Queries.GetStudents
{
    public class GetStudentsQueryHandler : IQueryHandler<GetStudentsQuery, List<StudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _repository;
        public GetStudentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<Student>();
        }

        public async Task<List<StudentDto>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
          var query = await _repository.GetAllAsync(filter:null,false,request.PageSize,request.PageNumber);
            return _mapper.Map<List<StudentDto>>(query);
        }
    }
}
