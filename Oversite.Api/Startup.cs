using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
using System;
using Newtonsoft.Json;

namespace Oversite.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        string[] corsOrigin = { "http://localhost:54668", "http://localhost:4200", "http://localhost:44326", "https://mafildev.mactech.net.in", "https://macserv.mactech.net.in", "https://qc.mactech.net.in", "https://mac.mactech.net.in", "https://gen.mactech.net.in", "http://localhost:55872" };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OversiteAPI", Version = "v1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                //c.CustomSchemaIds(type => type.FullName.Replace("+", ".").ToString());
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                                  });
            });
            services.AddStackExchangeRedisCache(options =>
            {
                //options.Configuration = "10.150.15.19:6379";
                options.Configuration = Configuration.GetConnectionString("RedisCacheUrl");
                //options.Configuration = "10.150.10.147:6379"; test
            });

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var request = context.Request;
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                var stream = request.Body;
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await stream.ReadAsync(buffer, 0, buffer.Length);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";
                var ex = context.Features.Get<IExceptionHandlerFeature>();
                if (ex != null)
                {
                    var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";

                }


                //get body string here...
                var requestContent = Encoding.UTF8.GetString(buffer);
                var result = JsonConvert.SerializeObject(new { UseException = requestContent });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Oversite");
            });


        }
    }
}
