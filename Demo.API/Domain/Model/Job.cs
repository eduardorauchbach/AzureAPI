using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Demo.API.Domain.Model
{
    public class Job
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
