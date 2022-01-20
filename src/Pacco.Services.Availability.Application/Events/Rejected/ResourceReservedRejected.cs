using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.Rejected
{
    [Contract]
    public class ResourceReservedRejected : IRejectedEvent
    {
        public Guid ResourceID { get; }
        public string Reason { get; }
        public string Code { get; }

        public ResourceReservedRejected(Guid resourceId, string reason, string code)
        {
            ResourceID = resourceId;
            Reason = reason;
            Code = code;
        }
    }
}