using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DemoAppWebCore.Models;
using Models.Services;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using DemoAppWebCore.Infrastructure;

namespace DemoAppWebCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDIService _service;
        private readonly ISessionManager _sessionManager;

        public HomeController(ILogger<HomeController> logger, IDIService service, ISessionManager sessionManager)
        {            
            _logger = logger;
            _service = service;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            _sessionManager.User = new User() { Id = 1, Email = "test@test.be" };
            _service.DoSomething();
            _logger.LogWarning("Accès à Index");
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Email = _sessionManager.User.Email;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
