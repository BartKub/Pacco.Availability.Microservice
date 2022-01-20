using System.Collections.Generic;
using Convey.CQRS.Events;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Application.Services
{
    public interface IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
        public IEvent Map(IDomainEvent @event);
    }
}