using System.ServiceModel;

namespace EventsLib
{
    interface IVelibServiceEvents
    {
        [OperationContract(IsOneWay = true)]
        void VelibAvailable(string station, string velibDispos);
    }
}
