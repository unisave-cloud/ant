using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Antling.Common;
using Antling.Docker;
using Antling.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Antling
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IDocker docker = new DockerViaBash();
            
            IImageBuilder imageBuilder = new ImageBuilder(docker);
            imageBuilder.Initialize();
            
            JobRunner jobRunner = new JobRunner(docker, imageBuilder);

            services.AddSingleton<IDocker>(docker);
            services.AddSingleton<IImageBuilder>(imageBuilder);
            services.AddSingleton<JobRunner>(jobRunner);
            
            // TODO: remove old docker images and containers on startup and disposal
            
            services.AddControllers();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateWorkingDirectory();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection(); // No, don't

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void UpdateWorkingDirectory()
        {
            Directory.SetCurrentDirectory(
                Path.Combine(
                    Environment.CurrentDirectory,
                    Configuration["WorkingDirectory"] ?? "./"
                )
            );
        }
    }
}