﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

using Lokad.Cqrs;
using Lokad.Cqrs.Feature.TapeStorage;

using SimpleCQRS.Messages;

namespace SimpleCQRS.Domain
{
    public class EventStore : IEventStore
    {
        private readonly IMessageSender sender;
        private readonly ITapeStorageFactory tapeFactory;
        private readonly IDataSerializer serializer;

        public EventStore(IMessageSender sender, ITapeStorageFactory tapeFactory, IDataSerializer serializer)
        {
            this.sender = sender;
            this.tapeFactory = tapeFactory;
            this.serializer = serializer;
        }

        [DataContract]
        public struct EventDescriptor
        {
            [DataMember]
            public readonly DomainEvent EventData;

            public EventDescriptor(DomainEvent eventData)
            {
                EventData = eventData;
            }
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion)
        {
            var stream = tapeFactory.GetOrCreateStream(aggregateId.ToString());
            var records = stream.ReadRecords(0, int.MaxValue).ToArray();
            if (records.Length > 0)
            {
                if (records[records.Length-1].Version != expectedVersion && expectedVersion != 0)
                {
                    throw new ConcurrencyException();
                }
            }
            SaveEventsInternal(events, stream, expectedVersion);
        }

        private void SaveEventsInternal(IEnumerable<DomainEvent> events, ITapeStream stream, int version)
        {
            var array = events.ToArray();

            foreach (var @event in array)
            {
                @event.Version = ++version;

                byte[] bytes;
                using(var buffer = new MemoryStream())
                {
                    var descriptor = new EventDescriptor(@event);
                    serializer.Serialize(descriptor, buffer);
                    bytes = buffer.ToArray();
                }
                stream.TryAppend(bytes, TapeAppendCondition.None);
            }

            sender.SendBatch(array);
        }

        public List<DomainEvent> GetEventsForAggregate(Guid aggregateId)
        {
            var stream = tapeFactory.GetOrCreateStream(aggregateId.ToString());
            var records = stream.ReadRecords(0, int.MaxValue);
            var descriptors = records.Select(r =>
            {
                using (var buffer = new MemoryStream(r.Data))
                {
                    return (EventDescriptor)serializer.Deserialize(buffer, typeof(EventDescriptor));
                }
            });

            return descriptors.Select(e => e.EventData).ToList();
        }
    }
}