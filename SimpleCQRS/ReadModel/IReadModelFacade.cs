using System;

namespace SimpleCQRS.ReadModel
{
    public interface IReadModelFacade
    {
        InventoryItems GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }
}