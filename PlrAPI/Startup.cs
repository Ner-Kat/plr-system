using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PlrAPI.Systems;
using Microsoft.AspNetCore.CookiePolicy;

namespace PlrAPI
{
    public class Startup
    {
        public Startup()
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Использование SSL для передачи токена
                    options.RequireHttpsMetadata = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Проверка издателя токена
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,

                        // Проверка пользователя токена
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,

                        // Проверка времени жизни токена
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        // Проверка ключа
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()

                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ForRegistered", policy =>
                {
                    policy.RequireRole(Roles.AllRoles());
                });
                options.AddPolicy("ForEditors", policy =>
                {
                    policy.RequireRole(Roles.Editor, Roles.Admin, Roles.SuperAdmin);
                });
                options.AddPolicy("ForAdmins", policy =>
                {
                    policy.RequireRole(Roles.Admin, Roles.SuperAdmin);
                });
                options.AddPolicy("ForSuperAdmin", policy =>
                {
                    policy.RequireRole(Roles.SuperAdmin);
                });
            });

            services.AddDbContext<ApplicationContext>();
            services.AddTransient<AuthUtils>();

            services.AddControllers();

            services.AddSwaggerGen();

            services.AddRouting(options => options.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");

                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
