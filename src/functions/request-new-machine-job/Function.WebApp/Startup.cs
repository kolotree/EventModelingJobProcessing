using System;
using EventStore.Client;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.EventStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Function.WebApp
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

            var store = EventStoreBuilder
                .NewUsing(
                    new EventStoreConfiguration(
                        Environment.GetEnvironmentVariable("EventStore_ConnectionString")!,
                        new UserCredentials(
                            Environment.GetEnvironmentVariable("EventStore_UserName")!,
                            Environment.GetEnvironmentVariable("EventStore_Password")!)))
                .NewStore();
            services.Add(new ServiceDescriptor(typeof(IStore), store));         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}