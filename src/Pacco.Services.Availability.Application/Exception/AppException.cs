using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Application.Exception
{
    public abstract class AppException: System.Exception
    {
        public abstract string Code { get; }

        protected AppException(string message): base(message)
        {
            
        }
    }
}
