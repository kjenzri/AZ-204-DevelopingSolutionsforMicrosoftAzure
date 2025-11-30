using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace func;

public class FileParser
{
    private readonly ILogger<FileParser> _logger;

    public FileParser(ILogger<FileParser> logger)
    {
        _logger = logger;
    }

    [Function("FileParser")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "test/plain; charset=utf-8");
        
        var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString")??string.Empty;
        
        BlobClient blobClient = new BlobClient(connectionString, "drop", "records.json");

        BlobDownloadResult blobDownloadResult = await blobClient.DownloadContentAsync();

        await response.WriteStringAsync(blobDownloadResult.Content.ToString());

        return response;
    }
}
