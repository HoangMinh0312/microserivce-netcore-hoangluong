using GreenPipes.Util;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Consumers
{
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
}
