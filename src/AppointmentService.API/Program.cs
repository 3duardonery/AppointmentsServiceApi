using AppointmentService.IoC.Database;
using AppointmentService.IoC.MapperProfile;
using AppointmentService.IoC.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.Settings;
using AppointmentService.Shared.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
string CorsRuleName = "customCors";

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console(LogEventLevel.Error)
                .CreateLogger();

AppSettings _appSettings = new AppSettings
{
    ConnectionString = builder.Configuration.GetValue<string>("ConnectionString"),
    Database = builder.Configuration.GetValue<string>("Database"),
    AuthEndpoint = builder.Configuration.GetValue<string>("AuthEndpoint"),
    FirebaseToken = builder.Configuration.GetValue<string>("FirebaseToken"),
    ProjectId = builder.Configuration.GetValue<string>("ProjectId"),
};

builder.Services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );

builder.Services
 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.Authority = $"https://securetoken.google.com/{_appSettings.ProjectId}";
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidIssuer = $"https://securetoken.google.com/{_appSettings.ProjectId}",
         ValidateAudience = true,
         ValidAudience = _appSettings.ProjectId,
         ValidateLifetime = true
     };
 });

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: CorsRuleName,
    policy =>
    {
        policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
    .AllowAnyMethod();
    });
});
builder.Services.AddFluentValidation();

builder.Services.AddTransient<IValidator<ProfessionalDto>, ProfessionalValidator>();
builder.Services.AddTransient<IValidator<AuthenticationRequestDto>, AuthenticationRequestValidator>();

builder.Services.AddSingleton(_appSettings);

builder.Services.AddServicesInjection(_appSettings);

builder.Services.AddMongoDBConfiguration(_appSettings);

builder.Services.AddMapperProfileConfiguration();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Title = "Swagger Demo Documentation",
        Version = "v1",
        Description = "This is a demo to see how documentation can easily be generated for ASP.NET Core Web APIs using Swagger and ReDoc.",
        Contact = new OpenApiContact
        {
            Name = "Christian Schou",
            Email = "someemail@somedomain.com"
        }
    });
});

builder.Services.AddSingleton(Log.Logger);

builder.Services.AddLogging(builder =>
{
    builder.AddSerilog(dispose: true);
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(options =>
    options.SwaggerEndpoint("/swagger/v1/swagger.json",
    "Swagger Demo Documentation v1"));

app.UseReDoc(options =>
{
    options.DocumentTitle = "Swagger Demo Documentation";
    options.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(CorsRuleName);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();