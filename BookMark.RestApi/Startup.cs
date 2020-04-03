using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BookMark.RestApi.Databases;
using BookMark.RestApi.Services;

namespace BookMark.RestApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services) {
            string base_url = Environment.GetEnvironmentVariable("RestApiUrl");
            services.AddControllers();
            services.AddDbContext<BookMarkDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString(base_url == null ? "local" : "docker"));
            });
            services.AddScoped<OrmService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BookMarkDbContext context) {
            string base_url = Environment.GetEnvironmentVariable("RestApiUrl");
            if (base_url != null) {
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            context.Database.Migrate();
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
