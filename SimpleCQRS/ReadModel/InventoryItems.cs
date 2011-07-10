using System;
using System.Collections.Generic;

namespace SimpleCQRS.ReadModel
{
    public class InventoryItems : ISingleton
    {
        public InventoryItems()
        {
            Items = new Dictionary<Guid, string>();
        }

        public IDictionary<Guid, string> Items { get; set; }
    }
}