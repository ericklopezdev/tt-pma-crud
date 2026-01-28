using Beneficiarios.Application.DTOs;
using Beneficiarios.Application.Interfaces;
using Beneficiarios.Application.Services;
using Beneficiarios.Application.Validators;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Infrastructure.Db;
using Beneficiarios.Infrastructure.Repositories;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

// Registro de repositorios
builder.Services.AddScoped<IRepository<Beneficiarios.Domain.Entities.Beneficiario>, BeneficiarioRepository>();
builder.Services.AddScoped<IRepository<Beneficiarios.Domain.Entities.DocumentoIdentidad>, DocumentoIdentidadRepository>();

// Registro de servicios de aplicación
builder.Services.AddScoped<IDocumentoIdentidadService, DocumentoIdentidadService>();
builder.Services.AddScoped<IBeneficiarioService, BeneficiarioService>();

// Registro de validadores
builder.Services.AddScoped<IValidator<CreateBeneficiarioDto>, CreateBeneficiarioValidator>();
builder.Services.AddScoped<IValidator<UpdateBeneficiarioDto>, UpdateBeneficiarioValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
