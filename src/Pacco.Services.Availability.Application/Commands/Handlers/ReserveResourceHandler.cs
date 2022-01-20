using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exception;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Application.Services.Clients;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    internal sealed class ReserveResourceHandler: ICommandHandler<ReserveResource>
    {
        private readonly IResourceRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly ICustomerServiceClient _customerServiceClient;

        public ReserveResourceHandler(IResourceRepository repository, 
            IEventProcessor eventProcessor,
            ICustomerServiceClient customerServiceClient)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _customerServiceClient = customerServiceClient;
        }

        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await _repository.GetAsync(command.ResourceId);
            if (resource == null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }

            var customerState = await _customerServiceClient.GetStateAsync(command.CustomerId);

            if (customerState is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }

            if (customerState.IsValid)
            {
                throw new InvalidCustomerStateException(command.CustomerId, customerState.State);
            }

            var reservation = new Reservation(command.DateTime, command.Priority);
            resource.AddReservation(reservation);

            await _repository.UpdateAsync(resource);
            await _eventProcessor.ProcessAsync(resource.Events);
        }
    }
}
