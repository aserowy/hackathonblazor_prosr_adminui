using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorHub.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/prosr")]
    public class HelloWorldController : ControllerBase
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
