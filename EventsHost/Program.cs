using EventsLib;
using System;
using System.ServiceModel;


namespace ClientToLaunchHost
{
    class Program{
        static void Main(string[] args){

            ServiceHost host  = new ServiceHost(typeof(VelibService));
            host.Open();
            Console.WriteLine("Host is running... Press <Enter> key to stop");
            Console.ReadLine();
            host.Close();
        }
    }
}
