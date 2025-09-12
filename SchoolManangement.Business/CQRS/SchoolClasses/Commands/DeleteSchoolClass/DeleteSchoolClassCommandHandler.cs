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

namespace SchoolManangement.Business.CQRS.SchoolClasses.Commands.DeleteSchoolClass
{
    public class DeleteSchoolClassCommandHandler : ICommandHandler<DeleteSchoolClassCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolClass> _repository;
        public DeleteSchoolClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<SchoolClass>();
        }

        public async Task<bool> Handle(DeleteSchoolClassCommand request, CancellationToken cancellationToken)
        {
           var classs= await _repository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new InvalidDataException("Class not found");

            await _repository.DeleteAsync(classs);
            await _unitOfWork.CommitAsync(cancellationToken);
                        return true;
        }
    }
}
