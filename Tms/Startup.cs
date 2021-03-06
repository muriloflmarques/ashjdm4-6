using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Tms.Infra.CrossCutting.Configurations;
using Tms.Infra.Data;
using Tms.Infra.Data.Interface;
using Tms.Service;
using Tms.Service.Interfaces;

namespace Tms
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adding Log only when no prod
            if (!this.HostEnvironment.IsProduction())
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole()
                        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
                    loggingBuilder.AddConsole()
                        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Error);
                    loggingBuilder.AddConsole()
                        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning);
                    loggingBuilder.AddDebug();
                });
            }
            services.AddDbContext<TmsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("ConnectionsStrings:TmsTasks_Dev_ConnectionString").Value);

                //Enble Sensitive log only when not production
                if (!this.HostEnvironment.IsProduction())
                    options.EnableSensitiveDataLogging(true);
            });

            services.AddCors();
            services.AddControllers();

            //XML for documenation - Must include <GenerateDocumentationFile>true</GenerateDocumentationFile> in CSPROJ
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Teste Login com JWT",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                //If file exisits, add documentation in Swagger page
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<TmsDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddScoped<ITaskService, TaskService>();

            services.Configure<DefaultQueryConfigurations>(options =>
                Configuration.GetSection("DefaultQueryConfigurations")
                .Bind(options, c => c.BindNonPublicProperties = true));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Teste API V1");
            });
        }
    }
}