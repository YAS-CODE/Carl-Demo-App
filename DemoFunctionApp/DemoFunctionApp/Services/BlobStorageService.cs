using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Configuration;

namespace DemoFunctionApp.Services
{
    public class BlobStorageService
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient client;
        //private readonly IConfiguration _configuration;
        public BlobStorageService(string connectionstr, string blobcontainer)
        {
            
            blobServiceClient = new BlobServiceClient(connectionstr);
            client = blobServiceClient.GetBlobContainerClient(blobcontainer);
        }

        public async Task<bool> UploadFileToBlobAsync(string strFileName, string fileData)
        {
            try
            {
                byte[] dateBytes = Encoding.UTF8.GetBytes(fileData);
                MemoryStream stream = new MemoryStream(dateBytes);
                
                await client.UploadBlobAsync(strFileName, stream);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async void DeleteBlobData(string fileUrl)
        {
            await client.DeleteBlobIfExistsAsync(fileUrl);
        }


        public bool CheckBlobExistAsync(string fileUrl)
        {
            //var blob = blobServiceClient.GetContainerReference("azure-webjobs-hosts").GetBlockBlobReference(fileUrl);
            var blobclient = client.GetBlobClient(fileUrl);
            if (blobclient.Exists())
                return true;
            return false;
        }

        public async Task<string> ReadFileToBlobAsync(string strFileName)
        {
            try
            {
                var blobclient  = client.GetBlobClient(strFileName);
                var ms = new MemoryStream();
                await blobclient.DownloadToAsync(ms);
                return Encoding.ASCII.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
    }
}
