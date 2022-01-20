using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External
{
    [Contract]
    public class ResourceAdded: IEvent
    {
        public ResourceAdded(Guid resourceId)
        {
            ResourceId = resourceId;
        }

        public Guid ResourceId { get; }
    }
}
