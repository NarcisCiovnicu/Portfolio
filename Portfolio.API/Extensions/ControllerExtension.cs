using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Portfolio.API.Extensions;

public static class ControllerExtension
{
    public static ProblemDetails UnauthorizedProblem(this ControllerBase controller, string detail)
    {
        return new ProblemDetails()
        {
            Detail = detail,
            Title = "Unauthorized",
            Instance = controller.HttpContext.Request.GetInstance(),
            Status = StatusCodes.Status401Unauthorized,
            Type = ProblemDetailsHelper.GetProblemDetailsType(StatusCodes.Status401Unauthorized)
        };
    }
}
