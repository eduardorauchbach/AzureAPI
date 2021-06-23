using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Rauchbach.Common.Configuration;

namespace ATS.API.Model
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
