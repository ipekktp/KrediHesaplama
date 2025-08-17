using KrediHesaplama.Models;
using KrediHesaplama.Service;
using Microsoft.AspNetCore.Mvc;
using KrediHesaplama.Data;
using System.Linq;
using BCrypt.Net;
using KrediHesaplama.Models.Requests;

namespace KrediHesaplama.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Tüm kullanıcı işlemleri için temel rota
    public class UserController : ControllerBase
    {
        private readonly KrediDbContext _context;
        private readonly LoginAndSignupService _logger;

        public UserController(KrediDbContext context, LoginAndSignupService logger)
        {
            // Dependency Injection ile KrediDbContext ve LoginAndSignupService'i alıyoruz
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")] // Login işlemi için HTTP POST metodu kullanıyoruz
        public IActionResult Login([FromBody] LoginRequest loginUser)
        {
            // TCKNye sahip kullanıcıyı veritabanından alıyoruz
            var user = _context.Users.FirstOrDefault(u => u.TCKN == loginUser.TCKN);

            // Kullanıcı bulunamazsa veya şifre yanlışsa
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Sifre, user.Sifre))
            {
                // user null olabileceğinden UserIdyi güvenli bir şekilde atıyoruz
                int userIdForLog = user?.UserId ?? 0;

                // Başarısız giriş denemesini logluyoruz
                _logger.LogIslem(
                    UserId: userIdForLog,
                    islemTuru: "Login",
                    mesaj: $"Başarısız giriş denemesi: TCKN = {loginUser.TCKN}",
                    seviye: "Warning");

                return Unauthorized("Geçersiz TCKN veya şifre");
            }

            // Başarılı giriş durumunu logluyoruz
            _logger.LogIslem(
                UserId: user.UserId,
                islemTuru: "Login",
                mesaj: $"Giriş başarılı: UserId = {user.UserId}, TCKN = {user.TCKN}",
                seviye: "Info");

            // Başarılı giriş sonrası kullanıcı bilgilerini döndürüyoruz
            return Ok(new
            {
                UserId = user.UserId,
                tckn = user.TCKN,
                adSoyad = user.adSoyad,
            });
        }

        [HttpPost("register")] // Register işlemi için HTTP POST metodu kullanıyoruz
        public IActionResult Register([FromBody] User newUser)
        {
            // Yeni kullanıcının bilgilerini kontrol ediyoruz
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.TCKN) ||
                string.IsNullOrWhiteSpace(newUser.Sifre) || string.IsNullOrWhiteSpace(newUser.adSoyad))
            {
                return BadRequest("Eksik kullanıcı bilgisi.");
            }

            try
            {
                // TCKNye sahip kullanıcı zaten varsa hata mesajı döndürüyoruz
                if (_context.Users.Any(u => u.TCKN == newUser.TCKN))
                {
                    _logger.LogIslem(
                        UserId: 0,
                        islemTuru: "Register",
                        mesaj: $"Zaten kayıtlı kullanıcı denemesi: TCKN = {newUser.TCKN}",
                        seviye: "Warning");

                    return BadRequest("Bu TCKN ile zaten bir kullanıcı kayıtlı.");
                }

                //Kullanıcı şifresini hashliyoruz
                newUser.Sifre = BCrypt.Net.BCrypt.HashPassword(newUser.Sifre);

                // Yeni kullanıcıyı veritabanına ekliyoruz
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Yeni kullanıcı kaydı sonrası log kaydı yapıyoruz
                _logger.LogIslem(
                    UserId: newUser.UserId,
                    islemTuru: "Register",
                    mesaj: $"Yeni kullanıcı kaydı: TCKN = {newUser.TCKN}, adSoyad = {newUser.adSoyad}",
                    seviye: "Info");

                // Kullanıcı kayıt işlemi için ayrı log kaydı yapıyoruz
                _logger.LogSignup(
                    UserId: newUser.UserId,
                    tckn: newUser.TCKN,
                    message: "Kullanıcı başarıyla kayıt oldu.");

                return Ok(new
                {
                    mesaj = "Kayıt başarılı.",
                    userId = newUser.UserId
                });
            }
            // Hata durumunda loglama yapıyoruz
            catch (Exception ex)
            {
                // Hata mesajını logluyoruz
                _logger.LogIslem(
                    UserId: 0,
                    islemTuru: "Register",
                    mesaj: $"Hata: {ex.Message}",
                    seviye: "Error");

                return StatusCode(500, "Sunucu hatası: " + ex.Message);
            }
        }
    }
}