using System;
using TokenSimulations.Simulations;

namespace TokenSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            ParadoxicalCovenant.SimplePull(100000);

            ParadoxicalCovenant.JackiePull(100000);

            ParadoxicalCovenant.JackiePullAfter(100000);

            ParadoxicalCovenant.OlivePull(100000);

            Console.ReadLine();
        }
    }
}
