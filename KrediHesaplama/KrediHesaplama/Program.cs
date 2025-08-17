using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using KrediHesaplama.Data;
using Microsoft.EntityFrameworkCore;
using KrediHesaplama.Service;

//Builder nesnesi olu�turuyoruz
var builder = WebApplication.CreateBuilder(args);

//cors ayarlar�n� yap�yoruz
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

//Entity Framework Core i�in KrediDbContexti ekliyoruz ve Sqlite ba�lant� dizesini kullan�yoruz
//// 'GetConnectionString' ile appsettings.json dosyas�ndan "DefaultConnection" adl� ba�lant� dizesini okuyoruz
builder.Services.AddDbContext<KrediDbContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// KrediService ve LoginAndSignupServicei ekliyoruz
builder.Services.AddScoped<LoginAndSignupService>();
builder.Services.AddScoped<KrediService>();

// Web API kontrolc�lerini kullanmak i�in gerekli servisi ekliyoruz
builder.Services.AddControllers();
// Bu API u� noktalar�n� otomatik olarak ke�feder.
builder.Services.AddEndpointsApiExplorer();
// Swagger dok�mantasyon olu�turucuyu ekliyoruz
builder.Services.AddSwaggerGen();

// Bu nesne HTTP isteklerini i�lemek i�in middleware pipeline tan�ml�yor
var app = builder.Build();

//Geli�tirme ortam�nda Swagger ve SwaggerUI� etkinle�tiriyoruz
// Bu sayede API test ve dok�mantasyonu i�in g�rsel bir aray�z olu�turuyoruz
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS y�nlendirmesini etkinle�tiriyoruz
app.UseHttpsRedirection();

// CORS politikas�n� uyguluyoruz
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Yetkilendirme middlewareini etkinle�tiriyoruz
app.UseAuthorization();

// Kontrolc�leri uygulama haritas�na ekliyoruz
// Bu gelen HTTP isteklerinin uygun kontrolc� metotlar�na y�nlendirilmesini sa�l�yor
app.MapControllers();

// Uygulamay� ba�lat�yoruz
app.Run();
