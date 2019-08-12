using GreenPipes.Util;
using MassTransit;
using Messages.Contract;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUnitTestProject1
{

    public class BusService :
   IHostedService
    {
        private readonly IBusControl _busControl;

        public BusService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }


    public class ConsumeObserver : Connectable<IConsumeObserver>,
         IConsumeObserver
    {

        public ConsumeObserver(IServiceProvider applicationServices)
        {
        }

        public Task PreConsume<T>(ConsumeContext<T> context)
            where T : class
        {
            return ForEachAsync(x => x.PreConsume(context));
        }

        public Task PostConsume<T>(ConsumeContext<T> context)
            where T : class
        {
            return ForEachAsync(x => x.PostConsume(context));
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception)
            where T : class
        {
            return ForEachAsync(x => x.ConsumeFault(context, exception));
        }
    }

    public class ProductMessageConsumerTest : IConsumer<SubmitProduct>, IConsumer<SubmitProduct1>
    {
        public Task Consume(ConsumeContext<SubmitProduct> context)
        {           
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<SubmitProduct1> context)
        {
            throw new NotImplementedException();
        }
    }
}
