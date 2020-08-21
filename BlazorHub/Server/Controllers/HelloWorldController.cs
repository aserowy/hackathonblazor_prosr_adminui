using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHub.Server.Controllers
{
    [ApiController]
    [Route("[controller]/prosr")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public static Task<string> GetAsync()
        {
            return System.IO.File.ReadAllTextAsync("HelloWorldHub.prosr");
        }
    }
}
