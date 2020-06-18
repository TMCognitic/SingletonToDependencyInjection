using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Repositories
{
    public interface IService : IDisposable
    {
        void DoSomething();
    }
}
