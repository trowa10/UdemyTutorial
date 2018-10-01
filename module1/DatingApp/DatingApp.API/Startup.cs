using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //.UseSqlite  can be found in Microsoft.EntityFrameworkCore
            //using nuget to install the package ctrl+shift+P
            //run EF in cmd line "dotnet ef" to sync
            //note:
            // "dotnet ef migrations add InitialCreate"  create some classess for EF core
            // "dotnet ef database update" create or update database from Migrations folder
            services
            .AddDbContext<DataContext>(
                    x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            //fix  An unhandled exception has occurred while executing the request.
            //Newtonsoft.Json.JsonSerializationException: Self referencing loop detected for property 'user' with type 'DatingApp.API.Models.User'. Path '[0].photos[0]'.
            .AddJsonOptions(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //fix to allow origins in the header
            services.AddCors();
            
            //Cloudinary Settings for phot upload
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            //used to automatically map DTO to model, installed using nuget
            services.AddAutoMapper();

            //Seed data for the first time into the database
            services.AddTransient<Seed>();
            
            //registering service using Dependency Injection
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();

            //For JWT authentication scheme for application
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.ASCII.GetBytes(this.Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false, //set to false since we are in localhost
                            ValidateAudience = false //set to false since we are in localhost
                        };
                    }
                    );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Note: this will executed if ASPNETCORE_ENVIRONMENT is set to Production
                //handle unhandled exceptions Globally!
                app.UseExceptionHandler(builder => builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null)
                    {
                        //customized exception handler using extensions
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                    }

                }));
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
           // seeder.SeedUsers(); //uncomment if you want to seed data for users and their photo
            //fix to allow origins in the header
            app.UseCors(
                x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            // For Authentication or JWT
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
