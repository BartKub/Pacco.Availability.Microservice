using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static Resource AsEntity(this ResourceDocument document) =>
            new Resource(document.Id, document.Tags,
                document.Reservations.Select(r => new Reservation(r.TimeStamp.AsDateTime(), r.Priority)), 
                document.Version);

        public static ResourceDocument AsDocument(this Resource resource)
        {
            return new ResourceDocument
            {
                Id = resource.Id,
                Version = resource.Version,
                Tags = resource.Tags,
                Reservations = resource.Reservations.Select(r => new ReservationDocument
                {
                    TimeStamp = r.Date.AsDaysSinceEpoch(),
                    Priority = r.Priority
                })
            };
        }

        public static ResourceDto AsDto(this ResourceDocument document)
        {
            return new ResourceDto
            {
                Id = document.Id,
                Tags = document.Tags ?? Enumerable.Empty<string>(),
                Reservations = document?.Reservations?.Select(r => new ReservationDto
                {
                    DateTime = r.TimeStamp.AsDateTime(),
                    Priority = r.Priority
                })
            };
        }

        internal static int AsDaysSinceEpoch(this DateTime dateTime) => (dateTime - new DateTime()).Days;
        internal static DateTime AsDateTime(this int daySinceEpoch) => new DateTime().AddDays(daySinceEpoch);
    }
}
