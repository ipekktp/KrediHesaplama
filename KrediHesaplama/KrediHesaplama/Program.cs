using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using KrediHesaplama.Data;
using Microsoft.EntityFrameworkCore;
using KrediHesaplama.Service;

//Builder nesnesi oluþturuyoruz
var builder = WebApplication.CreateBuilder(args);

//cors ayarlarýný yapýyoruz
var MyCorsPolicy = "_myCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCorsPolicy,
                      policy =>
                      {
                          //Sadece https://localhost:4200 adresinden gelen istekleri kabul ediyoruz 
                          policy.WithOrigins("https://localhost:4200")
                                .AllowAnyHeader()  
                                .AllowAnyMethod()  
                                .AllowCredentials();
                      });
});

//Entity Framework Core için KrediDbContexti ekliyoruz ve Sqlite baðlantý dizesini kullanýyoruz
//// 'GetConnectionString' ile appsettings.json dosyasýndan "DefaultConnection" adlý baðlantý dizesini okuyoruz
builder.Services.AddDbContext<KrediDbContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// KrediService ve LoginAndSignupServicei ekliyoruz
builder.Services.AddScoped<LoginAndSignupService>();
builder.Services.AddScoped<KrediService>();

// Web API kontrolcülerini kullanmak için gerekli servisi ekliyoruz
builder.Services.AddControllers();
// Bu API uç noktalarýný otomatik olarak keþfeder.
builder.Services.AddEndpointsApiExplorer();
// Swagger dokümantasyon oluþturucuyu ekliyoruz
builder.Services.AddSwaggerGen();

// Bu nesne HTTP isteklerini iþlemek için middleware pipeline tanýmlýyor
var app = builder.Build();

//Geliþtirme ortamýnda Swagger ve SwaggerUIý etkinleþtiriyoruz
// Bu sayede API test ve dokümantasyonu için görsel bir arayüz oluþturuyoruz
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS yönlendirmesini etkinleþtiriyoruz
app.UseHttpsRedirection();

// CORS politikasýný uyguluyoruz
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Yetkilendirme middlewareini etkinleþtiriyoruz
app.UseAuthorization();

// Kontrolcüleri uygulama haritasýna ekliyoruz
// Bu gelen HTTP isteklerinin uygun kontrolcü metotlarýna yönlendirilmesini saðlýyor
app.MapControllers();

// Uygulamayý baþlatýyoruz
app.Run();
