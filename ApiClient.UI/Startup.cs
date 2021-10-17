using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.Extensions.DependencyInjection;
using ApiClient.MarketResearch.Services;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Facade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApiClient.UI.Data;

namespace ApiClient.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options => {  options.DetailedErrors = true; });
            services.AddSingleton<WeatherForecastService>();

            var key = Configuration["apiKey"];
            var url = Configuration["apiUrl"];
            services.AddSingleton(new ApiConfig(key, url));
            services.AddHttpClient();
            services.AddTransient<IMakelaar, Makelaar>();
            services.AddTransient<ISearchApi, ApiClientFacade>();
            services.AddTransient<ApiCoordinatorFactory>();
            services.AddTransient<ApiCoordinator>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            var searchApi = app.ApplicationServices.GetRequiredService<ISearchApi>();
            var actorSystem = ActorSystem.Create("apiclient");
            actorSystem.UseServiceProvider(app.ApplicationServices);
            ActorSystemRefs.ActorSystem = actorSystem;
        }
    }
}
