using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.DTO.FilterDto
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedViewResponse<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        public PagedViewResponse()
        {
            PageSize = 10;
            Data = new List<T>();
            Pages = new List<int>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("number_of_pages")]
        public int NumberofPages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("page_size")]
        public int PageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("record_count")]
        public int RecordCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("page_list")]
        public List<int> Pages { get; set; }
    }
}
