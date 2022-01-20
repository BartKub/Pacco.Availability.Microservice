using System;

namespace Pacco.Services.Availability.Application.Services.Clients
{
    public class CustomerServiceDto
    {
        public string State { get; set; }
        public bool IsValid => State.Equals("valid", StringComparison.InvariantCulture);
    }
}