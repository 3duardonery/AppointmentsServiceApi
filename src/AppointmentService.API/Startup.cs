using AppointmentService.IoC.Database;
using AppointmentService.IoC.MapperProfile;
using AppointmentService.IoC.Services;
using AppointmentService.Shared.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AppointmentService.API
{
    public class Startup
    {
        private AppSettings _appSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _appSettings = new AppSettings
            {
                ConnectionString = Configuration.GetValue<string>("ConnectionString"),
                Database = Configuration.GetValue<string>("Database")
            };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddServicesInjection();

            services.AddSingleton(_appSettings);

            services.AddMongoDBConfiguration(_appSettings);

            services.AddMapperProfileConfiguration();

            services.AddSwaggerGen(options =>
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


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
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
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}