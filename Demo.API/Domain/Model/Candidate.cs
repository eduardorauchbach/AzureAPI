using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using RauchTech.Common.Configuration;

namespace Demo.API.Domain.Model
{
    public class Candidate
    {
        public long ID { get; set; }
        public string Name { get; set; }

        public string FileID { get; set; }
        public string File { get; private set; }

        public BlobFile BlobFile { get; set; }

        public void LoadUrls(IConfiguration config)
        {
            if (!string.IsNullOrEmpty(FileID))
            {
                File = config.GetValue("Blob:BaseURL")[0] + FileID;
            }
        }

        public List<CandidateJob> CandidateJobs { get; set; }
    }
}
