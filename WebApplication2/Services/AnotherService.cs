using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Services
{
    public interface IAnotherService
    {
        string Test();
    }
    public class AnotherService : IAnotherService
    {
        public string Test()
        {
            return "hello world";
        }
    }
}
