using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.DTO.Response
{
    /// <summary>
    /// Generic API Response Object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> where T : class
    {
        /// <summary>
        /// Message returns from the api.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// Response object of the api.
        /// </summary>
        [JsonProperty("payload")] 
        public T Payload { get; set; }
        /// <summary>
        /// Status code of the api.
        /// </summary>

        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
    }

    /// <summary>
    /// API Response Object
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Message returns from the api.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Status code of the api.
        /// </summary>
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
    }
}
