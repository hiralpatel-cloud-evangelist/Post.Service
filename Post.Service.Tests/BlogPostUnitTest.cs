using AutoMapper;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Post.Service.Base.BaseClasses;
using Post.Service.Base.BaseResponse;
using Post.Service.Controllers.V1;
using Post.Service.CQRS.Commands;
using Post.Service.DTO.Constants;
using Post.Service.DTO.FilterDto;
using Post.Service.DTO.Request;
using Post.Service.DTO.Response;
using Post.Service.Helper;
using Post.Service.Models.Tables;
using Post.Service.Services.CQRS.Queries;
using StackExchange.Redis;
using static Post.Service.CQRS.Commands.CreatePostCommand;

namespace Post.Service.Tests
{
    public class BlogPostUnitTest
    {
        private readonly IConfiguration _configuration;
        public BlogPostUnitTest()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsTest.json");

            _configuration = configurationBuilder.Build();

        }



        [Fact]
        public async Task GetBlogs_ReturnsOkResult()
        {
            // Arrange

            var mockDatabase = new Mock<IDatabase>();
            var mockAzureRedisCacheHelper = new Mock<AzureRedisCacheHelper>(mockDatabase);
            var mockMapper = new Mock<IMapper>();
            var mockSearchModel = new Mock<SearchRequestModel>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);

            var searchRequestModel = new SearchRequestModel
            {
                SearchText = string.Empty,
                PageSize = 10,
                Page = 1,
                SortColumn = string.Empty,
                SortOrder = string.Empty
            };

            var response = new BlogPostListResponse
            {
                BlogPostList = new PagedViewResponse<BlogPostResponse>()
                {
                    CurrentPage = 1,
                    NumberofPages = 1,
                    Pages = new List<int> { 1, 2 },
                    PageSize = 1,
                    RecordCount = 1,
                    Data = new List<BlogPostResponse>()
                    {
                       new BlogPostResponse()
                       {
                            BlogPostSid = "NewSID",
                            PostName = "Test",
                            PostDescription = "Test",
                            BlogImage = "image_string"
                       }
                    }
                }
            };

            mockMediator.Setup(m => m.Send(It.IsAny<GetPostQuery>(), CancellationToken.None))
                        .ReturnsAsync(response);

            // Act
            var result = await controller.GetBlogs(searchRequestModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            mockMediator.Verify(x => x.Send(It.IsAny<GetPostQuery>(), CancellationToken.None), Times.Once);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task CreateBlogPostAsync()
        {
            var mockDatabase = new Mock<IDatabase>();
            var mockAzureRedisCacheHelper = new Mock<AzureRedisCacheHelper>(mockDatabase);
            var mockMapper = new Mock<IMapper>();
            var mockSearchModel = new Mock<SearchRequestModel>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);

            // Create request model
            var request = new BlogPostRequestModel()
            {
                PostName = "test",
                PostDescription = "test description",
                File = null

            };


            // Create blob metadata model
            var blobMetadataModel = new BlobMetadataModel()
            {
                BlobConnectionString = "test",
                BlobContainer = "images"
            };


            // Prepare the response for CreatePostCommand
            var expectedResponse = new BlogPostResponse
            {
                BlogImage = "image_string",
                BlogPostSid = "NewSID",
                PostDescription = "test description",
                PostName = "test"
            };


            // Setup mock IMediator behavior for Send
            mockMediator.Setup(m =>
                m.Send(It.IsAny<CreatePostCommand>(), CancellationToken.None))
                .ReturnsAsync((CreatePostCommand command, CancellationToken token) =>
                {
                    return expectedResponse;
                });

            // Act
            var result = await controller.AddPost(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            mockMediator.Verify(x => x.Send(It.IsAny<CreatePostCommand>(), CancellationToken.None), Times.Once);
            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
            Assert.Equal(expectedResponse, objectResult.Value);

        }


        [Fact]
        public async Task UpdateBlogPostAsync()
        {
            var mockDatabase = new Mock<IDatabase>();
            var mockAzureRedisCacheHelper = new Mock<AzureRedisCacheHelper>(mockDatabase);
            var mockMapper = new Mock<IMapper>();
            var mockSearchModel = new Mock<SearchRequestModel>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);

            // Create request model
            var request = new BlogPostRequestModel()
            {
                PostName = "test",
                PostDescription = "test description",
                File = null

            };

            var sid = "unique sid";


            // Create blob metadata model
            var blobMetadataModel = new BlobMetadataModel()
            {
                BlobConnectionString = "test",
                BlobContainer = "images"
            };


            // Prepare the response for CreatePostCommand
            var expectedResponse = new BlogPostResponse
            {
                BlogImage = "image_string",
                BlogPostSid = "NewSID",
                PostDescription = "test description",
                PostName = "test"
            };


            // Setup mock IMediator behavior for Send
            mockMediator.Setup(m =>
                m.Send(It.IsAny<EditPostCommand>(), CancellationToken.None))
                .ReturnsAsync((EditPostCommand command, CancellationToken token) =>
                {
                    return expectedResponse;
                });

            // Act
            var result = await controller.UpdatePost(request, sid);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            mockMediator.Verify(x => x.Send(It.IsAny<EditPostCommand>(), CancellationToken.None), Times.Once);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.Equal(expectedResponse, okObjectResult.Value);

        }

        [Fact]
        public async Task GetBlogById_ReturnsOkResult()
        {
            // Arrange

            var mockDatabase = new Mock<IDatabase>();
            var mockAzureRedisCacheHelper = new Mock<AzureRedisCacheHelper>(mockDatabase);
            var mockMapper = new Mock<IMapper>();
            var mockSearchModel = new Mock<SearchRequestModel>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);

            string sid = "random string";
            var response = new BlogPostResponse
            {
                BlogPostSid = "",
                PostName = "Test",
                PostDescription = "Test",
                BlogImage = ""
            };

            mockDatabase.Setup(x => x.KeyExistsAsync(It.IsAny<RedisKey>(), CommandFlags.None)).ReturnsAsync(false);

            mockAzureRedisCacheHelper.Setup(x => x.IsInCache(It.IsAny<string>())).ReturnsAsync(false);

            mockMediator.Setup(m => m.Send(It.IsAny<GetByIdPostQuery>(), CancellationToken.None))
                        .ReturnsAsync(response);


            // Act
            var result = await controller.GetById(sid);

            // Assert
            mockMediator.Verify(x => x.Send(It.IsAny<GetByIdPostQuery>(), CancellationToken.None), Times.Once);
            mockAzureRedisCacheHelper.Verify(x => x.Get<BlogPostResponse>(It.IsAny<string>()), Times.Never);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            // Add more assertions based on your expected response
        }

        [Fact]
        public async Task GetBlogById_ReturnsOkResultFromRedisCache()
        {
            // Arrange

            var mockDatabase = new Mock<IDatabase>();
            var mockAzureRedisCacheHelper = new Mock<AzureRedisCacheHelper>(mockDatabase);
            var mockMapper = new Mock<IMapper>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);

       
            string sid = "random string";
            var response = new BlogPostResponse
            {
                BlogPostSid = "",
                PostName = "Test",
                PostDescription = "Test",
                BlogImage = ""
            };

            var blogresponse = new BlogPost()
            {
                BlogPostId = 1
            };

            mockDatabase.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None)).ReturnsAsync(It.IsAny<RedisValue>());

            mockDatabase.Setup(x => x.KeyExistsAsync(It.IsAny<RedisKey>(), CommandFlags.None)).ReturnsAsync(true);

            mockAzureRedisCacheHelper.Setup(x => x.IsInCache(sid)).ReturnsAsync(true);


            mockMediator.Setup(m => m.Send(It.IsAny<GetByIdPostQuery>(), CancellationToken.None))
                        .ReturnsAsync(response);


            // Act
            var result = await controller.GetById(sid);

            // Assert
            mockMediator.Verify(x => x.Send(It.IsAny<GetByIdPostQuery>(), CancellationToken.None), Times.Never);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            // Add more assertions based on your expected response
        }

        [Fact]
        public async Task DeletePost_ReturnsOkResult()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            var mockMapper = new Mock<IMapper>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);
            var controllerMock = new Mock<PostsController>();

            string sid = "random string";

            controllerMock.Setup(x => x.CommonDeletedResult()).Returns(new StatusCodeResult(204));
            mockMediator.Setup(x => x.Send(It.IsAny<DeletePostCommand>(), CancellationToken.None));

            // Act
            var result = await controller.DeletePost(sid);

            // Assert
            mockMediator.Verify(x => x.Send(It.IsAny<DeletePostCommand>(), CancellationToken.None), Times.Once);
            //controllerMock.Verify(x => x.CommonDeletedResult(), Times.Once);
            var okResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, okResult.StatusCode);
        }

        [Fact]
        public async Task GetBlogById_ReturnsNotFoundResult()
        {// Arrange

            var mockDatabase = new Mock<IDatabase>();
            var mockAzureRedisCacheHelper = new Mock<AzureRedisCacheHelper>(mockDatabase);
            var mockMapper = new Mock<IMapper>();
            var mockSearchModel = new Mock<SearchRequestModel>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);

            string sid = "sid"; // <- Either add Sid of post which is not present or random characters
            mockAzureRedisCacheHelper.Setup(x => x.IsInCache(It.IsAny<string>())).ReturnsAsync(false);

            mockMediator.Setup(m => m.Send(It.IsAny<GetByIdPostQuery>(), CancellationToken.None)).ThrowsAsync(new HttpStatusCodeException(StatusCodes.Status404NotFound, CommonConstants.PostNotFoundMessage));

            // Act
            var ExceptionResult = await Assert.ThrowsAsync<HttpStatusCodeException>(() => controller.GetById(sid));

            // Assert
            mockMediator.Verify(x => x.Send(It.IsAny<GetByIdPostQuery>(), CancellationToken.None), Times.Once);
            mockAzureRedisCacheHelper.Verify(x => x.Get<BlogPostResponse>(It.IsAny<string>()), Times.Never);

            var notFoundResult = Assert.IsType<HttpStatusCodeException>(ExceptionResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeletePost_ReturnsNotFoundResult()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            var mockMapper = new Mock<IMapper>();

            var mockMediator = new Mock<IMediator>();
            var controller = new PostsController(_configuration, mockDatabase.Object, mockMapper.Object, mockMediator.Object);
            var controllerMock = new Mock<PostsController>();

            string sid = "sid"; // <- Either add Sid of post which is already deleted or random characters

            controllerMock.Setup(x => x.CommonDeletedResult()).Returns(new StatusCodeResult(204));
            mockMediator.Setup(m => m.Send(It.IsAny<DeletePostCommand>(), CancellationToken.None)).ThrowsAsync(new HttpStatusCodeException(StatusCodes.Status404NotFound, CommonConstants.PostNotFoundMessage));

            // Act
            var ExceptionResult = await Assert.ThrowsAsync<HttpStatusCodeException>(() => controller.DeletePost(sid));

            // Assert
            mockMediator.Verify(x => x.Send(It.IsAny<DeletePostCommand>(), CancellationToken.None), Times.Once);

            var notFoundResult = Assert.IsType<HttpStatusCodeException>(ExceptionResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }


    }
}


