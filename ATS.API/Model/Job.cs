using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ATS.API.Model
{
    public class Job
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
