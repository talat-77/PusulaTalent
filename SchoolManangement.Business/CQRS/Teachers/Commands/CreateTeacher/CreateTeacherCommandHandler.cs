using AutoMapper;
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

namespace SchoolManangement.Business.CQRS.Teachers.Commands.CreateTeacher
{
    public class CreateTeacherCommandHandler : ICommandHandler<CreateTeacherCommand, TeacherDto>
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailNotificationService _emailNotificationService;

        public CreateTeacherCommandHandler(IUnitOfWork unitofWork, IMapper mapper,
                                         UserManager<ApplicationUser> userManager, IEmailNotificationService emailNotificationService)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _userManager = userManager;
            _teacherRepository = _unitofWork.GetRepository<Teacher>();
            _emailNotificationService = emailNotificationService;
        }

        public async Task<TeacherDto> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            // Otomatik credentials üret
            var username = await CredentialHelper.GenerateUniqueUsernameAsync(_userManager,request.Name, request.Surname);
            var password = CredentialHelper.GeneratePassword();

            // ApplicationUser oluştur
            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                FirstName = request.Name,
                LastName = request.Surname,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var userResult = await _userManager.CreateAsync(newUser, password);
            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User oluşturulamadı: {errors}");
            }

            // Teacher rolünü ekle
            await _userManager.AddToRoleAsync(newUser, "Teacher");

            // Teacher entity oluştur
            var newTeacher = new Teacher
            {
                UserId = newUser.Id,
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Department = request.Department
            };

            await _teacherRepository.CreateAsync(newTeacher);
            await _unitofWork.CommitAsync();

            await _emailNotificationService.SendWelcomeEmailAsync(request.Email, username, password, request.Name, request.Surname);

            var result = _mapper.Map<TeacherDto>(newTeacher);

            // Generated credentials'ları console'a yazdır
            Console.WriteLine($"🎓 Teacher Created - Username: {username}, Password: {password}");

            return result;
        }

      
       

    }
}
