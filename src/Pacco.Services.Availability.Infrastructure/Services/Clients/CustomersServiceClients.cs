using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Services.Clients;

namespace Pacco.Services.Availability.Infrastructure.Services.Clients
{
    internal class CustomersServiceClients: ICustomerServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly HttpClientOptions _options;
        private readonly string _url;

        public CustomersServiceClients(IHttpClient httpClient, 
            HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["customers"];
        }

        public Task<CustomerStateDto> GetStateAsync(Guid customerId) =>
            _httpClient.GetAsync<CustomerStateDto>($"{_url}/customers/{customerId}/state");

    }
}
