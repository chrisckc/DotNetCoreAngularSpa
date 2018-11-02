using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetCoreAngularSpa {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) => {
                logRequest(logger, context, "app");
                await next();
                logResponse(logger, context, "app");
            });

            // commented out to simplify analysis of the Spa middleware
            //app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.Use(async (context, next) => {
                logRequest(logger, context, "app after UseSpaStaticFiles");
                await next();
                logResponse(logger, context, "app after UseSpaStaticFiles");
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa => {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                app.Use(async (context, next) => {
                    logRequest(logger, context, "inside app.UseSpa");
                    await next();
                    logResponse(logger, context, "inside app.UseSpa");
                });

                if (env.IsDevelopment()) {
                    // UseAngularCliServer starts the webpack dev server automatically
                    spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer(baseUri: "http://localhost:4200"); // Alternative for Angular
                }
            });

            app.Use(async (context, next) => {
                logRequest(logger, context, "app after app.UseSpa");
                await next();
                logResponse(logger, context, "app after app.UseSpa");
            });
        }

        private void logRequest(ILogger logger, HttpContext context, string description) {
            if (context.Request != null) {
                var builder = new System.Text.StringBuilder()
                    .AppendLine($"{description} context.Request.Protocol: {context.Request.Protocol}")
                    .AppendLine($"{description} context.Request.Method: {context.Request.Method}")
                    .AppendLine($"{description} context.Request.Scheme: {context.Request.Scheme}")
                    .AppendLine($"{description} context.Request.Host: {context.Request.Host}")
                    .AppendLine($"{description} context.Request.PathBase: {context.Request.PathBase}")
                    .AppendLine($"{description} context.Request.Path: {context.Request.Path}")
                    .AppendLine($"{description} context.Request.QueryString: {context.Request.QueryString}");
                logger.LogInformation(builder.ToString());
            } else {
                logger.LogInformation($"{description} context.Request is null");
            }
        }

        private void logResponse(ILogger logger, HttpContext context, string description) {
            if (context.Response != null) {
                var builder = new System.Text.StringBuilder()
                    .Append($"{description} context.Response.StatusCode: {context.Response.StatusCode}")
                    .AppendLine($" Url: {context.Request?.Path.Value}");
                logger.LogInformation(builder.ToString());
            } else {
                logger.LogInformation($"{description} context.Response is null");
            }
        }
    }
}