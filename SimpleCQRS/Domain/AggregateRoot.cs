using System;
using System.Collections.Generic;

using SimpleCQRS.Messages;

namespace SimpleCQRS.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<DomainEvent> changes = new List<DomainEvent>();
       
        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        public IEnumerable<DomainEvent> GetUncommittedChanges()
        {
            return changes;
        }

        public void MarkChangesAsCommitted()
        {
            changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<DomainEvent> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(DomainEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(DomainEvent @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if(isNew)
            {
                changes.Add(@event);
            }
        }
    }
}