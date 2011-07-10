using System;

using Lokad.Cqrs.Feature.AtomicStorage;

namespace SimpleCQRS.ReadModel
{
    public class ReadModelFacade : IReadModelFacade
    {
        private readonly IAtomicEntityReader<Guid, InventoryItemDetailsDto> entityReader;
        private readonly IAtomicSingletonReader<InventoryItems> singletonReader;

        public ReadModelFacade(IAtomicEntityReader<Guid, InventoryItemDetailsDto> entityReader, IAtomicSingletonReader<InventoryItems> singletonReader)
        {
            this.entityReader = entityReader;
            this.singletonReader = singletonReader;
        }

        public InventoryItems GetInventoryItems()
        {
            return singletonReader.GetOrNew();
        }

        public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
        {
            InventoryItemDetailsDto view;
            entityReader.TryGet(id, out view);
            return view;
        }
    }
}