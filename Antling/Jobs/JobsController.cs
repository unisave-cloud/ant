using System.Threading;
using System.Threading.Tasks;
using Antling.Common;
using Antling.Docker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Antling.Jobs
{
    [ApiController]
    public class JobsController : Controller
    {
        private readonly ILogger<JobsController> logger;

        public JobsController(ILogger<JobsController> logger)
        {
            this.logger = logger;
        }
        
        [HttpPost("/execute-job")]
        public async Task<ExecuteJobResponse> ExecuteJob(
            [FromBody] ExecuteJobRequestBody body,
            [FromServices] JobRunner runner
        )
        {
            logger.LogInformation("Running a new job.");
            
            return await runner.ExecuteJob(body);
        }
    }
}