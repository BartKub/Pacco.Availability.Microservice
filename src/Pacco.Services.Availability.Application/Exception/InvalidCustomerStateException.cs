using System;

namespace Pacco.Services.Availability.Application.Exception
{
    public class InvalidCustomerStateException : AppException
    {
        public override string Code => "customer_state_not_valid";
        public Guid CustomerId { get; }
        public string State { get; }

        public InvalidCustomerStateException(Guid customerId, string state) :
            base($"customer state with id {customerId} was not valid")
        {
            CustomerId = customerId;
            State = state;
        }
    }
}