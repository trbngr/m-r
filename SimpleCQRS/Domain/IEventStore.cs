using System;
using System.Collections.Generic;

using SimpleCQRS.Messages;

namespace SimpleCQRS.Domain
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion);
        List<DomainEvent> GetEventsForAggregate(Guid aggregateId);
    }
}