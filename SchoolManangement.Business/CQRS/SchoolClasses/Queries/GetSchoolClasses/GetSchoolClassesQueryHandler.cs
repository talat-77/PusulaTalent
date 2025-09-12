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

namespace SchoolManangement.Business.CQRS.SchoolClasses.Queries.GetSchoolClasses
{
    public class GetSchoolClassesQueryHandler : IQueryHandler<GetSchoolClassesQuery, List<SchoolClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolClass> _repository;
        public GetSchoolClassesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SchoolClassDto>> Handle(GetSchoolClassesQuery request, CancellationToken cancellationToken)
        {
           var query = await _repository.GetAllAsync(filter:null,false,request.PageSize,request.PageNumber);
            return _mapper.Map<List<SchoolClassDto>>(query);
        }
    }
}
