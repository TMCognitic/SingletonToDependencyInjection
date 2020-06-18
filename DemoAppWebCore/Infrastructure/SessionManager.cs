using DemoAppWebCore.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAppWebCore.Infrastructure
{
    public class SessionManager : ISessionManager
    {
        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public User User
        {
            get { return (_session.Keys.Contains(nameof(User)) ? JsonConvert.DeserializeObject<User>(_session.GetString(nameof(User))) : null); }
            set { _session.SetString(nameof(User), JsonConvert.SerializeObject(value)); }
        }

        public void Abandon()
        {
            _session.Clear();
        }
    }
}
