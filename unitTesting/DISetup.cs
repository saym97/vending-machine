using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unitTesting
{
    public class DISetup
    {
        protected IServiceCollection _services;
        protected IServiceProvider _serviceProvider;
        protected IConfiguration _configuration;

        public DISetup()
        {
            _services = new ServiceCollection();

            var directory = Directory.GetCurrentDirectory();
            if (directory.IndexOf(@"\bin") >= 0)
                directory = directory.Substring(0, directory.IndexOf(@"\bin"));

            _configuration = new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile(@"appsettingsTest.json", false, false)
                .AddEnvironmentVariables()
                .Build();

            _services = coreServices.Infrastructure.Config.DependencyInjection.InjectDependencies(_services, _configuration);
            _serviceProvider = _services.BuildServiceProvider();
        }
    }
}
