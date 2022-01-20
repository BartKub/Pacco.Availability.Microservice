using System;
using System.Collections.Generic;
using System.Text;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events.External;
using Pacco.Services.Availability.Application.Events.Rejected;
using Pacco.Services.Availability.Application.Exception;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Infrastructure.Exception
{
    internal sealed class ExceptionToMessageMapper: IExceptionToMessageMapper
    {
        public object Map(System.Exception exception, object message)
            => exception switch
            {
                MissingResourceTagsException ex => new AddResourceRejected(Guid.Empty, ex.Message, ex.Code),
                InvalidResourceTagsException ex => new AddResourceRejected(Guid.Empty, ex.Message, ex.Code),
                CannotExpropriateReservationException ex => new ResourceReservedRejected(ex.ResourceId, ex.Message, ex.Code),
                ResourceAlreadyExistsException ex => new AddResourceRejected(ex.ResourceId, ex.Message, ex.Code),
                ResourceNotFoundException ex => message switch
                {
                    ReserveResource cmd => new ResourceReservedRejected(ex.ResourceId, ex.Message, ex.Code),
                    _ => null
                },
                _ => null
            };

    }
}
