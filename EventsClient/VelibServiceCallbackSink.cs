using System;

namespace ConsoleClient
{
    internal class VelibServiceCallbackSink : VelibServiceReference.IVelibServiceCallback{
        public void VelibAvailable(string station, string velibDispos)
        {
            var date = DateTime.Now;
            Console.WriteLine("\n" + date.Hour + "h" + date.Minute + ":" + date.Second +"\nIl y a " + velibDispos + " vélos disponibles à la station " + station);
        }
    }
}