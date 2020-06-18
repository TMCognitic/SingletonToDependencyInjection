using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using ToolBox.Connections;

namespace Models.Services
{
    public sealed class DIService : IDisposable, IDIService
    {
        private ILogger _logger;
        private IConnection _connection;
        private bool disposedValue;

        public DIService(ILogger<DIService> logger, IConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public void DoSomething()
        {
            if (disposedValue)
                throw new ObjectDisposedException(GetType().Name);

            _logger.LogInformation($"Service - {GetHashCode()}");
            //Je fais quelque chose
            Command command = new Command("Select SysDateTime();");
            _logger.LogInformation($"Connection - {_connection.GetHashCode()}");
            DateTime current = (DateTime)_connection.ExecuteScalar(command);

            _logger.LogInformation($"{current.ToLongTimeString()} : Call de DoSomething");
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _logger = null;
                _connection = null;
                disposedValue = true;
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~Service()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
