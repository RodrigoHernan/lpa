
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inmobiliaria.Authorization {
    public class PatenteRequirement : IAuthorizationRequirement
    {
    }


    public class PatenteHandler : AuthorizationHandler<PatenteRequirement>
    {
        // private readonly TokenValidator _tokenValidator;

        // public PatenteHandler(TokenValidator tokenValidator)
        // {
        //     _tokenValidator = tokenValidator;
        // }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PatenteRequirement requirement)
        {
            // if (context.Resource is AuthorizationFilterContext mvcContext)
            // {

            //     if (userIsAuthenticated == false)
            //     {
            //         context.Fail();
            //     }
            //     if (userIsAuthenticated == true)
            //     {
                    context.Succeed(requirement);
            //     }
            // }
            return Task.CompletedTask;
        }
    }

}
