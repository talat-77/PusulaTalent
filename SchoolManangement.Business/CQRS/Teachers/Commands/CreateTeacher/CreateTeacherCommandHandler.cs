using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolManangement.Business.Dto;
using SchoolManangement.Business.Services;
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
        private readonly IEmailService _emailService;

        public CreateTeacherCommandHandler(IUnitOfWork unitofWork, IMapper mapper,
                                         UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
            _userManager = userManager;
            _teacherRepository = _unitofWork.GetRepository<Teacher>();
            _emailService = emailService;
        }

        public async Task<TeacherDto> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            // Otomatik credentials üret
            var username = await GenerateUniqueUsername(request.Name, request.Surname);
            var password = GeneratePassword();

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

            await SendWelcomeEmail(request.Email, username, password, request.Name, request.Surname);

            var result = _mapper.Map<TeacherDto>(newTeacher);

            // Generated credentials'ları console'a yazdır
            Console.WriteLine($"🎓 Teacher Created - Username: {username}, Password: {password}");

            return result;
        }

        private async Task<string> GenerateUniqueUsername(string name, string surname)
        {
            var baseUsername = $"{name.ToLower()}{surname.ToLower()}";
            var username = baseUsername;
            var counter = 100;

            while (await _userManager.FindByNameAsync(username) != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            return username;
        }

        private string GeneratePassword()
        {
            // Güvenli password üretici
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private async Task SendWelcomeEmail(string email, string username, string password, string firstName, string lastName)
        {
            var subject = "Okul Sistemi - Giriş Bilgileriniz";

            var body = $@"
        <h2>Sayın {firstName} {lastName},</h2>
        
        <p>Okul yönetim sistemine hoş geldiniz!<br>
        Hesabınız başarıyla oluşturulmuştur.</p>
        
        <h3>🔑 GİRİŞ BİLGİLERİNİZ:</h3>
        <ul>
            <li><strong>Kullanıcı Adı:</strong> {username}</li>
            <li><strong>Şifre:</strong> {password}</li>
        </ul>
        
        <p><strong>⚠️ Güvenlik için ilk girişinizde şifrenizi değiştirmeniz önerilir.</strong></p>
        
        <p>Sisteme giriş için: <a href='#'>SİSTEM_URL</a></p>
        
        <p>İyi çalışmalar dileriz.</p>
        
        <hr>
        <p><em>Okul Yönetimi</em></p>
    ";

            await _emailService.SendMailAsync(email, subject, body, true); // isBodyHtml = true
        }
    }
}
