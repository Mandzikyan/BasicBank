using FCBankBasicHelper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BL.Core;
using WebAPI.Domain;
using Serilog;
using WebAPI.Sinks;
using BL.Repositories.Interfaceis;
using BL.Repositories;
using BL.Repository;
using BL;
using BL.MailConfirmation;
using BL.Configuration;
using BL.Validationn.Interfaces;
using WebAPI;
using BL.Encrypt;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddDbContext<FcbankBasicContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=FC-PROG-43\\MSSQLSERVER02;Database=FCBankBasic;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<FcbankBasicContext>()
    .AddDefaultTokenProviders();
//builder.Services.AddHostedService<Worker>();
var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.CustomSink()
        .Enrich.FromLogContext()
        .CreateLogger();
Injecton(builder);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

TokenExtension.AddTokenDocumentation(services, builder);
var app = builder.Build();
var serviceProvider = builder.Services.BuildServiceProvider();
var tokenService = serviceProvider.GetService<TokenService>();
TokenExtension.UseTokenDocumentation(app, tokenService);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void Injecton(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<UserManager<IdentityUser>>();
    builder.Services.AddScoped<TokenBL>();
    builder.Services.AddSingleton<IConfigurationKey, ConfigurationKey>();
    builder.Services.AddScoped<TokenService>(provider =>
    {
        var tokenbl = provider.GetService<TokenBL>();
        return new TokenService(provider.GetService<IConfiguration>(), tokenbl);
    });

    builder.Services.AddScoped<UserBL>();
    builder.Services.AddScoped<CustomerBl>();
    builder.Services.AddScoped<PhoneBl>();
    builder.Services.AddScoped<RoleBl>();
    builder.Services.AddSingleton<ConfigBl>();

    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
    builder.Services.AddScoped<IRoleRepository,RoleRepository>();
    builder.Services.AddScoped<ITokenRepository,TokenRepository>();

    builder.Services.AddSingleton<IValidation, Validation>();
    builder.Services.AddSingleton<IEncryption,Encryption>();
    builder.Services.AddSingleton<VerificationCode>();
}