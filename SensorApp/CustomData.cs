using System;
using System.Runtime.Serialization;

namespace SensorApp
{
    [DataContract]
    public class CustomData
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}