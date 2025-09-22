using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SchoolManangement.Business.Dto;
using SchoolManangement.Business.Helpers;
using SchoolManangement.Business.Services;
using SchoolManangement.Business.Services.Notification;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, StudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly Random _random = Random.Shared;
        private readonly IEmailNotificationService _emailNotificationService;

        public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
                                         UserManager<ApplicationUser> userManager, IEmailNotificationService emailNotificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _emailNotificationService = emailNotificationService;
        }

        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var username = await CredentialHelper.GenerateUniqueUsernameAsync(_userManager,request.FirstName, request.LastName);
            var password = CredentialHelper.GeneratePassword();

            var newUser = new ApplicationUser
            {
                UserName = username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true
            };

            var userResult = await _userManager.CreateAsync(newUser, password);
            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User oluşturulamadı: {errors}");
            }

            try
            {
                await _userManager.AddToRoleAsync(newUser, "Student");

                var newStudent = new Student
                {
                    UserId = newUser.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Age = request.Age,
                    DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
                    ClassNumber = request.ClassNumber,
                    StudentNumber = string.IsNullOrEmpty(request.StudentNumber) ? await GenerateUniqueStudentNumber() : request.StudentNumber,
                    SchoolClassId = null,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };

                await _studentRepository.CreateAsync(newStudent);
                await _unitOfWork.CommitAsync(cancellationToken);

                try
                {
                    await _emailNotificationService.SendWelcomeEmailAsync(request.Email, username, password, request.FirstName, request.LastName);
                }
                catch (Exception)
                {

                }

                var result = _mapper.Map<StudentDto>(newStudent);

                Console.WriteLine($"🎓 Student Created - Username: {username}, Password: {password}, Student Number: {newStudent.StudentNumber}");

                return result;
            }
            catch
            {
                await _userManager.DeleteAsync(newUser);
                throw;
            }
        }

     

        private async Task<string> GenerateUniqueStudentNumber()
        {
            var year = DateTime.Now.Year.ToString();
            string studentNumber;

            do
            {
                var randomPart = _random.Next(1000, 9999).ToString();
                studentNumber = $"{year}{randomPart}";
            }
            while (await _studentRepository.GetFirstOrDefaultAsync(s => s.StudentNumber == studentNumber) != null);

            return studentNumber;
        }

       

       
    }
}