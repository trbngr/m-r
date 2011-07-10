using System;
using System.Runtime.Serialization;

using Lokad.Cqrs;

using Newtonsoft.Json;

using ProtoBuf;

namespace SimpleCQRS.Messages
{
// ReSharper disable UnusedMember.Local
		
	[DataContract]
	[ProtoContract]
	public sealed class RemoveItemsFromInventory : Define.Command
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid InventoryItemId { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public int Count { get; private set; }
		[DataMember(Order=3), ProtoMember(3)]
		public int OriginalVersion { get; private set; }
		[JsonConstructor]
		private RemoveItemsFromInventory () {}
		public RemoveItemsFromInventory (Guid inventoryItemId, int count, int originalVersion)
		{
			InventoryItemId = inventoryItemId;
			Count = count;
			OriginalVersion = originalVersion;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class CheckInItemsToInventory : Define.Command
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid InventoryItemId { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public int Count { get; private set; }
		[DataMember(Order=3), ProtoMember(3)]
		public int OriginalVersion { get; private set; }
		[JsonConstructor]
		private CheckInItemsToInventory () {}
		public CheckInItemsToInventory (Guid inventoryItemId, int count, int originalVersion)
		{
			InventoryItemId = inventoryItemId;
			Count = count;
			OriginalVersion = originalVersion;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class CreateInventoryItem : Define.Command
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid InventoryItemId { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public string Name { get; private set; }
		[JsonConstructor]
		private CreateInventoryItem () {}
		public CreateInventoryItem (Guid inventoryItemId, string name)
		{
			InventoryItemId = inventoryItemId;
			Name = name;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class RenameInventoryItem : Define.Command
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid InventoryItemId { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public string NewName { get; private set; }
		[DataMember(Order=3), ProtoMember(3)]
		public int OriginalVersion { get; private set; }
		[JsonConstructor]
		private RenameInventoryItem () {}
		public RenameInventoryItem (Guid inventoryItemId, string newName, int originalVersion)
		{
			InventoryItemId = inventoryItemId;
			NewName = newName;
			OriginalVersion = originalVersion;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class DeactivateInventoryItem : Define.Command
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid InventoryItemId { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public int OriginalVersion { get; private set; }
		[JsonConstructor]
		private DeactivateInventoryItem () {}
		public DeactivateInventoryItem (Guid inventoryItemId, int originalVersion)
		{
			InventoryItemId = inventoryItemId;
			OriginalVersion = originalVersion;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class ItemsRemovedFromInventory : DomainEvent
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid Id { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public int Count { get; private set; }
		[JsonConstructor]
		private ItemsRemovedFromInventory () {}
		public ItemsRemovedFromInventory (Guid id, int count)
		{
			Id = id;
			Count = count;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class InventoryItemRenamed : DomainEvent
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid Id { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public string NewName { get; private set; }
		[JsonConstructor]
		private InventoryItemRenamed () {}
		public InventoryItemRenamed (Guid id, string newName)
		{
			Id = id;
			NewName = newName;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class InventoryItemDeactivated : DomainEvent
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid Id { get; private set; }
		[JsonConstructor]
		private InventoryItemDeactivated () {}
		public InventoryItemDeactivated (Guid id)
		{
			Id = id;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class ItemsCheckedInToInventory : DomainEvent
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid Id { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public int Count { get; private set; }
		[JsonConstructor]
		private ItemsCheckedInToInventory () {}
		public ItemsCheckedInToInventory (Guid id, int count)
		{
			Id = id;
			Count = count;
		}
	}
	
	[DataContract]
	[ProtoContract]
	public sealed class InventoryItemCreated : DomainEvent
	{
		[DataMember(Order=1), ProtoMember(1)]
		public Guid Id { get; private set; }
		[DataMember(Order=2), ProtoMember(2)]
		public string Name { get; private set; }
		[JsonConstructor]
		private InventoryItemCreated () {}
		public InventoryItemCreated (Guid id, string name)
		{
			Id = id;
			Name = name;
		}
	}

// ReSharper restore UnusedMember.Local
}