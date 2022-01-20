using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Commands.Handlers;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Exception;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Application.Services.Clients;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Application.Handlers
{
    public class ReserveResourceHandlerTest
    {
        private Task Act(ReserveResource command) => _handler.HandleAsync(command);


        #region Arrange

        private readonly ReserveResourceHandler _handler;
        private readonly IResourceRepository _resourceRepository;
        private readonly ICustomerServiceClient _serviceClient;
        private readonly IEventProcessor _eventProcessor;

        public ReserveResourceHandlerTest()
        {
            _resourceRepository = Substitute.For<IResourceRepository>();
            _serviceClient = Substitute.For<ICustomerServiceClient>();
            _eventProcessor = Substitute.For<IEventProcessor>();

            _handler = new ReserveResourceHandler(_resourceRepository, _eventProcessor, _serviceClient);
        }

        #endregion

        [Fact]
        public async Task given_invalid_resurceId_reserve_resource_should_throw_an_exception()
        {
            var command = new ReserveResource(Guid.NewGuid(), DateTime.UtcNow, 0, Guid.NewGuid());

            var exception = await Record.ExceptionAsync(async () => await Act(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceNotFoundException>();
        }

        [Fact]
        public async Task given_valid_resourceId_for_valid_customer_reserve_resource_should_succeed()
        {
            var command = new ReserveResource(Guid.NewGuid(), DateTime.UtcNow, 0, Guid.NewGuid());
            var resource = Resource.Create(new AggregateId(), new[] {"tag"});

            _resourceRepository.GetAsync(command.ResourceId).Returns(resource);

            var customerState = new CustomerStateDto
            {
                State = "valid"
            };

            _serviceClient.GetStateAsync(command.CustomerId).Returns(customerState);

            await Act(command);

            await _resourceRepository.Received(1).UpdateAsync(resource);
            await _eventProcessor.Received().ProcessAsync(resource.Events);
        }
    }
}
