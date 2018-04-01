using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace WcfVelib
{
    public class VelibOperations : IVelibOperations
    {

        private static readonly string APIKey = "33b2f23eda16b6f3b43d7f3524bfa1f5fe02ca99";
        private Dictionary<string, string[]> cache = new Dictionary<string, string[]>();
        private string[] cityCache;
        Boolean firstConnection = true;
        public const int DelayMilliseconds = 10000;


        public string[] GetCities()
        {
            if (firstConnection == false)
            {
                return cityCache;
            }
            else
            {
                WebRequest requestCity = WebRequest.Create(
                    "https://api.jcdecaux.com/vls/v1/contracts?apiKey=" + APIKey);
                string responseFromServerCity = GetResponse(requestCity);
                int i = 0;
                JArray docCity = JArray.Parse(responseFromServerCity);
                string[] cities = new string[27];
                foreach (JObject item in docCity)
                {
                    cities[i++] = (string)item.SelectToken("name");
                }
                cityCache = cities;
                firstConnection = false;
                return cities;
            }
        }

        public string[] GetStations(string city)
        {
            if (cache.ContainsKey(city))
            {
                return cache[city];
            }
            else
            {
                WebRequest request = WebRequest.Create(
                    "https://api.jcdecaux.com/vls/v1/stations?contract=" + city + "&apiKey=" + APIKey);
                string responseFromServer = GetResponse(request);
                JArray docStations = JArray.Parse(responseFromServer);
                int i = 0;
                string[] stations = new string[docStations.Count];
                foreach (JObject item in docStations)
                {
                    stations[i++] = ((string)item.SelectToken("name") + "#" +
                        ((string)item.SelectToken("status") == "OPEN" ? "Ouverte" : "Fermée") + "#" +
                        (string)item.SelectToken("available_bikes") + "#" +
                        (string)item.SelectToken("available_bike_stands") + "/" + (string)item.SelectToken("bike_stands"));
                }
                cache.Add(city, stations);

                return stations;
            }
        }

        private string GetResponse(WebRequest request)
        {
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }

        public IAsyncResult BeginGetCities(AsyncCallback callback, object state)
        {
            var asyncResult = new SimpleAsyncResult<string[]>(state);

            var timer = new System.Timers.Timer(DelayMilliseconds);
            timer.Elapsed += (_, args) =>
            {
                asyncResult.Result = GetCities();
                asyncResult.IsCompleted = true;
                callback(asyncResult);
                timer.Enabled = false;
                timer.Close();
            };
            timer.Enabled = true;
            return asyncResult;
        }

        public string[] EndGetCities(IAsyncResult asyncResult)
        {
            return ((SimpleAsyncResult<string[]>)asyncResult).Result;
        }

        public IAsyncResult BeginGetStations(string city, AsyncCallback callback, object state)
        {
            var asyncResult = new SimpleAsyncResult<string[]>(state);

            var timer = new System.Timers.Timer(DelayMilliseconds);
            timer.Elapsed += (_, args) =>
            {
                asyncResult.Result = GetStations(city);
                asyncResult.IsCompleted = true;
                callback(asyncResult);
                timer.Enabled = false;
                timer.Close();
            };
            timer.Enabled = true;
            return asyncResult;
        }

        public string[] EndGetStations(IAsyncResult asyncResult)
        {
            return ((SimpleAsyncResult<string[]>)asyncResult).Result;
        }


        public class SimpleAsyncResult<T> : IAsyncResult
        {
            private readonly object accessLock = new object();
            private bool isCompleted = false;
            private T result;

            public SimpleAsyncResult(object asyncState)
            {
                AsyncState = asyncState;
            }

            public T Result
            {
                get
                {
                    lock (accessLock)
                    {
                        return result;
                    }
                }
                set
                {
                    lock (accessLock)
                    {
                        result = value;
                    }
                }
            }

            public bool IsCompleted
            {
                get
                {
                    lock (accessLock)
                    {
                        return isCompleted;
                    }
                }
                set
                {
                    lock (accessLock)
                    {
                        isCompleted = value;
                    }
                }
            }

            public WaitHandle AsyncWaitHandle { get { return null; } }

            public bool CompletedSynchronously { get { return false; } }

            public object AsyncState { get; private set; }

            WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();
        }

    }
}
