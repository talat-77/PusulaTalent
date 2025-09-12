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

namespace SchoolManangement.Business.CQRS.SchoolClasses.Commands.CreateSchoolClass
{
    public class CreateSchoolClassCommandHandler : ICommandHandler<CreateSchoolClassCommand, SchoolClassDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolClass> _repository;
        public CreateSchoolClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<SchoolClass>();
        }

        public async Task<SchoolClassDto> Handle(CreateSchoolClassCommand request, CancellationToken cancellationToken)
        {
           var newschoolClass = _mapper.Map<SchoolClass>(request);
            await _repository.CreateAsync(newschoolClass);
            await _unitOfWork.CommitAsync(cancellationToken);
           return _mapper.Map<SchoolClassDto>(newschoolClass);
          
        }
    }
}
