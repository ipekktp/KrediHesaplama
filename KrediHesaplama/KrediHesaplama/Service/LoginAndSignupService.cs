using KrediHesaplama.Models;
using KrediHesaplama.Data;
using System;

namespace KrediHesaplama.Service
{
    public class LoginAndSignupService
    {
        private readonly KrediDbContext _context;

        public LoginAndSignupService(KrediDbContext context)
        {
            // Dependency Injection ile KrediDbContexti alıyoruz
            _context = context;
        }

        // Login register ve diğer işlemler için tek bir loglama metodu oluşturuyoruz
        // UserIdnin null olabileceği durumu için int? kullanıyoruz
        public void LogIslem(int? UserId, string islemTuru, string mesaj, string seviye = "Info")
        {
            // Loglama işlemi için LoginLog modelini kullanıyoruz
            var loginlog = new LoginLog
            {
                UserId = UserId,
                islemTuru = islemTuru,
                Message = mesaj,
                Seviye = seviye,
                Timestamp = DateTime.Now
            };

            _context.LoginLogs.Add(loginlog);
            _context.SaveChanges();
        }

        // Kayıt işlemi için ayrı log kaydı oluşturuyoruz
        public void LogSignup(int UserId, string? tckn, string message)
        {
            var signupLog = new SignupLog
            {
                UserId = UserId,
                TCKN = tckn,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.SignupLogs.Add(signupLog);
            _context.SaveChanges();
        }
    }
}