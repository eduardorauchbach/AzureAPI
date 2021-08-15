using Microsoft.Extensions.Configuration;
using RauchTech.DataExtensions.AzureBlob;
using System;
using System.IO;

namespace RauchTech.DataExtensions.AzureBlob
{
    public interface IAzureBlobService
    {
        AzureBlobFile Insert(AzureBlobFile blobFile);

        AzureBlobFile Update(AzureBlobFile blobFile);

        void Delete(string id);


        AzureBlobFile Get(string id);
    }
}
