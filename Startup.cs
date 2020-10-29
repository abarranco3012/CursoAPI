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

            // Cria uma única instância do serviço quando é solicitado pela primeira
            // vez e reutiliza essa mesma instância em todos os locais em que esse
            // serviço é necessário
            // ==>>>  services.AddSingleton<IRepository, Repository>();

            // Sempre vai gerar uma nova instância para cada item encontrado que
            // possua tal dependência, ou seja, se houver 5 dependências, serão 5 instâncias
            // ==>>>  services.AddTransient<IRepository, Repository>();

            // Garante que em uma requisição seja criada uma instância de uma classe se
            // houver outras dependências (renovando nas requisições subsequentes)
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
