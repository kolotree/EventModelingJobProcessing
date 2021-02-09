using System.Threading.Tasks;
using JobProcessing.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Function.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FunctionController : ControllerBase
    {
        private readonly IStore _store;

        public FunctionController(IStore store)
        {
            _store = store;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var functionResult = await new FunctionHandler(_store).Handle(HttpContext.Request);
            return new JsonResult(functionResult) {StatusCode = functionResult.StatusCode};
        }
    }
}