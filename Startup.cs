using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.API.Data;

namespace SmartSchool.API
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
            services.AddDbContext<DataContext>(
                context => context.UseSqlite(Configuration.GetConnectionString("Default")));

            // Cria uma �nica inst�ncia do servi�o quando � solicitado pela primeira
            // vez e reutiliza essa mesma inst�ncia em todos os locais em que esse
            // servi�o � necess�rio
            // ==>>>  services.AddSingleton<IRepository, Repository>();

            // Sempre vai gerar uma nova inst�ncia para cada item encontrado que
            // possua tal depend�ncia, ou seja, se houver 5 depend�ncias, ser�o 5 inst�ncias
            // ==>>>  services.AddTransient<IRepository, Repository>();

            // Garante que em uma requisi��o seja criada uma inst�ncia de uma classe se
            // houver outras depend�ncias (renovando nas requisi��es subsequentes)
            // ==>>>  services.AddScoped<IRepository, Repository>();

            services.AddScoped<IRepository, Repository>();

            services.AddControllers()
                    .AddNewtonsoftJson(
                        opt => opt.SerializerSettings.ReferenceLoopHandling = 
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
