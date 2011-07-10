using Lokad.Cqrs;
using Lokad.Cqrs.Feature.AtomicStorage;

using SimpleCQRS.Messages;

namespace SimpleCQRS.ReadModel
{
    public class InventoryListView
        : Define.Subscribe<InventoryItemCreated>,
          Define.Subscribe<InventoryItemRenamed>,
          Define.Subscribe<InventoryItemDeactivated>
    {
        private readonly NuclearStorage storage;

        public InventoryListView(NuclearStorage storage)
        {
            this.storage = storage;
        }

        public void Consume(InventoryItemCreated message)
        {
            var entity = new InventoryItemListDto(message.Id, message.Name);
            storage.AddEntity(message.Id, entity);
            storage.UpdateSingletonEnforcingNew<InventoryItems>(i => i.Items.Add(message.Id, message.Name));
        }

        public void Consume(InventoryItemRenamed message)
        {
            storage.UpdateEntity<InventoryItemListDto>(message.Id, e => e.Name = message.NewName);
            storage.UpdateSingleton<InventoryItems>(i => i.Items[message.Id] = message.NewName);
        }

        public void Consume(InventoryItemDeactivated message)
        {
            storage.TryDeleteEntity<InventoryItemListDto>(message.Id);
            storage.UpdateSingleton<InventoryItems>(i => i.Items.Remove(message.Id));
        }
    }
}