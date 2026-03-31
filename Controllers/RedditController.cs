using Microsoft.AspNetCore.Mvc;
using ServiceAnalyzer.Application.DTO;
using ServiceAnalyzer.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace ServiceAnalyzer.Controllers
{
    [ApiController]
    [Route("api/reddit")]
    public class RedditController : Controller
    {
        private readonly IRedditService _service;

        public RedditController(IRedditService service)
        {
            _service = service;
        }

        [HttpPost("analyze/file")]
        public async Task<IActionResult> AnalyzeToFile([FromBody] AnalyzeRequestDto request)
        {
            var result = await _service.AnalyzeAsync(request);

            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", "result.json");
        }
    }
}
