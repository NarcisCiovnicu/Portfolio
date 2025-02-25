using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Portfolio.API.Extensions
{
    public static class ControllerExtension
    {
        public static IActionResult UnauthorizedProblem(this ControllerBase controller, string detail)
        {
            return controller.Problem(detail, controller.HttpContext.Request.GetInstance(),
                StatusCodes.Status401Unauthorized, "Unauthorized",
                ProblemDetailsHelper.GetProblemDetailsType(StatusCodes.Status401Unauthorized));
        }
    }
}
