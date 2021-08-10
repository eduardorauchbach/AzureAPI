using Demo.API.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RauchTech.Common.Configuration;
using System;
using System.IO;

namespace Demo.API.Domain.Data.Base
{
    public class BlobHelper
    {
        private readonly string _blobConnection;
        private readonly string _blobFolder;

        private readonly IConfiguration _config;

        public BlobHelper(IConfiguration config)
        {
            _config = config;
            _blobConnection = _config.GetValue("Blob:Connection")[0];
            _blobFolder = _config.GetValue("Blob:Folder")[0];
        }

        public BlobFile Get(string id)
        {
            CloudStorageAccount storageAccount;

            CloudBlobClient blobClient;
            CloudBlobContainer container;
            CloudBlobDirectory directory;

            CloudBlob cloudBlob;

            BlobFile blobFile;

            try
            {
                storageAccount = CloudStorageAccount.Parse(_blobConnection);
                blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference(_blobFolder);

                directory = container.GetDirectoryReference(string.Empty);
                cloudBlob = directory.GetBlobReference(id);

                _ = cloudBlob.FetchAttributesAsync();

                blobFile = new BlobFile();
                blobFile.ID = cloudBlob.Name;
                blobFile.Created = cloudBlob.Properties.Created?.DateTime;

                using (MemoryStream downloadFileStream = new MemoryStream())
                {
                    cloudBlob.DownloadToStreamAsync(downloadFileStream).Wait();
                    blobFile.Data = Convert.ToBase64String(downloadFileStream.ToArray());
                }
            }
            catch
            {
                throw;
            }

            return blobFile;
        }

        public BlobFile InsertOrUpdate(BlobFile blobFile, string oldID = null)
        {
            CloudStorageAccount storageAccount;

            CloudBlobClient blobClient;
            CloudBlobContainer container;
            CloudBlockBlob blockBlob;

            string baseName;
            string currentExtension;
            string newName;

            byte[] currentData;

            try
            {
                //Getting new ID with fixed extension				
                baseName = Path.GetFileNameWithoutExtension(blobFile.ID);
                currentExtension = Path.GetExtension(blobFile.Name);
                newName = baseName + currentExtension;

                storageAccount = CloudStorageAccount.Parse(_blobConnection);
                blobClient = storageAccount.CreateCloudBlobClient();

                container = blobClient.GetContainerReference(_blobFolder);

                if (oldID != null)
                {
                    blockBlob = container.GetBlockBlobReference(oldID);
                    blockBlob.DeleteIfExistsAsync().Wait();
                }

                blockBlob = container.GetBlockBlobReference(blobFile.ID);
                blockBlob.DeleteIfExistsAsync().Wait();

                //Preventing wrong file format
                blobFile.ID = newName;
                blockBlob = container.GetBlockBlobReference(blobFile.ID);
                blockBlob.DeleteIfExistsAsync().Wait();

                currentData = Convert.FromBase64String(blobFile.Data);
                _ = blockBlob.UploadFromByteArrayAsync(currentData, 0, currentData.Length);
            }
            catch
            {
                throw;
            }

            return blobFile;
        }

        public void Delete(string id)
        {
            CloudStorageAccount storageAccount;

            CloudBlobClient blobClient;
            CloudBlobContainer container;
            CloudBlobDirectory directory;

            CloudBlob cloudBlob;

            try
            {
                storageAccount = CloudStorageAccount.Parse(_blobConnection);
                blobClient = storageAccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference(_blobFolder);

                directory = container.GetDirectoryReference(string.Empty);
                cloudBlob = directory.GetBlobReference(id);

                _ = cloudBlob.DeleteIfExistsAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
