using AutoMapper;
using MediatR;
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

namespace SchoolManangement.Business.CQRS.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, StudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Student> _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
                                         UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _emailService = emailService;
        }

        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            // Otomatik credentials üret
            var username = await GenerateUniqueUsername(request.FirstName, request.LastName);
            var password = GeneratePassword();

            // ApplicationUser oluştur
            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                FirstName = request.FirstName,
                LastName = request.LastName,
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

            // Student rolünü ekle
            await _userManager.AddToRoleAsync(newUser, "Student");

            // Student entity oluştur
            var newStudent = new Student
            {
                UserId = newUser.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                DateOfBirth = request.DateOfBirth,
                ClassNumber = request.ClassNumber,
                StudentNumber = string.IsNullOrEmpty(request.StudentNumber) ? await GenerateUniqueStudentNumber() : request.StudentNumber,
                ClassId = request.ClassId,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _studentRepository.CreateAsync(newStudent);
            await _unitOfWork.CommitAsync(cancellationToken);

            await SendWelcomeEmail(request.Email, username, password, request.FirstName, request.LastName);

            var result = _mapper.Map<StudentDto>(newStudent);

            // Generated credentials'ları console'a yazdır
            Console.WriteLine($"🎓 Student Created - Username: {username}, Password: {password}, Student Number: {newStudent.StudentNumber}");

            return result;
        }

        private async Task<string> GenerateUniqueUsername(string firstName, string lastName)
        {
            var baseUsername = $"{firstName.ToLower()}{lastName.ToLower()}";
            var username = baseUsername;
            var counter = 100;

            while (await _userManager.FindByNameAsync(username) != null)
            {
                username = $"{baseUsername}{counter}";
                counter++;
            }

            return username;
        }

        private async Task<string> GenerateUniqueStudentNumber()
        {
            var year = DateTime.Now.Year.ToString();
            var random = new Random();
            string studentNumber;
            
            do
            {
                var randomPart = random.Next(1000, 9999).ToString();
                studentNumber = $"{year}{randomPart}";
            }
            while (await _studentRepository.GetFirstOrDefaultAsync(s => s.StudentNumber == studentNumber) != null);

            return studentNumber;
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
            var subject = "Okul Sistemi - Öğrenci Giriş Bilgileriniz";

            var body = $@"
        <h2>Sayın {firstName} {lastName},</h2>
        
        <p>Okul yönetim sistemine hoş geldiniz!<br>
        Öğrenci hesabınız başarıyla oluşturulmuştur.</p>
        
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