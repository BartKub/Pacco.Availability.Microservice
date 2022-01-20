using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Core.Exceptions
{
    public abstract class DomainException: Exception
    {
        public abstract string Code { get;}

        protected DomainException(string message) : base(message)
        {

        }
    }
}
