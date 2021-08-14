using System;

namespace RauchTech.DataExtensions.AzureBlob
{
    public class AzureBlobFile
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public string Data { get; set; }
    }
}
