using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PopApi.Models;
using PopApi.Services;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PopApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PopApi", Version = "v1" });
            });

            ReadData(services);

            services.AddScoped<IStatsService, StatsService>();
        }
        //Register the data read from excel(json) as singleton data
        private void ReadData(IServiceCollection services)
        {
            using StreamReader sr = new("data.json");
            var jsonString = sr.ReadToEnd();

            var popData = JsonSerializer.Deserialize<PopData>(jsonString);
            var actualStates = popData.Actuals.Select(x => x.State).ToList();
            var estimateStates = popData.Estimates.Select(x => x.State);
            actualStates.AddRange(estimateStates);

            popData.States = actualStates.Distinct().ToList();
            //Register popData as singleton data in service
            services.AddSingleton<PopData>(popData);

            //Enable cors
            services.AddCors(cfg => { cfg.AddPolicy("AllowOrigin", opt => opt.AllowAnyOrigin()); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PopApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(opt => opt.AllowAnyOrigin());
        }
    }
}
