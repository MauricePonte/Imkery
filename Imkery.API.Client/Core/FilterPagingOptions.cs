using Newtonsoft.Json;
using System.Collections.Generic;

namespace Imkery.API.Client.Core
{
    public class FilterPagingOptions
    {
        public Dictionary<string, string> FilterParameters { get; set; } = new Dictionary<string, string>();
        public string SortProperty { get; set; }

        public string[] Includes { get; set; }

        public int Page { get; set; } = 0;

        public int ItemsPerPage { get; set; } = 5;

        public string ToQueryString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static FilterPagingOptions FromString(string json)
        {
            return JsonConvert.DeserializeObject<FilterPagingOptions>(json);
        }
    }
}