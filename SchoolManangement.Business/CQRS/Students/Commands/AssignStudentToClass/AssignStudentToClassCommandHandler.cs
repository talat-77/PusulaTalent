using AutoMapper;
using SchoolManangement.Business.CQRS.Students.Commands.AssignStudentToClass;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.Entity;
using Yonetim360Business.Mediator;

public class AssignStudentToClassCommandHandler : ICommandHandler<AssignStudentToClassCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<SchoolClass> _classRepository;
    private readonly IRepository<StudentAssignment> _assignmentRepository;
    private readonly IRepository<CourseAssignment> _courseAssignmentRepository;
    private readonly IRepository<StudentCourse> _studentCourseRepository;

    public AssignStudentToClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _studentRepository = _unitOfWork.GetRepository<Student>();
        _classRepository = _unitOfWork.GetRepository<SchoolClass>();
        _assignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
        _courseAssignmentRepository = _unitOfWork.GetRepository<CourseAssignment>();
        _studentCourseRepository = _unitOfWork.GetRepository<StudentCourse>();
    }

    public async Task<bool> Handle(AssignStudentToClassCommand request, CancellationToken cancellationToken)
    {

        var student = await _studentRepository.GetFirstOrDefaultAsync(s => s.Id == request.StudentId) ??
            throw new InvalidDataException("Student not found");

        var classEntity = await _classRepository.GetFirstOrDefaultAsync(c => c.Id == request.SchoolClassId) ??
            throw new InvalidDataException("Class not found");

        var existingAssignment = await _assignmentRepository.GetFirstOrDefaultAsync(
            x => x.StudentId == request.StudentId && x.SchoolClassId == request.SchoolClassId);

        if (existingAssignment != null)
        {
            throw new InvalidDataException("This student is already assigned to this class.");
        }

        student.SchoolClassId = request.SchoolClassId;
        await _studentRepository.UpdateAsync(student);
        // 2. StudentAssignment oluştur
        var assignment = _mapper.Map<StudentAssignment>(request);
        await _assignmentRepository.CreateAsync(assignment);

        // 3. Sınıfın tüm derslerini al
        var classAssignments = await _courseAssignmentRepository.GetAllAsync(
            filter: ca => ca.SchoolClassId == request.SchoolClassId
        );

        // 4. Öğrenciyi sınıfın tüm derslerine otomatik kaydet
        if (classAssignments.Any())
        {
            var studentCourses = new List<StudentCourse>();

            foreach (var courseAssignment in classAssignments)
            {
                // duplicate kontrolü
                var existingEnrollment = await _studentCourseRepository.GetFirstOrDefaultAsync(
                    sc => sc.StudentId == request.StudentId && sc.CourseId == courseAssignment.CourseId
                );

                if (existingEnrollment == null)
                {
                    var studentCourse = new StudentCourse
                    {
                        Id = Guid.NewGuid(),
                        StudentId = request.StudentId,
                        CourseId = courseAssignment.CourseId,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    studentCourses.Add(studentCourse);
                }
            }


            if (studentCourses.Any())
            {
                foreach (var studentCourse in studentCourses)
                {
                    await _studentCourseRepository.CreateAsync(studentCourse);
                }
            }
        }


        await _unitOfWork.CommitAsync(cancellationToken);
        return true;
    }
}