using System;

namespace ConsoleApp
{
    class Program
    {
        static WcfVelib.VelibOperationsClient client;

        static void Main(string[] args)
        {
            client = new WcfVelib.VelibOperationsClient();

            while (true)
            {
                Boolean cityError = false;
                Console.WriteLine("Entrez le numéro correspondant à la ville qui vous intéresse :");
                Console.WriteLine("Si rien ne s'affiche, appuyez sur la flèche du bas.");
                string[] cities = client.GetCities();
                int i = 1;
                foreach (string item in cities)
                {
                    Console.WriteLine(i++ + " - " + item);
                }

                int choice = 0;
                if (!Int32.TryParse(Console.ReadLine(), out choice) || choice > 26)
                {
                    Console.WriteLine("Numéro non reconnu, vous allez être redirigé");
                    System.Threading.Thread.Sleep(2000);
                    cityError = true;
                }

                if (cityError == false)
                {
                    string city = cities[choice - 1];
                    Console.WriteLine("Vous avez choisi : " + city);

                    string[] stations = client.GetStations(city);

                    Console.WriteLine("Entrez un nom de station, ou pour avoir la liste des stations entrez \"?\"");
                    DisplayStations(stations);
                }

            }

        }

        public static void DisplayStations(string[] stations)
        {
            while (true)
            {
                bool contains = false;
                string recherche = Console.ReadLine().ToUpper();
                foreach (string item in stations)
                {
                    string[] splitted = item.Split('#');
                    string name = splitted[0];
                    string status = splitted[1];
                    string available_bikes = splitted[2];
                    string available_bike_stands = splitted[3];

                    if (recherche.Equals("MENU"))
                    {
                        return;
                    }
                    
                    else if (name.Contains(recherche))
                    {
                        contains = true;
                        Console.WriteLine("Station choisie : " + name);
                        Console.WriteLine("Statut de la station : " + status);
                        Console.WriteLine("Nombre de vélos restants : " + available_bikes);
                        Console.WriteLine("Nombre de stands disponibles : " + available_bike_stands);
                        Console.WriteLine("Vous pouvez entrer un autre nom de station, taper \"?\" pour avoir la liste des stations ou taper \"menu\" pour revenir au menu principal");
                        break;
                    }
                }
                if (contains == false)
                {
                    Console.WriteLine("Aucune station ne correspond à votre recherche, veuillez réessayer parmi les stations suivantes");
                    foreach (string item in stations)
                    {
                        Console.WriteLine(item.Split('#')[0]);
                    }
                }
                contains = false;
            }
        }
    }
}