using Congress.Api.HubDispatcher;
using Congress.Api.Hubs;
using Congress.Core.Interface;
using Congress.Data.Data;
using Congress.Data.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Congress.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();         

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,                    
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("CongressBackendApi"))
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Exception:{0}", context.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Congress", new Info
                {
                    Title = "Congress Web Api",
                    Version = "1.0.0",
                    Description = "Congress Web Api",
                    TermsOfService = "http://swagger.io/terms/"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile));
            });

            services.AddSignalR();

            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IMethod, SMethod>();
            services.AddScoped<IJob, SJob>();
            services.AddScoped<ICountry, SCountry>();
            services.AddScoped<ICity, SCity>();
            services.AddScoped<IUser, SUser>();
            services.AddScoped<IMinio, SMinio>();
            services.AddScoped<IMenu, SMenu>();
            services.AddScoped<ISponsor, SSponsor>();
            services.AddScoped<IEvent, SEvent>();
            services.AddScoped<IEventDetail, SEventDetail>();
            services.AddScoped<IEventParticipant, SEventParticipant>();
            services.AddScoped<ICategory, SCategory>();
            services.AddScoped<ICategory, SCategory>();
            services.AddScoped<IEventCategory, SEventCategory>();
            services.AddScoped<IEventSponsor, SEventSponsor>();
            services.AddScoped<IUserInterest, SUserInterest>();

            services.AddSingleton<INotificationDispatcher, NotificationDispatcher>();       
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger()
           .UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("../swagger/Congress/swagger.json", "Congress Api");
           });

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}
