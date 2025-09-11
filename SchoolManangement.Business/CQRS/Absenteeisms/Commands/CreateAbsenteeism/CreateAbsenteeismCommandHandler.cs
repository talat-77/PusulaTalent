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

namespace SchoolManangement.Business.CQRS.Absenteeisms.Commands.CreateAbsenteeism
{
    public class CreateAbsenteeismCommandHandler : ICommandHandler<CreateAbsenteeismCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Absenteeism> _absentRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public CreateAbsenteeismCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _absentRepository = _unitOfWork.GetRepository<Absenteeism>();
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();

        }

        public async Task<bool> Handle(CreateAbsenteeismCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.Id == request.UserId)
                ?? throw new InvalidDataException("User not found");

            var student = await _studentRepository.GetFirstOrDefaultAsync(s => s.Id == request.StudentId)??
                throw new InvalidDataException("Student not found");

            var absenteeism = _mapper.Map<Absenteeism>(request);
            await _absentRepository.CreateAsync(absenteeism);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;

        }
    }
}
