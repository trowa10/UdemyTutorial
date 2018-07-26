using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //fix to allow origins in the header
            services.AddCors();

            //registering service using Dependency Injection
            services.AddScoped<IAuthRepository, AuthRepository>();

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();

            //fix to allow origins in the header
            app.UseCors(
                x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            // For Authentication or JWT
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
