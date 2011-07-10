#region Copyright (c) 2011, EventDay Inc.

// Copyright (c) 2011, EventDay Inc.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the EventDay Inc. nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL EventDay Inc. BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// In laymen terms, if this software murders your family and burns your house to the ground,
// don't come to us. ;)

#endregion

using System;
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
        private readonly IDataSerializer serializer;
        private readonly ITapeStorageFactory tapeFactory;

        public EventStore(IMessageSender sender, ITapeStorageFactory tapeFactory, IDataSerializer serializer)
        {
            this.sender = sender;
            this.tapeFactory = tapeFactory;
            this.serializer = serializer;
        }

        #region IEventStore Members

        public void SaveEvents(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion)
        {
            var stream = tapeFactory.GetOrCreateStream(aggregateId.ToString());
            var record = stream.ReadRecords(0, int.MaxValue).LastOrDefault();

            if (record != null)
            {
                if (record.Version != expectedVersion && expectedVersion != 0)
                {
                    throw new ConcurrencyException();
                }
            }

            SaveEventsInternal(events, stream, expectedVersion);
        }

        public List<DomainEvent> GetEventsForAggregate(Guid aggregateId)
        {
            var stream = tapeFactory.GetOrCreateStream(aggregateId.ToString());
            var records = stream.ReadRecords(0, int.MaxValue);
            var descriptors = records.Select(r =>
            {
                using (var buffer = new MemoryStream(r.Data))
                {
                    return (EventDescriptor) serializer.Deserialize(buffer, typeof (EventDescriptor));
                }
            });

            return descriptors.Select(e => e.EventData).ToList();
        }

        #endregion

        private void SaveEventsInternal(IEnumerable<DomainEvent> events, ITapeStream stream, int version)
        {
            var array = events.ToArray();

            foreach (var @event in array)
            {
                @event.Version = ++version;

                byte[] bytes;
                using (var buffer = new MemoryStream())
                {
                    var descriptor = new EventDescriptor(@event);
                    serializer.Serialize(descriptor, buffer);
                    bytes = buffer.ToArray();
                }
                stream.TryAppend(bytes, TapeAppendCondition.None);
            }

            sender.SendBatch(array);
        }

        #region Nested type: EventDescriptor

        [DataContract]
        public struct EventDescriptor
        {
            [DataMember] public readonly DomainEvent EventData;

            public EventDescriptor(DomainEvent eventData)
            {
                EventData = eventData;
            }
        }

        #endregion
    }
}