using System;
using System.Runtime.Serialization;

namespace SimpleCQRS.ReadModel
{
    [DataContract]
    public class InventoryItemDetailsDto : IEntity
    {
        [DataMember]
        public Guid Id;
        [DataMember]
        public string Name;
        [DataMember]
        public int CurrentCount;
        [DataMember]
        public int Version;

        public InventoryItemDetailsDto(Guid id, string name, int currentCount, int version)
        {
			Id = id;
			Name = name;
            CurrentCount = currentCount;
            Version = version;
        }
    }
}
