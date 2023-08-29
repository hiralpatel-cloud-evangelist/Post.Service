using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Post.Service.Base.BaseClasses;
using Post.Service.Base.BaseResponse;
using Post.Service.CQRS.Commands;
using Post.Service.DTO.Constants;
using Post.Service.DTO.FilterDto;
using Post.Service.DTO.Request;
using Post.Service.DTO.Response;
using Post.Service.Helper;
using Post.Service.Models.Tables;
using Post.Service.Services.CQRS.Queries;
using StackExchange.Redis;
using System.Net;
using System.Text.RegularExpressions;

namespace Post.Service.Controllers.V2
{
    /// <summary>
    /// API controller for managing blog posts. [WITHOUT AUTHENTICATION / FOR TESTING PURPOSE ONLY
    /// </summary>
    [ApiController]
    [ValidateModel]
    [Route("v2/posts")]
    public class PostsV2Controller : ApiControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabase _database;
        private AzureRedisCacheHelper _azureRedisCacheHelper;
        private IMediator _mediator;
        private readonly IMapper _mapper;

        public PostsV2Controller(IConfiguration configuration, IDatabase database, IMapper mapper, IMediator mediator)
        {

            _configuration = configuration;
            _database = database;
            _azureRedisCacheHelper = new AzureRedisCacheHelper(_database);
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of blog posts based on search criteria.
        /// </summary>
        /// <param name="searchRequestModel">The search criteria.</param>
        /// <returns>An <see cref="BlogPostListResponse"/> containing the list of blog posts.</returns> 
        /// <summary>
        /// Retrieves a list of blog posts based on search criteria.
        /// </summary>
        /// <response code="200">OK: The request was successful and the response body contains the representation requested.</response>
        /// <response code="401">UNAUTHORIZED: The supplied credentials, if any, are not sufficient to access the resource.</response>
        /// <response code="500">SERVER ERROR: We couldn't return the representation due to an internal server error.</response>
        /// <returns code="429">TOO MANY REQUESTS: Your application is sending too many simultaneous requests.</returns>
        [ProducesResponseType(typeof(BlogPostListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
        [HttpGet]
        public async Task<IActionResult> GetBlogs([FromQuery] SearchRequestModel searchRequestModel)
        {
            var response = await _mediator.Send(new GetPostQuery(searchRequestModel));
            return Ok(response);

        }

        /// <summary>
        /// Retrieves a blog post by its unique identifier.
        /// </summary>
        /// <param name="sid">The unique identifier (SID) of the blog post to retrieve.</param>
        /// <returns>An <see cref="BlogPostResponse"/> containing the retrieved blog post.</returns>
        /// <response code="200">OK: The request was successful and the response body contains the representation requested.</response>
        /// <response code="401">UNAUTHORIZED: The supplied credentials, if any, are not sufficient to access the resource.</response>
        /// <response code="500">SERVER ERROR: We couldn't return the representation due to an internal server error.</response>
        /// <returns code="429">TOO MANY REQUESTS: Your application is sending too many simultaneous requests.</returns>
        [ProducesResponseType(typeof(BlogPostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
        [HttpGet]
        [Route("{sid}")]
        public async Task<IActionResult> GetById([FromRoute] string sid)
        {

            if (await _azureRedisCacheHelper.IsInCache(sid))
            {
                var blogPostResponse = await _azureRedisCacheHelper.Get<BlogPostResponse>(sid);
                return Ok(blogPostResponse);

            }
            var blogDetail = await _mediator.Send(new GetByIdPostQuery(sid));
            await _azureRedisCacheHelper.Set(sid, blogDetail);

            return Ok(blogDetail);
        }

        /// <summary>
        /// Adds a new blog post.
        /// </summary>
        /// <param name="requestModel">The data of the new blog post.</param>
        /// <returns>An <see cref="BlogPostResponse"/> containing the retrieved blog post.</returns>
        /// <response code="201">OK: The request was successful and the response body contains the representation requested.</response>
        /// <response code="401">UNAUTHORIZED: The supplied credentials, if any, are not sufficient to access the resource.</response>
        /// <response code="500">SERVER ERROR: We couldn't return the representation due to an internal server error.</response>
        /// <returns code="429">TOO MANY REQUESTS: Your application is sending too many simultaneous requests.</returns>
        /// <response code="400">BAD REQUEST: The data given in the POST or PUT failed validation. Inspect the response body for details.</response>
        [ProducesResponseType(typeof(BlogPostResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddPost([FromForm] BlogPostRequestModel requestModel)
        {
            //Add post to database
            BlobMetadataModel blobMetadataModel = new BlobMetadataModel();
            blobMetadataModel.BlobConnectionString = _configuration.GetValue<string>("BlobStorageConnection");
            blobMetadataModel.BlobContainer = _configuration.GetValue<string>("BlobStorageContainer");
            //only images are allowed to upload in post
            Regex filetypeTypeRegex = new Regex(CommonConstants.ImageFileRegex);
            if (requestModel.File != null)
            {
                if (!filetypeTypeRegex.IsMatch(requestModel.File.FileName))
                {
                    throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, CommonConstants.FileNotValidErrorMessage);
                }
            }

            var response = await _mediator.Send(new CreatePostCommand(requestModel, blobMetadataModel));
            return CommonCreatedResult(response);
        }

        /// <summary>
        /// Updates an existing blog post.
        /// </summary>
        /// <param name="requestModel">The updated data for the blog post.</param>
        /// <param name="sid">The unique identifier (SID) of the blog post to update.</param>
        /// <returns>An <see cref="BlogPostResponse"/> containing the retrieved blog post..</returns>
        /// <response code="200">OK: The request was successful and the response body contains the representation requested.</response>
        /// <response code="401">UNAUTHORIZED: The supplied credentials, if any, are not sufficient to access the resource.</response>
        /// <response code="500">SERVER ERROR: We couldn't return the representation due to an internal server error.</response>
        /// <returns code="429">TOO MANY REQUESTS: Your application is sending too many simultaneous requests.</returns>
        /// <response code="400">BAD REQUEST: The data given in the POST or PUT failed validation. Inspect the response body for details.</response>

        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(BlogPostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]

        [HttpPut("{sid}")]
        public async Task<IActionResult> UpdatePost([FromForm] BlogPostRequestModel requestModel, [FromRoute] string sid)
        {

            BlobMetadataModel blobMetadataModel = new BlobMetadataModel();
            blobMetadataModel.BlobConnectionString = _configuration.GetValue<string>("BlobStorageConnection");
            blobMetadataModel.BlobContainer = _configuration.GetValue<string>("BlobStorageContainer");

            Regex filetypeTypeRegex = new Regex(CommonConstants.ImageFileRegex);
            if (requestModel.File != null)
            {
                if (!filetypeTypeRegex.IsMatch(requestModel.File.FileName))
                {
                    throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, CommonConstants.FileNotValidErrorMessage);
                }
            }

            var result = await _mediator.Send(new EditPostCommand(requestModel, sid, blobMetadataModel));
            return CommonUpdateResult(result);

        }


        /// <summary>
        /// Deletes a blog post by its unique identifier.
        /// </summary>
        /// <param name="sid">The unique identifier (SID) of the blog post to delete.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the success or failure of the operation.</returns>
        /// <response code="204">No Content: The request was successful and the response body contains the representation requested.</response>
        /// <response code="401">UNAUTHORIZED: The supplied credentials, if any, are not sufficient to access the resource.</response>
        /// <response code="500">SERVER ERROR: We couldn't return the representation due to an internal server error.</response>
        /// <returns code="429">TOO MANY REQUESTS: Your application is sending too many simultaneous requests.</returns>
        /// <response code="400">BAD REQUEST: The data given in the POST or PUT failed validation. Inspect the response body for details.</response>

        [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status429TooManyRequests)]
        [HttpDelete("{sid}")]
        public async Task<IActionResult> DeletePost([FromRoute] string sid)
        {
            await _mediator.Send(new DeletePostCommand(sid));
            return CommonDeletedResult();

        }
    }
}
