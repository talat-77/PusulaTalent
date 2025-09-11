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

namespace SchoolManangement.Business.CQRS.Absenteeisms.Queries.GetAbsenteeisms
{
    public class GetAbsenteeismsQueryHandler : IQueryHandler<GetAbsenteeismsQuery, List<AbsenteeismDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Absenteeism> _repository;
        public GetAbsenteeismsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<Absenteeism>();
        }

        public async Task<List<AbsenteeismDto>> Handle(GetAbsenteeismsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsync(
                filter: a => a.StudentId == request.StudentId,
                include: x => x.Include(i => i.Student)
                );

            return _mapper.Map<List<AbsenteeismDto>>(query); 
        }
    }
}
