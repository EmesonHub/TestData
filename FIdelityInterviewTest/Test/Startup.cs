using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Test.Data;
using Test.Data.Utilities;
using Test.Data.DTO;
using Test.Data.Jobs;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard.BasicAuthorization;
using System.Diagnostics;
using Test.Data.Jobs.QuartzScheduler;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using Test.Data.Interfaces;
using Test.Data.Classes;

namespace Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();


            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Test", Version = "1.0.0" });

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentPath = Path.Combine(AppContext.BaseDirectory, xmlComments);

                x.IncludeXmlComments(xmlCommentPath);
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });


            });

            //services.AddCors();
            //services.AddDbContextPool<AppDBContext>(opt => opt.UseMySql(Configuration.GetConnectionString("Test_Conn"), new MySqlServerVersion(new Version())));

            JWTClass.JWT_Issuer = Configuration["Jwt:Issuer"];
            JWTClass.JWT_Key = Configuration["Jwt:Key"];

            AppDBContext_Setting.ConnectionString = Configuration.GetConnectionString("Test_Conn");
            services.AddDbContext<AppDBContext>();

            //Initializing the interface mapping
            services.AddScoped<IUserData, UserData>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });


            services.AddCors(options => options.AddPolicy("AllowFromAll", builder => builder.WithMethods("GET", "POST", "DELETE", "PUT").AllowAnyOrigin().AllowAnyHeader()));

            //Hangfire
            services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(AppDBContext_Setting.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            // Add the processing server as IHostedService
            services.AddHangfireServer();


            //cronjobs from ncrontab
            //services.AddHostedService<NCronTabService>();



            // Add Quartz services
            //services.AddHostedService<QuartzHostedService>();
            //services.AddSingleton<IJobFactory, SingletonJobFactory>();
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //// Add our job
            //services.AddSingleton<QuartzTask>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(QuartzTask),
            //    cronExpression: "0 39 13 ? * *")); // run by this time

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();
            app.UseCors("AllowFromAll");
            app.UseRouting();


            app.UseSwagger();

            loggerFactory.AddLog4Net(); // << Add this line

            ApplicationLogging.LoggerFactory = loggerFactory;
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "");
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHangfireDashboard();
            });

            //hangfire dashboard

            //app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {
                new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                {
                    SslRedirect=false,
                    RequireSsl=false,
                    LoginCaseSensitive=false,
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login="admin",
                            PasswordClear="1234567890"
                        }
                    }
                })
                }
            });

        }
    }
}