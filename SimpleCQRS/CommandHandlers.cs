using Lokad.Cqrs;

using SimpleCQRS.Domain;
using SimpleCQRS.Messages;

namespace SimpleCQRS
{
    public class InventoryCommandHandlers
        : Define.Handle<CreateInventoryItem>,
          Define.Handle<DeactivateInventoryItem>,
          Define.Handle<RemoveItemsFromInventory>,
          Define.Handle<CheckInItemsToInventory>,
          Define.Handle<RenameInventoryItem>
    {
        private readonly IRepository<InventoryItem> repository;

        public InventoryCommandHandlers(IRepository<InventoryItem> repository)
        {
            this.repository = repository;
        }

        #region Handles<CheckInItemsToInventory> Members

        public void Consume(CheckInItemsToInventory message)
        {
            var item = repository.GetById(message.InventoryItemId);
            item.CheckIn(message.Count);
            repository.Save(item, message.OriginalVersion);
        }

        #endregion

        #region Handles<CreateInventoryItem> Members

        public void Consume(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.InventoryItemId, message.Name);
            repository.Save(item, 0);
        }

        #endregion

        #region Handles<DeactivateInventoryItem> Members

        public void Consume(DeactivateInventoryItem message)
        {
            var item = repository.GetById(message.InventoryItemId);
            item.Deactivate();
            repository.Save(item, message.OriginalVersion);
        }

        #endregion

        #region Handles<RemoveItemsFromInventory> Members

        public void Consume(RemoveItemsFromInventory message)
        {
            var item = repository.GetById(message.InventoryItemId);
            item.Remove(message.Count);
            repository.Save(item, message.OriginalVersion);
        }

        #endregion

        #region Handles<RenameInventoryItem> Members

        public void Consume(RenameInventoryItem message)
        {
            var item = repository.GetById(message.InventoryItemId);
            item.ChangeName(message.NewName);
            repository.Save(item, message.OriginalVersion);
        }

        #endregion
    }
}