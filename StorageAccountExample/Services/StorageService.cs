using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountExample.Services
{
    public static class StorageService
    {

        private static BlobContainerClient _containerClient { get; set; }
        private static BlobClient _blobClient { get; set; }


        public static async Task InitilizeStorageAsync(string connectionString, string containerName, bool uniqueName = false)
        {
            if (uniqueName)
            {
                containerName = $"{containerName}-{Guid.NewGuid()}";
            }


            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                try
                {
                    _containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
                }
                catch 
                {
                    try
                    {
                        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    }
                    catch 
                    {
                        Console.WriteLine("Failed Third Initilize");

                    }
                    
                }
            }
            catch 
            {
                Console.WriteLine("Failed First Initilize");
                
            }



        }

        public static async Task WriteToFileAsync(string @filePath, string content)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }


                await File.WriteAllTextAsync(filePath, content);
            }
            catch 
            {
                Console.WriteLine("Write Failed");
               
            }

           
        }

        public static async Task UploadFileAsync(string @filePath)
        {
            try
            {
                _blobClient = _containerClient.GetBlobClient(Path.GetFileName(filePath));

                using FileStream fileStream = File.OpenRead(filePath);
                await _blobClient.UploadAsync(fileStream, true);
                fileStream.Close();
                
            }
            catch 
            {
                Console.WriteLine("Upload Failed");
                
            }

           

        }

        public static async Task DownloadFileAsync(string @downloadPath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(downloadPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));

                    BlobDownloadInfo download = await _blobClient.DownloadAsync();

                    using FileStream fileStream = File.OpenWrite(downloadPath);

                    await download.Content.CopyToAsync(fileStream);
                    fileStream.Close();

                    

                }
            }
            catch 
            {
                Console.WriteLine("DownloadFail");
                
            }
            
           
           
        }

        public static async Task<string>   ReadDownloadFileAsync(string @downloadPath)
        {
            try
            {
                return await File.ReadAllTextAsync(downloadPath);
                
            }
            catch 
            {
                Console.WriteLine("ReadDownload Failed");
                return "";
               
            }
            

        }
    }
}
