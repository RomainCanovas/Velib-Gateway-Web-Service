using System;
using System.ServiceModel;

namespace ConsoleClient
{
    class Program{
        static void Main(string[] args){
            VelibServiceCallbackSink objsink = new VelibServiceCallbackSink();
            InstanceContext iCntxt = new InstanceContext(objsink);
            VelibServiceReference.VelibServiceClient objClient = new VelibServiceReference.VelibServiceClient(iCntxt);
            Console.WriteLine("Les informations se mettent à jour toutes les 5 secondes");
            Console.WriteLine("Dans quelle ville se trouve la station que vous recherchez?");
            string city = Console.ReadLine();
            Console.WriteLine("Tapez maintenant le nom partiel ou complet de votre station. Dans le cas où plusieurs \nstations comportent votre recherche, la première occurence sera affichée.");
            string station = Console.ReadLine();
            objClient.SubscribeVelibAvailable(city, station);
            Console.ReadLine();
        }
    }
}