using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RauchTech.Common.Configuration;
using System;
using System.IO;

namespace RauchTech.DataExtensions.AzureBlob
{
    public interface IAzureBlobHelper
    {
        AzureBlobFile Get(string id);

        AzureBlobFile InsertOrUpdate(AzureBlobFile blobFile, string oldID = null);

        void Delete(string id);
    }
}
