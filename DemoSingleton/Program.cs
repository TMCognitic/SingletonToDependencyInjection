using Model.Repositories;
using Model.Services;
using System;

namespace DemoSingleton
{
    class Program
    {
        static void Main(string[] args)
        {
            IService service = Locator.Instance.Resources[typeof(IService)];

            service.DoSomething();

            //using (IService service = Locator.Instance.Service)
            //{
            //    service.DoSomething();
            //}

            //Controller1 controller1 = new Controller1();
            //Controller2 controller2 = new Controller2();

            //controller1.Index();
            //controller2.Index();
        }
    }

    class Controller1
    {
        private IService _service;
        private ILog _logger;

        public Controller1()
        {
            _logger = DILocator.Instance.Logger;
            _service = DILocator.Instance.Service;
        }

        public void Index()
        {
            _logger.Write($"Call Index from {GetType().Name}");
            _service.DoSomething();
        }
    }

    class Controller2
    {        
        private IService _service;
        private ILog _logger;

        public Controller2()
        {
            _logger = DILocator.Instance.Logger;
            _service = DILocator.Instance.Service;
        }

        public void Index()
        {
            _logger.Write($"Call Index from {GetType().Name}");
            _service.DoSomething();
        }
    }
}
