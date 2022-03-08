using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Custom_Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirment:IAuthorizationRequirement
    {

        public ResourceOperation ResourceOperation { get; set; }

        public ResourceOperationRequirment(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }

    }
}
