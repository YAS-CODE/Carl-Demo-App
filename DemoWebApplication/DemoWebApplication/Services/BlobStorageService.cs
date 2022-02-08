using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DemoWebApplication.Services.Interface;
using System.Text;

namespace DemoWebApplication.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient client;
        private readonly IConfiguration _configuration;
        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AccessKey"));
            client = blobServiceClient.GetBlobContainerClient(configuration.GetValue<string>("BlobContainter"));
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
                var blobclient = client.GetBlobClient(strFileName);
                var ms = new MemoryStream();
                await blobclient.DownloadToAsync(ms);
                return Encoding.ASCII.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<string>> GetListAsync(string path, bool files, string? prefix, int? segmentSize)
        {

            var blobServiceClient2 = new BlobServiceClient(_configuration.GetConnectionString("AccessKey"));
            var client2 = blobServiceClient2.GetBlobContainerClient(_configuration.GetValue<string>("BlobContainter"));

            List<string> directoryNames = new List<string>();
            try
            {
                
                // Call the listing operation and return pages of the specified size.
                var resultSegment = client2.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: path)
                    .AsPages(default, segmentSize);

                // Enumerate the blobs returned for each page.
                await foreach (Azure.Page<BlobHierarchyItem> blobPage in resultSegment)
                {
                    // A hierarchical listing may return both virtual directories and blobs.
                    foreach (BlobHierarchyItem blobhierarchyItem in blobPage.Values)
                    {
                        if (blobhierarchyItem.IsPrefix && !files)
                        {
                            // Write out the prefix of the virtual directory.
                            //Console.WriteLine("Virtual directory prefix: {0}", blobhierarchyItem.Prefix);
                            directoryNames.Add(blobhierarchyItem.Prefix);
                            // Call recursively with the prefix to traverse the virtual directory.
                            //await ListBlobsHierarchicalListing(container, blobhierarchyItem.Prefix, null);
                        }
                        else if(files)
                        {
                            // Write out the name of the blob.
                            //Console.WriteLine("Blob name: {0}", blobhierarchyItem.Blob.Name);
                            directoryNames.Add(blobhierarchyItem.Blob.Name);
                        }
                    }

                    
                }
            }
            catch (Exception e)
            {
                
            }
            return directoryNames;
        }
    }
}
