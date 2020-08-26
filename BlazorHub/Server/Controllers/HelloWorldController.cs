using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorHub.Server.Controllers
{
    [ApiController]
    [Route("hub/[controller]/prosr")]
    public class HelloWorldHubController : ControllerBase
    {
        [HttpGet]
        public Task<string> GetAsync()
        {
            var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var definitionPath = Path.Combine(workingDir, "HelloWorldHub.prosr");

            return System.IO.File.ReadAllTextAsync(definitionPath);
        }
    }
}
