using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    internal class EventProcessor: IEventProcessor
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<IEventProcessor> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IMessageBroker messageBroker, 
            IEventMapper eventMapper, 
            ILogger<IEventProcessor> logger, 
            IServiceScopeFactory scopeFactory)
        {
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }


        public async Task ProcessAsync(IEnumerable<IDomainEvent> events)
        {
            if (events is null)
            {
                return;
            }

            var integrationEvents = await HandleDomainEventsAsync(events);

            if (!integrationEvents.Any())
            {
                return;
                
            }

            await _messageBroker.PublishAsync(integrationEvents);
        }

        private async Task<List<IEvent>> HandleDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            var integrationEvents = new List<IEvent>();
            using var scope = _scopeFactory.CreateScope();
            foreach (var domainEvent in domainEvents)
            {
                var domainEventType = domainEvent.GetType();
                _logger.LogTrace($"Handling a domain event: {domainEventType.Name}");
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEventType);
                dynamic handlers = scope.ServiceProvider.GetServices(handlerType);
                foreach (var handler in handlers)
                {
                    await handler.HandlerAsync((dynamic)domainEvent);
                }

                var integrationEvent = _eventMapper.Map(domainEvent);
                if (integrationEvent is null)
                {
                    continue;
                }

                integrationEvents.Add(integrationEvent);
            }

            return integrationEvents;
        }
    }
}
