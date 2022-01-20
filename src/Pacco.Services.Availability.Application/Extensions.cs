using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;

[assembly: InternalsVisibleTo("Pacco.Services.Availability.Tests.Unit")]
namespace Pacco.Services.Availability.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder) => 
            builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();
    }
}
