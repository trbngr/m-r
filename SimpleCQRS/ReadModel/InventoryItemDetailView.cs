using Lokad.Cqrs;
using Lokad.Cqrs.Feature.AtomicStorage;

using SimpleCQRS.Messages;

namespace SimpleCQRS.ReadModel
{
    public class InventoryItemDetailView
        : Define.Subscribe<InventoryItemCreated>,
          Define.Subscribe<InventoryItemDeactivated>,
          Define.Subscribe<InventoryItemRenamed>,
          Define.Subscribe<ItemsRemovedFromInventory>,
          Define.Subscribe<ItemsCheckedInToInventory>
    {
        private readonly NuclearStorage storage;

        public InventoryItemDetailView(NuclearStorage storage)
        {
            this.storage = storage;
        }

        public void Consume(InventoryItemCreated message)
        {
            storage.AddEntity(message.Id, new InventoryItemDetailsDto(message.Id, message.Name, 0, message.Version));
        }

        public void Consume(InventoryItemDeactivated message)
        {
            storage.TryDeleteEntity<InventoryItemDetailsDto>(message.Id);
        }

        public void Consume(InventoryItemRenamed message)
        {
            storage.UpdateEntity<InventoryItemDetailsDto>(message.Id, d =>
            {
                d.Name = message.NewName;
                d.Version = message.Version;
            });
        }

        public void Consume(ItemsCheckedInToInventory message)
        {
            storage.UpdateEntity<InventoryItemDetailsDto>(message.Id, d =>
            {
                d.CurrentCount += message.Count;
                d.Version = message.Version;
            });
        }

        public void Consume(ItemsRemovedFromInventory message)
        {
            storage.UpdateEntity<InventoryItemDetailsDto>(message.Id, d =>
            {
                d.CurrentCount -= message.Count;
                d.Version = message.Version;
            });
        }
    }
}