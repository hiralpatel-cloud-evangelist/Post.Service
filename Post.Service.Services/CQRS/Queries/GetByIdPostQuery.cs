using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Post.Service.Base.BaseResponse;
using Post.Service.DTO.Constants;
using Post.Service.DTO.Response;
using Post.Service.Models.Tables;

namespace Post.Service.Services.CQRS.Queries
{

    // This class represents a query to retrieve a blog post by its identifier.
    public class GetByIdPostQuery : IRequest<BlogPostResponse>
    {
        public string Sid { get; set; } // Identifier of the requested blog post

        // Constructor to initialize the query with the provided identifier
        public GetByIdPostQuery(string _sid)
        {
            Sid = _sid;
        }

        // Handler class responsible for processing the GetByIdPostQuery.
        public class GetProductByIdQueryHandler : IRequestHandler<GetByIdPostQuery, BlogPostResponse>
        {
            private readonly IMapper _mapper; // Object mapper
            private readonly PostBlogsContext _dbContext; // Database context for interacting with the database

            // Constructor to inject necessary dependencies
            public GetProductByIdQueryHandler(IMapper mapper, PostBlogsContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            // Method to handle the retrieval of a blog post by its identifier
            public async Task<BlogPostResponse> Handle(GetByIdPostQuery query, CancellationToken cancellationToken)
            {
                // Attempt to retrieve the blog post by its SID from the repository.
                var blogDetails = _dbContext.BlogPosts.Where(x => x.BlogPostSid == query.Sid && x.Status != (int)Status.Delete).FirstOrDefault();

                // Check if the blog post was not found.
                if (blogDetails == null)
                {
                    // Throw an exception indicating that the post was not found
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound, CommonConstants.PostNotFoundMessage);
                }

                // Map data entity into response model
                var blogPostDetails = _mapper.Map<BlogPostResponse>(blogDetails);

                // Return the retrieved blog post in a BlogPostResponse
                return blogPostDetails;
            }
        }
    }

}
