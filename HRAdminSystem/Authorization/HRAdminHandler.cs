using Microsoft.AspNetCore.Authorization;

namespace HRAdminSystem.Authorization
{
    public class HRAdminHandler : AuthorizationHandler<HRAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRAdminRequirement requirement)
        {
            if (context.User.IsInRole("HRAdmin"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class HRAdminRequirement : IAuthorizationRequirement { }
}
