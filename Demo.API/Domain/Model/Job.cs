using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Demo.API.Domain.Model
{
    public class Job
    {
        #region Data Base Parse

        public static readonly Dictionary<string, string> ColumnsLibrary = new Dictionary<string, string>
        {
            { "id", "ID"},
            { "title", "Title"},
            { "description", "Description"}
        };

        #endregion

        public long ID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
