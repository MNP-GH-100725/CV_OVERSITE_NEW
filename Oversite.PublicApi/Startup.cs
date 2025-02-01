
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oversite.PublicApi.Service;
using System.Collections.Generic;
using System.Text;
using WebMarkupMin.AspNetCore3;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.CookiePolicy;
using Oversite.PublicApi.Utilities;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using Oversite.DTO.Response;
using System.IdentityModel.Tokens.Jwt;
using RSA_Angular_.NET_CORE.RSA;
using Maibro.Helper;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Primitives;
using Oversite.DTO.Request;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Oversite.PublicApi.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


namespace Oversite.PublicApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        static string[] corsOrigin = { "http://localhost:36440", "http://localhost:4200", "https://mafildev.mactech.net.in", "https://mafiltest.mactech.net.in", "https://qc.mactech.net.in", "https://mac.mactech.net.in", "https://gen.mactech.net.in", "http://localhost:55872", "https://uatvef.manappuram.com", "http://localhost:7500", "https://mavef.manappuram.com", "http://10.193.200.249" };
        string[] corsMethod = { "GET", "POST" };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<CustomCompressionProvider>();

            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            services.AddControllers().AddNewtonsoftJson();
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages().AddNewtonsoftJson();
            services.AddMvc().AddNewtonsoftJson();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddDistributedMemoryCache();
            services.AddTransient<TokenValidator>();
            //services.AddTransient<PreTokenValidator>();
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Custom header name
            });
            services.AddResponseCompression(options => { options.MimeTypes = MimeTypes.CommonMimeTypes; });
            if (Configuration.GetSection("ShowSwagger").Value.ToString() == "1")
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OversitePublicAPI", Version = "v2" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme.",
                    });
                    //c.CustomSchemaIds(type => type.FullName.Replace("+", ".").ToString());
                    c.AddSecurityDefinition("preauthorization", new OpenApiSecurityScheme
                    {
                        Description = "ApiKey must appear in header",
                        Type = SecuritySchemeType.ApiKey,
                        Name = "preauthorization",
                        In = ParameterLocation.Header,
                        Scheme = "ApiKeyScheme"
                    });
                    c.AddSecurityDefinition("preauthrequest", new OpenApiSecurityScheme
                    {
                        Description = "ApiKey must appear in header",
                        Type = SecuritySchemeType.ApiKey,
                        Name = "preauthrequest",
                        In = ParameterLocation.Header,
                        Scheme = "ApiKeyScheme"
                    });

                    c.AddSecurityDefinition("functionid", new OpenApiSecurityScheme
                    {
                        Description = "ApiKey must appear in header",
                        Type = SecuritySchemeType.ApiKey,
                        Name = "functionid",
                        In = ParameterLocation.Header,
                        Scheme = "ApiKeyScheme"
                    });
                    c.AddSecurityDefinition("productid", new OpenApiSecurityScheme
                    {
                        Description = "ApiKey must appear in header",
                        Type = SecuritySchemeType.ApiKey,
                        Name = "productid",
                        In = ParameterLocation.Header,
                        Scheme = "ApiKeyScheme"
                    });
                    c.AddSecurityDefinition("preauthrequestKey", new OpenApiSecurityScheme
                    {
                        Description = "ApiKey must appear in header",
                        Type = SecuritySchemeType.ApiKey,
                        Name = "preauthrequestKey",
                        In = ParameterLocation.Header,
                        Scheme = "ApiKeyScheme"
                    });


                    var Bearer = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },

                        In = ParameterLocation.Header
                    };
                    var PreAuthorization = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "preauthorization"
                        },

                        In = ParameterLocation.Header
                    };
                    var preauthrequest = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "preauthrequest"
                        },

                        In = ParameterLocation.Header
                    };

                    var functionid = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "functionid"
                        },

                        In = ParameterLocation.Header
                    };

                    var productid = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "productid"
                        },

                        In = ParameterLocation.Header
                    };
                    var preauthrequestKey = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "preauthrequestKey"
                        },

                        In = ParameterLocation.Header
                    };


                    var requirement = new OpenApiSecurityRequirement
                    {
                             { Bearer, new List<string>() },
                             { PreAuthorization, new List<string>() },
                             { preauthrequest, new List<string>() },
                             { functionid, new List<string>() },
                             { productid, new List<string>() },
                             { preauthrequestKey, new List<string>() }
                    };
                    c.AddSecurityRequirement(requirement);
                    //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    //{
                    //    {
                    //      new OpenApiSecurityScheme
                    //    {
                    //        Reference = new OpenApiReference
                    //        {
                    //            Type = ReferenceType.SecurityScheme,
                    //            Id = "Bearer"
                    //        }
                    //    },
                    //         new string[]{}
                    //    }
                    //});


                });
            }
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      builder =>
            //                      {
            //                          //builder.WithOrigins(corsOrigin).AllowAnyHeader().SetIsOriginAllowed(corsOrigin => true).WithMethods(corsMethod);
            //                          builder.WithOrigins(corsOrigin).AllowAnyHeader().SetIsOriginAllowed(corsOrigin => true).WithMethods(corsMethod);
            //                      });
            //});

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => {
                    builder.WithOrigins(corsOrigin).SetIsOriginAllowed(corsOrigin => true).WithMethods("GET", "POST").WithHeaders("authorization", "functionid", "productid", "accept", "content-type", "origin", "preauthorization", "preauthrequest", "type", "X-CSRF-TOKEN", "preauthrequestKey");
                });
            });

            services.AddStackExchangeRedisCache(options =>
            {
                //options.Configuration = "10.150.15.19:6379";
                options.Configuration = Configuration.GetSection("RedisCacheUrl").Value;

            });
            // builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            services.Configure<FormOptions>(o =>  // currently all set to max, configure it to your needs!
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue; // <-- !!! long.MaxValue
                o.MultipartBoundaryLengthLimit = int.MaxValue;
                o.MultipartHeadersCountLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
            });
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration.GetSection("DOMAIN_URL").Value,
            //        ValidAudience = Configuration.GetSection("DOMAIN_URL").Value,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"))
            //    };
            //});
            services.AddWebMarkupMin(
            options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
            })
        .AddHtmlMinification(
            options =>
            {
                options.MinificationSettings.RemoveRedundantAttributes = true;
                options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
            })
        .AddHttpCompression();

            //// Rate limiting for API calls Start

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                //options.EndpointWhitelist = new List<string> { "*:/api/General/GetValueByTypeId" };
                options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "300s",
                Limit = 300,
            }
        };
            });
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddRateLimiter(_ => _
                    .AddFixedWindowLimiter(policyName: "fixed", options =>
                    {
                        options.PermitLimit = 5;
                        options.Window = TimeSpan.FromSeconds(12);
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        options.QueueLimit = 2;
                    }));

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo($"{AppContext.BaseDirectory}/tmp-keys/"));
            //services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
            services.AddControllers().AddNewtonsoftJson();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            });


            // Rate limiting for API calls End

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseRouting();

            //app.UseCors();
            ////app.UseAuthentication();
            ////app.UseAuthorization();

            // Rate limiting
            app.UseIpRateLimiting();

            //RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);

            // Rate limiting

            //app.UseMiddleware(); // Custom Middleware

            //app.UseWebMarkupMin();
            //app.UseExceptionHandler(c => c.Run(async context =>
            //{
            //    var exception = context.Features
            //        .Get<IExceptionHandlerPathFeature>()
            //        .Error;
            //    var controllerActionDescriptor = context
            //                       .GetEndpoint()
            //                       .Metadata
            //                       .GetMetadata<ControllerActionDescriptor>();

            //    var controllerName = controllerActionDescriptor.ControllerName;
            //    var actionName = controllerActionDescriptor.ActionName;
            //    var response = new { error = exception.Message };
            //    await new ErrorLog().writeLog("controllerName:" + controllerName);
            //    await new ErrorLog().writeLog("actionName:" + actionName);
            //    await new ErrorLog().writeLog("exception:" + exception.Message);
            //    if (exception.InnerException != null)
            //    {
            //        await new ErrorLog().writeLog("InnerException:" + exception.InnerException.ToString());
            //    }
            //    await context.Response.WriteAsync(exception.Message);
            //}));
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var request = context.Request;
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                var stream = request.Body;
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await stream.ReadAsync(buffer, 0, buffer.Length);

                //get body string here...
                var requestContent = Encoding.UTF8.GetString(buffer);

                var result = JsonConvert.SerializeObject(new { error = requestContent });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);//-----error 
            }));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors();
            //app.UseResponseCompression();
            app.UseAuthorization();
            // Rate limiting
            //app.UseIpRateLimiting();
            app.UseRateLimiter();
            app.Use(async (context, next) =>

            {

                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN"); // Or this
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");  // Or this
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // Or this
                context.Response.Headers.Add("Cross-Origin-Resource-Policy", "same-origin");
                context.Response.Headers.Add("Cross-Origin-Opener-Policy", "'same-origin");
                context.Response.Headers.Add("Cross-Origin-Embedder-Policy", "require-corp");
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Add("Server", "no-referrer");

                context.Response.Headers.Remove("X-AspNet-Version");
                context.Response.Headers.Remove("X-AspNetMvc-Version");
                context.Response.Headers.Remove("Server");//------security headers

                context.Response.OnStarting(state =>
                {
                    var ctx = (HttpContext)state;

                    if (!ctx.Response.Headers.ContainsKey("Arr-Disable-Session-Affinity"))
                    {
                        ctx.Response.Headers.Add("Arr-Disable-Session-Affinity", "True"); // Disables the Azure ARRAffinity cookie
                    }

                    if (ctx.Response.Headers.ContainsKey("Server"))
                    {
                        ctx.Response.Headers.Remove("Server"); // For security reasons
                    }

                    if (ctx.Response.Headers.ContainsKey("x-powered-by") || ctx.Response.Headers.ContainsKey("X-Powered-By"))
                    {
                        ctx.Response.Headers.Remove("x-powered-by");
                        ctx.Response.Headers.Remove("X-Powered-By");
                    }

                    if (!ctx.Response.Headers.ContainsKey("X-Frame-Options"))
                    {
                        ctx.Response.Headers.Add("X-Frame-Options", "DENY");
                    }

                    return Task.FromResult(0);
                }, context);

                await next();

            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("functionid", "functionid");
                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("productid", "productid");
                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("preauthorization", "preauthorization");
                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("preauthrequest", "preauthrequest");
                await next.Invoke();
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("preauthrequestKey", "preauthrequestKey");
                await next.Invoke();
            });

            //var cookiePolicyOptions = new CookiePolicyOptions
            //{
            //    MinimumSameSitePolicy = SameSiteMode.Strict,
            //    Secure = CookieSecurePolicy.Always,
            //    HttpOnly = HttpOnlyPolicy.Always
            //};

            //app.UseExceptionHandler(a => a.Run(async context =>
            //{
            //    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            //    var exception = exceptionHandlerPathFeature.Error;

            //    var result = JsonConvert.SerializeObject(new { error = exception.Message });
            //    context.Response.ContentType = "application/json";
            //    await context.Response.WriteAsync(result);
            //}));

            app.UseMiddleware<CreateSession>();
            app.UseMiddleware<DecryptQueryMiddleware>();
            //app.UseCookiePolicy(cookiePolicyOptions);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            if (Configuration.GetSection("ShowSwagger").Value.ToString() == "1")
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "OversitePublicAPI");
                });

            }
        }
        internal static class MimeTypes
        {
            public static readonly IEnumerable<string> CommonMimeTypes = new[]
            {
        "application/javascript",
        "application/json",
        "application/ld+json",
        "application/msword",
        "application/octet-stream",
        "application/ogg",
        "application/pdf",
        "application/rtf",
        "application/vnd.apple.installer+xml",
        "application/vnd.mozilla.xul+xml",
        "application/vnd.ms-excel",
        "application/vnd.ms-fontobject",
        "application/vnd.ms-powerpoint",
        "application/vnd.oasis.opendocument.presentation",
        "application/vnd.oasis.opendocument.spreadsheet",
        "application/vnd.oasis.opendocument.text",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.visio",
        "application/x-csh",
        "application/x-sh",
        "application/x-shockwave-flash",
        "application/xhtml+xml",
        "application/xml",
        "audio/3gpp",
        "audio/3gpp2",
        "audio/aac",
        "audio/midi",
        "audio/x-midi",
        "audio/mpeg",
        "audio/ogg",
        "audio/opus",
        "audio/wav",
        "audio/webm",
        "font/otf",
        "font/ttf",
        "font/woff",
        "font/woff2",
        "image/bmp",
        "image/gif",
        "image/jpeg",
        "image/png",
        "image/svg+xml",
        "image/tiff",
        "image/vnd.microsoft.icon",
        "image/webp",
        "text/calendar",
        "text/css",
        "text/csv",
        "text/html",
        "text/javascript",
        "text/json",
        "text/plain",
        "text/xml",
        "video/3gpp",
        "video/3gpp2",
        "video/mp2t",
        "video/mpeg",
        "video/ogg",
        "video/webm",
        "video/x-msvideo",
    };

            public static bool IsInjection(string inputText)
            {
                bool isInj = false;
                Regex rgx = new Regex(@"^(?=.*SELECT.*FROM)(?!.*(?:CREATE|DROP|UPDATE|INSERT|ALTER|DELETE|ATTACH|DETACH)).*$", RegexOptions.IgnoreCase);



                inputText = inputText.ToUpper().Trim();
                if (rgx.IsMatch(inputText))
                {
                    isInj = true;
                }
                else
                {
                    isInj = false;
                }

                string regexForTypicalInj = @"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix";
                Regex reT = new Regex(regexForTypicalInj);
                if (reT.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                inputText = inputText.ToLower().Trim();

                string regexForUnion = @"/((\%27)|(\'))union/ix";
                Regex reUn = new Regex(regexForUnion);
                if (reUn.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }


                string regexForSelect = @"/((\%27)|(\'))select/ix";
                Regex reS = new Regex(regexForSelect);
                if (reS.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForInsert = @"/((\%27)|(\'))insert/ix";
                Regex reI = new Regex(regexForInsert);
                if (reI.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForUpdate = @"/((\%27)|(\'))update/ix";
                Regex reU = new Regex(regexForUpdate);
                if (reU.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForDelete = @"/((\%27)|(\'))delete/ix";
                Regex reDel = new Regex(regexForDelete);
                if (reDel.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForDrop = @"/((\%27)|(\'))drop/ix";
                Regex reDr = new Regex(regexForDrop);
                if (reDr.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForAlter = @"/((\%27)|(\'))alter/ix";
                Regex reA = new Regex(regexForAlter);
                if (reA.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForCreate = @"/((\%27)|(\'))create/ix";
                Regex reC = new Regex(regexForCreate);
                if (reC.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                string regexForOR = @"/((\%27)|(\')) or1=1 /ix";
                Regex rer = new Regex(regexForOR);
                if (rer.IsMatch(inputText))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                if (inputText.Contains("or1=1"))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                if (inputText.Contains("or 1=1"))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                if (inputText.Contains("-"))
                    isInj = true;
                else
                {
                    isInj = false;
                }
                return isInj;

            }
        }
        public class ValidateModelStateAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                if (!context.ModelState.IsValid)
                {
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }
        }
        public class CustomCompressionProvider : Microsoft.AspNetCore.ResponseCompression.ICompressionProvider
        {
            public string EncodingName => "mycustomcompression";
            public bool SupportsFlush => true;

            public Stream CreateStream(Stream outputStream)
            {
                // Replace with a custom compression stream wrapper.
                return outputStream;
            }
        }
        public class CreateSession
        {
            private readonly RequestDelegate _next;

            public CreateSession(RequestDelegate next)
            {
                this._next = next;
            }

            public async Task Invoke(HttpContext httpContext)
            {
                var request = httpContext.Request;
                var stream = request.Body;
                WebApiRequest webApiRequest = new WebApiRequest();

                if (httpContext.GetEndpoint() != null)
                {
                    var controllerActionDescriptor = httpContext
                                          .GetEndpoint()
                                          .Metadata
                                          .GetMetadata<ControllerActionDescriptor>();

                    var controllerName = controllerActionDescriptor.ControllerName;
                    var actionName = controllerActionDescriptor.ActionName;



                    if (controllerName != "CryptoManager")
                    {
                        if (controllerName != "NewDoc")
                        {
                            if (controllerName != "HdfcPremium")
                            {

                                if (request.Method == HttpMethods.Post && request.ContentLength > 0)
                                {

                                    StringValues _origin;
                                    httpContext.Request.Headers.TryGetValue("origin", out _origin);

                                    if (_origin.ToString() != null)
                                    {
                                        if (!corsOrigin.Contains(_origin.ToString()))
                                        {
                                            httpContext.Response.Clear();
                                            var responsedata = JsonConvert.SerializeObject(CreateNotResponse(_origin.ToString()));
                                            httpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                                            await httpContext.Response.WriteAsync(responsedata);

                                        }
                                    }



                                    int i = 0;

                                    //request.Body.Position = 0;  //rewinding the stream to 0
                                    StringValues PreAuthorization;
                                    StringValues preauthrequest;
                                    StringValues preauthrequestKey;

                                    httpContext.Request.Headers.TryGetValue("PreAuthorization", out PreAuthorization);
                                    httpContext.Request.Headers.TryGetValue("preauthrequest", out preauthrequest);
                                    httpContext.Request.Headers.TryGetValue("preauthrequestKey", out preauthrequestKey);
                                    var requestContent1 = (new EncryptDecryptUtil().DecodeFrom64(PreAuthorization.ToString()));
                                    string pre = preauthrequest;
                                    webApiRequest = new WebApiRequest()
                                    {
                                        apiRequest = requestContent1,
                                        apiResponse = pre
                                    };


                                    if (MimeTypes.IsInjection(requestContent1))
                                    {
                                        httpContext.Response.ContentType = "text/plain";
                                        httpContext.Response.StatusCode = 401; //UnAuthorized---sql injection blk code
                                        await httpContext.Response.WriteAsync("UnAuthorized");
                                    }

                                    var Method = httpContext.Request.Method.ToString();
                                    if (Method.ToUpper() == "POST")
                                    {
                                        if (webApiRequest != null)
                                        {

                                            if (webApiRequest != null)
                                            {
                                                if (webApiRequest.apiRequest != null && webApiRequest.apiResponse != null)
                                                {
                                                    StringValues authorizationToken;

                                                    httpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
                                                    string ob = new EncryptDecryptUtil().DecryptProperty(webApiRequest.apiRequest);



                                                    string json = Regex.Replace(ob.Replace(@"\", ""), "^\"|\"$", "");


                                                    if (controllerName == "FormValidation" && actionName == "PREFormControlData")//--- pre tocken(for before login apis)
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName == "Login" && actionName == "ProductList")
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName == "PasswordReset" && actionName == "ChangePassword")
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName == "TokenValidator" && actionName == "GetotpemployeeData")
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName == "Reports" && actionName == "PreReportProductData")
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName == "Login" && actionName == "PreBranchList")
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName.ToUpper() == ("Login").ToUpper() && actionName.ToUpper() == ("UserLogin").ToUpper())
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else if (controllerName == "TokenValidator" && actionName == "ValidateTokenWithEmpCode")
                                                    {
                                                        string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOSpre_");

                                                        if (apiRequest != webApiRequest.apiResponse)
                                                        {
                                                            httpContext.Response.Clear();
                                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                            await httpContext.Response.WriteAsync(responsedata);
                                                        }
                                                    }
                                                    else
                                                    {


                                                        if (controllerName != "CryptoManager")
                                                        {
                                                            if (controllerName != "Login" && actionName != "Preauth")
                                                            {
                                                                string apiRequest = GETREQUESTHASH(json, authorizationToken, "CVOS_");

                                                                if (apiRequest != webApiRequest.apiResponse)
                                                                {
                                                                    httpContext.Response.Clear();
                                                                    var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                                    await httpContext.Response.WriteAsync(responsedata);
                                                                }
                                                            }
                                                        }

                                                    }



                                                    var requestData = Encoding.UTF8.GetBytes(json);
                                                    stream = new MemoryStream(requestData);
                                                    request.Body = stream;

                                                }
                                                else
                                                {
                                                    httpContext.Response.Clear();
                                                    var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                    await httpContext.Response.WriteAsync(responsedata);
                                                }
                                            }
                                            else
                                            {
                                                httpContext.Response.Clear();
                                                var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                                await httpContext.Response.WriteAsync(responsedata);
                                            }
                                        }
                                        else
                                        {
                                            httpContext.Response.Clear();
                                            var responsedata = JsonConvert.SerializeObject(CreateResponse());
                                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                            await httpContext.Response.WriteAsync(responsedata);
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
                await _next(httpContext);

            }

            public bool IsBase64String(string base64)
            {
                base64 = base64.Trim();
                return (base64.Length % 4 == 0) && Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
            }
            public static string CreateRequest(string objectResult, string key)
            {
                int encCount = 3;
                string encStr1 = string.Empty;
                byte[] data = Convert.FromBase64String(objectResult.Replace(key, ""));
                string decodedString = Encoding.UTF8.GetString(data).Replace(@"\", "");
                encStr1 = Regex.Replace(decodedString, "^\"|\"$", "");
                return encStr1;

            }
            public string GETREQUESTHASH(string response, string token, string tokenType)
            {
                string _resHash = string.Empty;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var SecurityToken = handler.ReadToken(token.Replace(tokenType, "")) as JwtSecurityToken;
                var jti = SecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                string[] value = new RsaEncHelper().Decrypt(jti).Split(',');
                var eString = response + "" + value[0] + "" + token;
                _resHash = new EncryptDecryptUtil().ToMD5(eString);

                return _resHash;
            }
            public Response<string> CreateResponse(int i = 0)
            {
                Response<string> _response = new Response<string>()
                {

                    status = ResponseTypeContants.BadRequest,
                    apiStatus = ApiStatusConstants.NOT_COMPLETED,
                    responseMsg = ResponseTypeContants.FAIL,
                    Data = ResponseTypeContants.NOTVALIDREQUEST

                };
                return _response;
            }

            public Response<string> CreateNotResponse(string i)
            {
                Response<string> _response = new Response<string>()
                {

                    status = ResponseTypeContants.BadRequest,
                    apiStatus = ApiStatusConstants.NOT_COMPLETED,
                    responseMsg = ResponseTypeContants.FAIL,
                    Data = i

                };
                return _response;
            }

        }
    }
}
public class ResponseMiddleware
{
    public class ResponseClass
    {
        public int count { get; set; }

        public DateTime timestamp { get; set; }

        public string status { get; set; }

        public object results { get; set; }
        public string ErrorMessage { get; set; }
    }
    private readonly RequestDelegate _next;
    public ResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (IsSwagger(context))
            await this._next(context);
        else
        {
            var existingBody = context.Response.Body;
            using (var newBody = new MemoryStream())
            {
                context.Response.Body = newBody;
                await _next(context);
                var newResponse = await FormatResponse(context.Response);
                context.Response.Body = new MemoryStream();
                newBody.Seek(0, SeekOrigin.Begin);
                context.Response.Body = existingBody;
                var newContent = JsonConvert.DeserializeObject(new StreamReader(newBody).ReadToEnd());

                // Send modified content to the response body.
                //
                await context.Response.WriteAsync(newResponse);
            }
        }

    }

    private bool IsSwagger(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }
    private async Task<string> FormatResponse(HttpResponse response)
    {
        //We need to read the response stream from the beginning...and copy it into a string...I'D LIKE TO SEE A BETTER WAY TO DO THIS
        //
        response.Body.Seek(0, SeekOrigin.Begin);
        var content = await new StreamReader(response.Body).ReadToEndAsync();
        var Response = new ResponseClass();
        Response.status = response.StatusCode == 200 ? "success" : "Error";
        if (!IsResponseValid(response))
        {
            Response.ErrorMessage = content;
        }
        else
        {
            Response.results = content;
        }
        Response.count = response.ToString().Length;
        var json = JsonConvert.SerializeObject(Response);

        //We need to reset the reader for the response so that the client an read it
        response.Body.Seek(0, SeekOrigin.Begin);
        return $"{json}";
    }
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    public byte[] getEdata(string response, string key)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        byte[] hashedDataBytes;
        UTF8Encoding encoder = new UTF8Encoding();
        hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(response + "" + key));
        return hashedDataBytes;
    }
    private bool IsResponseValid(HttpResponse response)
    {
        if ((response != null)
            && (response.StatusCode == 200
            || response.StatusCode == 201
            || response.StatusCode == 202))
        {
            return true;
        }
        return false;
    }
}
