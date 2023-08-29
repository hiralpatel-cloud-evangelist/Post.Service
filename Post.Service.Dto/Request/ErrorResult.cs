using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.DTO.Request
{
    public class ErrorResult
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        //[JsonProperty(PropertyName = "success")]
        //public string Success { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "more_info")]
        public string MoreInfo { get; set; }

        //[JsonProperty(PropertyName = "data")]
        //public T Data { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }

    public class SucessResult
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        //[JsonProperty(PropertyName = "success")]
        //public string Success { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "more_info")]
        public string MoreInfo { get; set; }

        //[JsonProperty(PropertyName = "data")]
        //public T Data { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
    }

    public class FluentErrorResponse
    {
        public int Code { get; set; }
        public int Status { get; set; }
        public JObject Message { get; set; }
        public bool isFluentError { get; set; }
    }

    public class NotFoundResponse
    {
        /// <summary>
        /// (Conditional) An error code to find help for the exception.
        /// </summary>
        [JsonProperty("code")]

        public int code { get; set; }

        /// <summary>
        /// A more descriptive message regarding the exception.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }

        /// <summary>
        /// The HTTP status code for the exception.
        /// </summary>
        [JsonProperty("status")]
        public int status { get; set; }

    }


    public class BadRequestResponse
    {
        /// <summary>
        /// (Conditional) An error code to find help for the exception.
        /// </summary>
        [JsonProperty("code")]

        public int code { get; set; }
        /// <summary>
        /// A more descriptive message regarding the exception.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }

        /// <summary>
        ///  The HTTP status code for the exception.
        /// </summary>
        [JsonProperty("status")]
        public int status { get; set; }
    }

    public class InternalServerErrorReponse
    {
        /// <summary>
        /// (Conditional) An error code to find help for the exception.
        /// </summary>
        [JsonProperty("code")]

        public int code { get; set; }
        /// <summary>
        /// A more descriptive message regarding the exception.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }

        /// <summary>
        /// The HTTP status code for the exception.
        /// </summary>
        [JsonProperty("status")]
        public int status { get; set; }
    }

    public class UnauthorizedResponse
    {
        /// <summary>
        /// (Conditional) An error code to find help for the exception.
        /// </summary>
        [JsonProperty("code")]

        public int code { get; set; }

        /// <summary>
        /// A more descriptive message regarding the exception.
        /// </summary>
        [JsonProperty("message")]
        public string message { get; set; }

        /// <summary>
        /// The HTTP status code for the exception.
        /// </summary>
        [JsonProperty("status")]
        public int status { get; set; }

    }
}
