using FluentValidation.AspNetCore;
using hotel_booking_api.Extensions;
using hotel_booking_api.Middleware;
using hotel_booking_data.Contexts;
using hotel_booking_data.Seeder;
using hotel_booking_models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace hotel_booking_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            StaticConfig = configuration;
            Environment = environment;
        }

        public static IConfiguration StaticConfig { get; private set; }
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            // Has the conditions for both developmenet and production environments
            services.AddDbContextAndConfigurations(Environment, Configuration);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IUrlHelper>(x =>
                    x.GetRequiredService<IUrlHelperFactory>()
                        .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            // Configure Mailing Service
            services.ConfigureMailService(Configuration);

            services.AddSingleton(Log.Logger);

            // Adds our Authorization Policies to the Dependecy Injection Container
            services.AddPolicyAuthorization();

            // Configure Identity
            services.ConfigureIdentity();

            services.AddAuthentication();

            // Add Jwt Authentication and Authorization
            services.ConfigureAuthentication(Configuration);

            // Configure AutoMapper
            services.ConfigureAutoMappers();

            // Configure Cloudinary
            services.AddCloudinary(CloudinaryServiceExtension.GetAccount(Configuration));

            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
           

            services.AddMvc().AddFluentValidation(fv => {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                fv.ImplicitlyValidateChildProperties = true;
            });

            services.AddSwagger();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            // Register Dependency Injection Service Extension
            services.AddDependencyInjection();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            HbaDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Management Api v1"));
  
            HbaSeeder.SeedData(dbContext, userManager, roleManager).GetAwaiter().GetResult();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
