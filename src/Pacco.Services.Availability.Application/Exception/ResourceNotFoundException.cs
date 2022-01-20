using System;

namespace Pacco.Services.Availability.Application.Exception
{
    public class ResourceNotFoundException : AppException
    {
        public override string Code => "resource_not_found";
        public Guid ResourceId { get; }

        public ResourceNotFoundException(Guid resourceId) :
            base($"resource with id {resourceId} was not found")
        {
            ResourceId = resourceId;
        }
    }
}