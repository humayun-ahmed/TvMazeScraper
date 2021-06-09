using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Rtl.TvMaze.Api.HealthCheck
{
    public class HealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var healthCheckResultHealthy = true;//ToDo this will be implemented according to the deployment environment and different other consideration
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("test","test" );
            IReadOnlyDictionary<string, object> data = dictionary;
            

            var healthCheckResult = HealthCheckResult.Healthy("A healthy result.", data);

            return Task.FromResult(healthCheckResultHealthy ? healthCheckResult : HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}
