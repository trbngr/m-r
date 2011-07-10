using System;

namespace SimpleCQRS.Domain
{
    public interface IRepository<out T> where T : AggregateRoot, new()
    {
        void Save(AggregateRoot aggregate, int expectedVersion);
        T GetById(Guid id);
    }
}