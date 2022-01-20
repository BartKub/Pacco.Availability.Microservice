using System;

namespace Pacco.Services.Availability.Core.Exceptions
{
    public class CannotExpropriateReservationException : DomainException
    {
        public override string Code => "cannot_expropriate_reservation";
        public Guid ResourceId { get; }
        public DateTime DateTime { get; }
        

        public CannotExpropriateReservationException(Guid resourceId, DateTime dateTime) :
            base($"resource with id {resourceId} cannot be expropriated. Reservation at {dateTime} ")
        {
            ResourceId = resourceId;
            DateTime = dateTime;
        }
    }
}