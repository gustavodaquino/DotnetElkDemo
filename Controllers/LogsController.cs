using DotnetElkDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace DotnetElkDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController : ControllerBase
{
    private readonly IElasticClient _elasticClient;

    public LogsController(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    [HttpPost]
    public async Task<IActionResult> GenerateLog(TraceDocument traceDocument)
    {
        await _elasticClient.IndexDocumentAsync(traceDocument);

        return Ok(traceDocument);
    }
}