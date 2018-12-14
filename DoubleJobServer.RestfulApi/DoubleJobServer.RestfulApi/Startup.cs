using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DoubleJobServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DoubleJobServer.Infrastructure.Repositories;
using DoubleJobServer.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;
using FluentValidation;
using FluentValidation.AspNetCore;
using DoubleJobServer.Infrastructure.Resources;
using DoubleJobServer.RestfulApi.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

//ASP.NET Core 服务器的作用是响应客户端发过来的请求, 这些请求会作为HttpContext传递进来  Kestrel它是跨平台的服务器, 基于Libuv.可以作为一个独立进程自行托管, 也可以在IIS里. 但是还是建议使用IIS或Nginx等作为反向代理服务器. 在构建API或微服务时, 这些服务器可以作为网关使用, 因为它们会限制对外暴露的东西也可以更好的与现有系统集成, 所以它们会提供额外的防御层

namespace DoubleJobServer.RestfulApi
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public static IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Serilog支持把日志写入到各种的Sinks里，可以把sink看做媒介（文件，数据库等）
            //全局最低记录日志级别是Debug，但是针对以Microsoft开头的命名空间的最低级别是Information。使用Enruch.FromLogContext()可以让程序在执行上下文时动态添加或移除属性（这个需要看文档）。 按日生成记录文件，日志文件名后会带着日期，并放到./logs目录下
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .Enrich.FromLogContext()
              .WriteTo.Console()   //控制台和文件的Log都可以输出
              .WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
              .CreateLogger();

            services.AddScoped<ICountryRepository, CountryRepository>();

            services.AddScoped<ICityRepository, CityRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper();

            //注册DbContext, 使用的是内存数据库
            services.AddDbContext<MyContext>(options =>
            {
                options.UseInMemoryDatabase("MyDatabase");
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                 //为项目添加Xml输出格式的支持
                 options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            })
              .AddFluentValidation();

            //为每一个Resource Model 配置验证器
            services.AddTransient<IValidator<CityAddResource>, CityAddOrUpdateResourceValidator<CityAddResource>>();
            services.AddTransient<IValidator<CityUpdateResource>, CityUpdateResourceValidator>();
            services.AddTransient<IValidator<CountryAddResource>, CountryAddResourceValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();

            //创建Logger并记录日志
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var logger = _loggerFactory.CreateLogger("Global Exception Logger");
                        logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                    }
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(exceptionHandlerFeature?.Error?.Message ?? "An Error Occurred.");
                });
            }
            );


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An error occured.");
                    });
                });
            }

            app.UseMvc();
        }
    }
}
