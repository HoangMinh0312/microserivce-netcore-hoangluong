using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using Messages.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication2;

namespace XUnitTestProject1
{
    public class StartUpTest : Startup
    {
        public StartUpTest(IConfiguration configuration) : base(configuration)
        {
        }

        public override void RegisMassTransit(IServiceCollection services)
        {
            services.AddSingleton<ConsumeObserver>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductMessageConsumerTest>();

                x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.Host.ConnectConsumeObserver(services.BuildServiceProvider().GetService<ConsumeObserver>());
                    cfg.ReceiveEndpoint("order_processing", ep =>
                    {
                        ep.ConfigureConsumer<ProductMessageConsumerTest>(provider);
                    });

                    cfg.ConfigureEndpoints(provider);

                }));

            });
            services.AddSingleton<IHostedService, BusService>();

            EndpointConvention.Map<SubmitProduct>(new Uri("loopback://localhost/order_processing"));
            //services.AddSingleton<IPublishEndpoint>(bus);
            //services.AddSingleton<ISendEndpointProvider>(bus);
            //services.AddSingleton<IBus>(bus);
        }
    }
}
