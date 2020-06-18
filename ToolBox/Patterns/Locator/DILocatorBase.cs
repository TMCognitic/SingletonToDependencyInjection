using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Patterns.Locator
{
    public abstract class DILocatorBase
    {
        protected IServiceProvider Container { get; set; }

        public DILocatorBase()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Container = serviceCollection.BuildServiceProvider();
        }

        protected abstract void ConfigureServices(IServiceCollection serviceCollection);
    }
}
