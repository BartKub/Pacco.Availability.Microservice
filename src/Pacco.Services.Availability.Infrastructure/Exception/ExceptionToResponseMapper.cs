using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Convey.WebApi.Exceptions;
using Pacco.Services.Availability.Application.Exception;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Infrastructure.Exception
{
    internal sealed class ExceptionToResponseMapper: IExceptionToResponseMapper
    {
        public ExceptionResponse Map(System.Exception exception)
            => exception switch
            {
                DomainException ex => new ExceptionResponse(new {code = ex.Code, reason = ex.Message},
                    HttpStatusCode.BadRequest),
                AppException ex => new ExceptionResponse(new { code = ex.Code, reason = ex.Message },
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new {code ="error", reason ="There was an error."}, HttpStatusCode.InternalServerError)
            };
    }
}
