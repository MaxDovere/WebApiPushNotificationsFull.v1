using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiIdentityServer.BusinessLayer.Swagger;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiIdentityServer.BusinessLayer.Services;
using WebApiIdentityServer.BusinessLayer.Services.IdentityServerPersonal;
using WebApiPushNotifications.Services;
using WebApiPushNotifications.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApiPushNotifications
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
            services.AddPushServiceStore(Configuration)
                .AddWebPushNotificationService(Configuration)
                .AddWebPushQueue();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddControllers(options =>
            {
                options.InputFormatters.Add(new TextPlainInputFormatter());
            });

            services.AddSwaggerGen(options => 
                SwaggerGenOptionsService.AddSwaggerOptions(options)
            );

            services.AddAuthenticationOptions(Configuration);

            services.AddScoped<IIdentityService, IdentityService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
#if DEBUG
            // For Debug in Kestrel (IISEXpress)
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/api-webpush/swagger/v2/swagger.json", "WebApiPushNotifications v2"));
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiPushNotifications v2"));
#else
            // To deploy on IIS
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/api-webpush/swagger/v1/swagger.json", "WebApiPushNotifications v1"));
#endif

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

            //app.UseCors();
            app.UseCors("CorsPolicy");

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                //MissingMemberHandling = MissingMemberHandling.Ignore,
                //NullValueHandling = NullValueHandling.Ignore
            };
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UsePushStore();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api-webpush");
            });
        }
    }
}
