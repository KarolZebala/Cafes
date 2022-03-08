using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CafeApi.Custom_Authorization
{
    public class MinimumReqirementAgeHandler : AuthorizationHandler<MinimumRequirementAge>
    {
        private readonly ILogger<MinimumReqirementAgeHandler> _logger;
        public MinimumReqirementAgeHandler(ILogger<MinimumReqirementAgeHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRequirementAge requirement)
        {
            var age = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            _logger.LogInformation($"user {userEmail} with date of birth {age}");

            var userAge = age.AddYears(requirement.MinimumAge);
            if (userAge <= DateTime.Today)
            {
                _logger.LogInformation("Authorization succedded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed");

            }

            return Task.CompletedTask;
        }
    }
}
