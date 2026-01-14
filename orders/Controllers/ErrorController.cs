using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public IActionResult HandleError()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem(
            detail: exception?.Message,
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
}
