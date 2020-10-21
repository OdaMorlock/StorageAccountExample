using StorageAccountExample.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StorageAccountExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=godlike;AccountKey=7/YNhTvUi1d0/q52bdBADb+/fDfLDiit7SCPlwNeo5voaNu8J1WmfIcyN9SUuiWGTSf7Oa7sbE3zAdYZs1yzmQ==;EndpointSuffix=core.windows.net";
            var containerName = "godlikes";

            var fileName = $"myfile-{Guid.NewGuid()}.txt";
            var content = "this is the content of the file ";
            var filePath = Path.Combine(@"d:\WIN20\", fileName);
            var downloadPath = Path.Combine(@"d:\WIN20\Download\",fileName);

            Console.WriteLine("Initializing Storage Account With ContainerName:" + containerName );
            await  StorageService.InitilizeStorageAsync(connectionString, containerName);

            Console.WriteLine("Creating and Writeing content in file: " + filePath);
            await StorageService.WriteToFileAsync(filePath, content);

            Console.WriteLine("Uploading file to Azure Storage Blob in container " + containerName);
            await StorageService.UploadFileAsync(filePath);

            Console.WriteLine("Downloading file from Azure Storage Blob to: " + Path.GetDirectoryName(downloadPath));
            await StorageService.DownloadFileAsync(downloadPath);

            Console.WriteLine("Reading content from file: " + downloadPath);
            Console.WriteLine(await StorageService.ReadDownloadFileAsync(downloadPath));



        }
    }
}
