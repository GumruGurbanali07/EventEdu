using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IStringLocalizer<TestController> localizer) : Controller
{
    [HttpPost]
    public IActionResult Test([FromBody] RequestData data)
    {
        return Ok(localizer["EmailInvalidError"].Value);
    }
}

public class RequestData
{
    public string Name { get; set; }
}