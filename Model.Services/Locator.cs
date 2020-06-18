using Model.Repositories;
using Model.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ToolBox.Connections;
using ToolBox.Patterns.Locator;

namespace Model.Services
{
    public sealed class Locator : LocatorBase
    {
        private static Locator _instance;

        public static Locator Instance
        {
            get
            {
                return _instance ?? (_instance = new Locator());
            }
        }

        private Locator()
        {
            Container.Register<DbProviderFactory, SqlClientFactory>();
            Container.Register<IConnectionInfo, ConnectionInfo>(() => new ConnectionInfo(@"Data Source=AW-BRIAREOS\SQL2016DEV;Initial Catalog=GestContact;Integrated Security=True;"));
            Container.Register<ILog, Log>();
            Container.Register<IConnection, Connection>();
            Container.Register<IService, Service>();
        }

        public ILog Logger
        {
            get
            {
                return Container.GetResource<ILog>();
            }
        }

        public IService Service
        {
            get
            {
                return Container.GetResource<IService>();
            }
        }

        public IConnection Connection
        {
            get
            {
                return Container.GetResource<IConnection>();
            }
        }
    }
}
