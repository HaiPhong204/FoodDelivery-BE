using MyAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using MyAPI.Models;
using MyAPI.Models.Setting;
using MyAPI.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MyApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCors();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ConfigureSwaggerOptions();
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"));
});
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureRepository();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureExceptionHandler(app.Logger);
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

