using Model.Repositories;
using System;

namespace Model.Services
{
    internal sealed class Log : ILog
    {
        public void Write(string text)
        {            
            Console.WriteLine($"Log - {GetHashCode()}");
            Console.WriteLine(text);
        }
    }
}
