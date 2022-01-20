using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Services.Clients
{
    public interface ICustomerServiceClient
    {
        Task<CustomerStateDto> GetStateAsync(Guid customerId);
    }
}
