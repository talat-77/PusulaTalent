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
        private static readonly Random _random = Random.Shared;

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
            var username = await GenerateUniqueUsername(request.FirstName, request.LastName);
            var password = GeneratePassword();

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
                    ClassId = null,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };

                await _studentRepository.CreateAsync(newStudent);
                await _unitOfWork.CommitAsync(cancellationToken);

                try
                {
                    await SendWelcomeEmail(request.Email, username, password, request.FirstName, request.LastName);
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
            string studentNumber;

            do
            {
                var randomPart = _random.Next(1000, 9999).ToString();
                studentNumber = $"{year}{randomPart}";
            }
            while (await _studentRepository.GetFirstOrDefaultAsync(s => s.StudentNumber == studentNumber) != null);

            return studentNumber;
        }

        private string GeneratePassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
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
            <li><strong>Email alanına Mail adresinizi : {email} girin </li>
            <li><strong>Şifre:</strong> {password}</li>
        </ul>
        
        <p><strong>⚠️ Güvenlik için ilk girişinizde şifrenizi değiştirmeniz önerilir.</strong></p>
        
        <p>Sisteme giriş için: <a href='#'>SİSTEM_URL</a></p>
        
        <p>İyi çalışmalar dileriz.</p>
        
        <hr>
        <p><em>Okul Yönetimi</em></p>
    ";

            await _emailService.SendMailAsync(email, subject, body, true);
        }
    }
}