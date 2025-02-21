using Microsoft.AspNetCore.Mvc;
using Portfolio.API.Domain;

namespace Portfolio.API.Extensions
{
    public static class ControllerExtension
    {
        public static IActionResult UnauthorizedProblem(this ControllerBase controller, string detail)
        {
            return controller.Problem(detail, controller.HttpContext.Request.GetInstance(),
                StatusCodes.Status401Unauthorized, "Unauthorized",
                Constants.ProblemDetailsType.Status400);
        }
    }
}
