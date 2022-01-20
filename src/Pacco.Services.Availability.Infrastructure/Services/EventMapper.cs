using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Convey.CQRS.Events;
using Convey.WebApi;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Events.External;
using Pacco.Services.Availability.Application.Events.Rejected;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    public class EventMapper: IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

        public IEvent Map(IDomainEvent @event) =>
            @event switch
            {
                ResourceCreated e => new ResourceAdded(e.Resource.Id),
                ReservationCancelled e => new ResourceReservationCancelled(e.Resource.Id, e.Reservation.Date),
                ReservationAdded e => new ResourceReserved(e.Resource.Id, e.Reservation.Date),
                _ => null
            };
    }
}
