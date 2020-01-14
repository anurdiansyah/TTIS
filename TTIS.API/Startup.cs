using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TTIS.API.Configuration;
using TTIS.API.Models;
using TTIS.API.Services;
using TTIS.API.UsersModels;

namespace TTIS.API
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
            var settings = Configuration.Get<AppSettings>();
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<TTISDbContext>(options => options.UseSqlServer(settings.ConnectionString.LocalDb));
            services.AddDbContext<LoggingContext>(options => options.UseSqlServer(settings.ConnectionString.LocalDb));
            services.AddDbContext<IS4UsersContext>(options => options.UseSqlServer(settings.ConnectionString.UsersDb));

            // We allow our routes to be in lowercase
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = settings.Auth.AuthUrl;
                        options.Audience = settings.Auth.ApiName;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters.ValidateAudience = false;
                        options.TokenValidationParameters.ValidateIssuer = false;

                    });
            services.AddAuthorization();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(1); });
            services.AddTransient<IRequestor, Requestor>();

            // Enable the use of SQL Server utilizing DI
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
