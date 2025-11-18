
using System.Reflection.Metadata;
using Azure.Storage;
using Azure.Storage.Blobs;

var blobServiceEndpoint = "https://mediastorekarim.blob.core.windows.net/";
var storageAccountName = "mediastorekarim";
var storageAccountKey = "<KEY>";

var accountCredentials = new StorageSharedKeyCredential(storageAccountName, 
                            storageAccountKey);
var serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
var info = await serviceClient.GetAccountInfoAsync();

await Console.Out.WriteLineAsync("Connected to Azure Storage Account");
await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
await Console.Out.WriteLineAsync($"Account kind:\t{info?.Value.AccountKind}");
await Console.Out.WriteLineAsync($"Account sku:\t{info?.Value.SkuName}");

await foreach(var container in serviceClient.GetBlobContainersAsync())
{
    await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
    var containerClient = serviceClient.GetBlobContainerClient(container.Name);
    await foreach(var blob in containerClient.GetBlobsAsync())
    {
        await Console.Out.WriteLineAsync($"\tExisting Blob:\t{blob.Name}");
    }
}

var newContainerName = "vector-graphics";
var newContainer = serviceClient.GetBlobContainerClient(newContainerName);
await newContainer.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

var newContainerClient = serviceClient.GetBlobContainerClient(newContainer.Name);
var uploadedBlobName = "graph.svg";
var blobClient = newContainerClient.GetBlobClient(uploadedBlobName);
if(!await blobClient.ExistsAsync())
{
    await Console.Out.WriteLineAsync($"Blob {blobClient.Name} not found!");
}
else
{
    await Console.Out.WriteLineAsync($"Blob found, URI:\t{blobClient.Uri}");
}