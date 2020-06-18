using Microsoft.Extensions.DependencyInjection;
using Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ToolBox.Connections;
using ToolBox.Patterns.Locator;

namespace Model.Services
{
    public class DILocator : DILocatorBase
    {
        private static DILocator _instance;

        public static DILocator Instance
        {
            get
            {
                return _instance ?? (_instance = new DILocator());
            }
        }

        private DILocator()
        {
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DbProviderFactory, SqlClientFactory>((sp) => SqlClientFactory.Instance);
            serviceCollection.AddSingleton<IConnectionInfo, ConnectionInfo>((sp) => new ConnectionInfo(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=GestContact;Integrated Security=True;"));
            serviceCollection.AddSingleton<ILog, Log>();
            serviceCollection.AddSingleton<IConnection, Connection>();
            serviceCollection.AddSingleton<IService, Service>();

            //Singleton garantit une seule instance maximum.
            //Scoped Asp MVC garantit la même instance pour le même appel
            //Transient donne systématique une instance différentes
        }

        public ILog Logger
        {
            get
            {
                return Container.GetService<ILog>();
            }
        }

        public IService Service
        {
            get
            {
                return Container.GetService<IService>();
            }
        }

        public IConnection Connection
        {
            get
            {
                return Container.GetService<IConnection>();
            }
        }
    }
}
