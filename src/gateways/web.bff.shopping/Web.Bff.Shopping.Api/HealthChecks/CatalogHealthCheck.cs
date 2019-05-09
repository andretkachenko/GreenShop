using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Api.HealthChecks
{
    public class CatalogHealthCheck : IHealthCheck
    {
        public string Url { get; }

        public CatalogHealthCheck(string url)
        {
            Url = url;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new Exception($"Catalog service was not found at {Url}.");
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
            }

            return HealthCheckResult.Healthy();
        }
    }
}
