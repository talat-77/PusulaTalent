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

namespace SchoolManangement.Business.CQRS.Students.Commands.AssignStudentToClass
{
    public class AssignStudentToClassCommandHandler : ICommandHandler<AssignStudentToClassCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<SchoolClass> _classRepository;
        private readonly IRepository<StudentAssignment> _assignmentRepository;
        public AssignStudentToClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _classRepository = _unitOfWork.GetRepository<SchoolClass>();
            _assignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
        }

        public async Task<bool> Handle(AssignStudentToClassCommand request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetFirstOrDefaultAsync(s => s.Id == request.StudentId)??
                throw new InvalidDataException("not found a student");

            var classEntity = await _classRepository.GetFirstOrDefaultAsync(c => c.Id == request.SchoolClassId) ??
                throw new InvalidDataException("not found a class");

            var existingAssignments = await _assignmentRepository.GetFirstOrDefaultAsync(x => x.StudentId == request.StudentId && x.SchoolClassId == request.SchoolClassId);

            if (existingAssignments != null)
            {
                throw new InvalidDataException("This student is already assigned to this class.");
            }

            var assignment = _mapper.Map<StudentAssignment>(request);
            await _assignmentRepository.CreateAsync(assignment);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;

        }
    }
}
