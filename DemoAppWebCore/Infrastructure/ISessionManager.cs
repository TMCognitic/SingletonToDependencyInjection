using DemoAppWebCore.Models;

namespace DemoAppWebCore.Infrastructure
{
    public interface ISessionManager
    {
        User User { get; set; }
        void Abandon();
    }
}