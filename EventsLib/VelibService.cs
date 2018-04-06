using System;
using System.Text;
using System.ServiceModel;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Threading;

namespace EventsLib
{
    public class VelibService : IVelibService
    {
        static Action<string, string> velibAvailableEvent = delegate { };

        public void SubscribeVelibAvailable(string city, string station)
        {
            IVelibServiceEvents subscriber =
            OperationContext.Current.GetCallbackChannel<IVelibServiceEvents>();
            velibAvailableEvent += subscriber.VelibAvailable;
            string[] param = new string[] { city, station};
            Thread updater = new Thread(new ParameterizedThreadStart(Update));
            updater.Start(param);
        }

        public void Update(object param)
        {
            string[] mParam = (string[])param;
            string city = mParam[0];
            string station = mParam[1];

            while (true)
            {
                WebRequest request = WebRequest.Create("https://api.jcdecaux.com/vls/v1/stations?contract=" + city + "&apiKey=33b2f23eda16b6f3b43d7f3524bfa1f5fe02ca99");
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JArray doc = JArray.Parse(responseFromServer);

                foreach (JObject item in doc)
                {
                    string name = (string)item.SelectToken("name");
                    if (name.ToLower().Contains(station.ToLower()))
                    {
                        string velibDispos = (string)item.SelectToken("available_bikes");
                        velibAvailableEvent(name, velibDispos);
                        break;
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}