using CafeApi.Custom_Authorization;
using CafeApi.Entities;
using CafeApi.Middleware;
using CafeApi.Models;
using CafeApi.Models.Validators;
using CafeApi.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeApi
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
            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));//Mo¿emy po przycinku dodaæ co k
                                                                                                    //zawiera ten claim np. German, Polak, itp.
                                                                                                    //mo¿emy te¿ dodaæ customow¹ politykê
                options.AddPolicy("AtLeast20", builder => builder.AddRequirements(new MinimumRequirementAge(20)));
            });


            services.AddScoped<IAuthorizationHandler, MinimumReqirementAgeHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirmentHandler>();//dla tego handlera nie nak³adamy osobnej polityki
            services.AddControllers().AddFluentValidation();                                //bo jego wymaganie jest bardziej dynamiczne?                            
            services.AddDbContext<CafeDbContext>();
            services.AddScoped<CafeSeeder>();
            services.AddScoped<ICafeService, CafeService>();
            services.AddScoped<IDrinkService, DrinkService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CafeApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CafeSeeder seeder)
        {
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CafeApi v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseAuthentication();

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
