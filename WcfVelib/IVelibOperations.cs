using System;
using System.ServiceModel;

namespace WcfVelib
{
    [ServiceContract]
    public interface IVelibOperations
    {
        [OperationContract]
        string[] GetCities();

        [OperationContract]
        string[] GetStations(string city);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetCities(AsyncCallback callback, object state);
        string[] EndGetCities(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetStations(string city, AsyncCallback callback, object state);
        string[] EndGetStations(IAsyncResult asyncResult);
    }
}
