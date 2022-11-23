using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace AuthenticationAuthorization.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy="VerifyAnotherMagicWord")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "HelloWorld")]
    [Authorize(Policy = "VerifyMagicWord")]
    public ActionResult Get()
    {
        return Ok(new {Message = "HelloWorld" });
    }
}
