using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManangement.Business.Services.Notification
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailService _emailService;

        public EmailNotificationService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendWelcomeEmailAsync(string email, string username, string password, string firstName, string lastName)
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

