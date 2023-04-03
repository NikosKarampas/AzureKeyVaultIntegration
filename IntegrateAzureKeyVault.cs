using System.Threading;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Company.Function
{
    public class IntegrateAzureKeyVault
    {
        private readonly ILogger _logger;
        private readonly CosmosDBOptions _cosmosDbOptions;

        public IntegrateAzureKeyVault(ILoggerFactory loggerFactory, IOptions<CosmosDBOptions> cosmosDbOptions)
        {
            _logger = loggerFactory.CreateLogger<IntegrateAzureKeyVault>();
            this._cosmosDbOptions = cosmosDbOptions.Value;
        }

        [Function("IntegrateAzureKeyVault")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var cosmosDBConnectionString = _cosmosDbOptions.CosmosConnectionString;
            var cosmosDBAccessKey = _cosmosDbOptions.CosmosAccessKey;

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString($"Connection string is {cosmosDBConnectionString} and access key is {cosmosDBAccessKey}");

            return response;
        }
    }
}
