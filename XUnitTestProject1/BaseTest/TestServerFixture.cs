using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using WebApplication2;

namespace XUnitTestProject1.BaseTest
{
    public class TestServerFixture: IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }


        public static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;

            var applicationBasePath = AppContext.BaseDirectory;

            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));

                if (projectDirectoryInfo.Exists)
                    if (new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj")).Exists)
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        public TestServerFixture()
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json");

            var builder = new WebHostBuilder()
  .UseContentRoot(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
  .UseConfiguration(configurationBuilder.Build())//.UseEnvironment("Development")
                   .UseStartup<StartUpTest>();  // Uses Start up class from your API Host project to configure the test server

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();

        }      

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
