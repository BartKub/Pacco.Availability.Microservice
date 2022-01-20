using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events.External;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Tests.Shared.Factories;
using Pacco.Services.Availability.Tests.Shared.Fixtures;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Integration.Async
{
    public class AddResourceTest : IDisposable, IClassFixture<PaccoApplicationFactory<Program>>
    {
        private Task Act(AddResource command) => _rabbitMqFixture.PublishAsync(command, Exchange);


        #region Arrange

        private const string Exchange = "availability";
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;

        public AddResourceTest(PaccoApplicationFactory<Program> factory)
        {
            _rabbitMqFixture = new RabbitMqFixture(Exchange);
            _mongoDbFixture = new MongoDbFixture<ResourceDocument, Guid>("resources");
            factory.Server.AllowSynchronousIO = true;
        }

        #endregion

        [Fact]
        public async Task add_resource_command_should_add_document_with_given_id_to_database()
        {
            var command = new AddResource(Guid.NewGuid(), new[] {"tag"});

            var tcs = _rabbitMqFixture.SubscribeAndGet<ResourceAdded, ResourceDocument>(Exchange,
                _mongoDbFixture.GetAsync, command.ResourceId);

            await Act(command);

            var doc = await tcs.Task;

            doc.ShouldNotBeNull();
            doc.Id.ShouldBe(command.ResourceId);
            doc.Tags.ShouldBe(command.Tags);

        }

        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }
    }
   
}
