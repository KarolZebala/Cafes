using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Custom_Authorization
{
    public class MinimumRequirementAge : IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumRequirementAge(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
