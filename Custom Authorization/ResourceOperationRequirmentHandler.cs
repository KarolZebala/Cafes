using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CafeApi.Entities;
using Microsoft.AspNetCore.Authorization;

namespace CafeApi.Custom_Authorization
{
    public class ResourceOperationRequirmentHandler : AuthorizationHandler<ResourceOperationRequirment, Cafe>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOperationRequirment requirement, Cafe cafe)
        {
            if(requirement.ResourceOperation==ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            //chcemy sprawdzić czy dany użytkownik jest tym, który utworzył daną kawiarnię
            //pobranie id usera
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (cafe.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
