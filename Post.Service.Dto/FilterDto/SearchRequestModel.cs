using Newtonsoft.Json;

namespace Post.Service.DTO.FilterDto
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public SearchRequestModel()
        {
            //set default pagesize as 10 and 1st page if no search criteria specified in request
          if(Page == 0) Page = 1;
          if(PageSize == 0) PageSize = 10;
        }

        /// <summary>
        /// Search string to look up for matching results. 
        /// </summary>
        [JsonProperty(PropertyName = "search_text")]
        public string? SearchText { get; set; }

        /// <summary>
        /// Expected page number in the result set.
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        /// <summary>
        /// Page size of the result set.
        /// </summary>
        [JsonProperty(PropertyName = "page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// The column / attribute by which the results shall be sorted.
        /// </summary>
        [JsonProperty(PropertyName = "sort_column")]
        public string? SortColumn { get; set; }

        /// <summary>
        /// The order by which the results shall be sorted.  Possible values are 'asc' for ascending order, 'desc' for descending order.
        /// </summary>
        [JsonProperty(PropertyName = "sort_order")]
        public string? SortOrder { get; set; }

        ///// <summary>
        ///// Search filter list to look up for matching results. If must be in format '[{key:'keyname',value:'keyvalue'},{key:'keyname',value:'keyvalue'}]'.
        ///// </summary>
        //[JsonProperty(PropertyName = "filters")]
        //public string? Filters { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[JsonProperty(PropertyName = "user_id")]
        //public Guid? UserId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FilterRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "key", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "condition", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Condition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "from", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object From { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "to", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; }
    }
}
