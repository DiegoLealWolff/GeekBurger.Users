using GeekBurger.Users.Extensions;
using GeekBurger.Users.Repository;
using GeekBurger.Users.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeekBurger.Users
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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "GeekBurger.Users",
                    Description = "GeekBurguer Users Api"
                });
            });                      

            services.AddDbContext<UsersDbContext>(o => o.UseInMemoryDatabase("geekburger-users"));

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IServiceBusService, ServiceBusService>();
            services.AddScoped<IFacialServices, FacialServices>();           
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UsersDbContext usersDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint(@"/swagger/v1/swagger.json", "GeekBurguerUsers");
            });

            using (var serviceScope = app
                .ApplicationServices
                .GetService<IServiceScopeFactory>()
                .CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<UsersDbContext>();
                context.Database.EnsureCreated();
            }

            usersDbContext.Seed();            
        }
    }
}
