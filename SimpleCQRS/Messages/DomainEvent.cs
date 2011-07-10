using System.Runtime.Serialization;

using Lokad.Cqrs;

namespace SimpleCQRS.Messages
{
    [DataContract]
    public abstract class DomainEvent : Define.Event
    {
        [DataMember]
        public int Version { get; set; }
    }
}