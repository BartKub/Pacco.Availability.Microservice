using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External
{
    [Contract]
    public class ResourceReserved: IEvent   
    {
        public Guid ResourceId { get;  }
        public DateTime DateTime { get;  }

        public ResourceReserved(Guid resourceId, DateTime dateTime)
        {
            ResourceId = resourceId;
            DateTime = dateTime;
        }
    }
}
