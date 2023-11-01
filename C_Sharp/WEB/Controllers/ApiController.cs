public class ApiController : Controller
{
    private readonly IConfiguration _configuration;

    public ApiController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("/api")]
    public IActionResult GetSecret()
    {
        var secretValue = _configuration["keyvalue1"];
        return Content($"Hello, world! This is : {secretValue}");
    }
}
