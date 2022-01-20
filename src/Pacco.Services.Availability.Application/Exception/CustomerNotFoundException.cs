using System;

namespace Pacco.Services.Availability.Application.Exception
{
    public class CustomerNotFoundException : AppException
    {
        public override string Code => "customer_not_found";
        public Guid CustomerId { get; }

        public CustomerNotFoundException(Guid customerId) :
            base($"customer with id {customerId} was not found")
        {
            CustomerId = customerId;
        }
    }
}