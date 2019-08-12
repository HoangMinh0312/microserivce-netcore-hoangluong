using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MassTransit;
using Messages.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository;
using WebApplication2.Config;
using WebApplication2.Filters;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2
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
            services.AddAutoMapper();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAnotherService, AnotherService>();
            services.Configure<MySettings>(Configuration.GetSection("mysettings"));

            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h => {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            EndpointConvention.Map<SubmitProduct>(new Uri("rabbitmq://localhost/order_processing"));

            services.AddSingleton<IPublishEndpoint>(bus);
            services.AddSingleton<ISendEndpointProvider>(bus);
            services.AddSingleton<IBus>(bus);

            services.AddMvc(op=>
            {
                op.Filters.Add<TheFirstFilter>(2);
                op.Filters.Add<TheSecondFilter>(1);
                op.Filters.Add<ApiExceptionFilter>(1);

            }).AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });
            });

            services.AddDbContext<Service1DbContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddScoped<DbContext>(provider =>
            {
                //resolve another classes from DI
                var anyOtherClass = provider.GetService<Service1DbContext>();
                //pass any parameters
                return anyOtherClass;
            });

            RegisMassTransit(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseExceptionHandler();

            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public virtual void RegisMassTransit(IServiceCollection services)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h => {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            EndpointConvention.Map<SubmitProduct>(new Uri("rabbitmq://localhost/order_processing"));

            services.AddSingleton<IPublishEndpoint>(bus);
            services.AddSingleton<ISendEndpointProvider>(bus);
            services.AddSingleton<IBus>(bus);
        }
    }
}
