// #define _claims
#define _applicationuser

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;

namespace MusicStore.Auth
{

#if _claims
    public class MinimumOrderAgeRequirement :
        AuthorizationHandler<MinimumOrderAgeRequirement>, IAuthorizationRequirement
    {
        private readonly int _minimumOrderAge;

        public MinimumOrderAgeRequirement(int minimumOrderAge)
        {
            _minimumOrderAge = minimumOrderAge;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOrderAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            var birthDate = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            var ageInYears = DateTime.Today.Year - birthDate.Year;

            if (ageInYears >= _minimumOrderAge)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
#endif

#if _applicationuser

    //https://stackoverflow.com/questions/42423282/dependency-injection-on-authorizationoptions-requirement-in-dotnet-core

    public class MinimumOrderAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumOrderAge;

        public MinimumOrderAgeRequirement(int minimumOrderAge)
        {
            MinimumOrderAge = minimumOrderAge;
        }
    }


    public class MinimumOrderAgeRequirementHandler : AuthorizationHandler<MinimumOrderAgeRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MinimumOrderAgeRequirementHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOrderAgeRequirement requirement)
        {


            //var taskGetUser =  _userManager.GetUserAsync(context.User);
            //taskGetUser.Wait();
            //var birthDate = taskGetUser.Result.Birthdate;

            var user = await _userManager.GetUserAsync(context.User);
            var birthDate = user.Birthdate;

            var ageInYears = DateTime.Today.Year - birthDate.Year;

            if (ageInYears >= requirement.MinimumOrderAge)
            {
                context.Succeed(requirement);
            }

            //return Task.CompletedTask;
        }
    }
#endif


}
