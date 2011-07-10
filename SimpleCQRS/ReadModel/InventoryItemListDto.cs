using System;
using System.Runtime.Serialization;

namespace SimpleCQRS.ReadModel
{
    [DataContract]
    public class InventoryItemListDto : IEntity
    {
        [DataMember]
        public Guid Id;

        [DataMember]
        public string Name;

        public InventoryItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}