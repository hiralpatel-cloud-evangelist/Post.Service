using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Service.Base.HelperClasses;
using Post.Service.DTO.Constants;
using Post.Service.DTO.FilterDto;
using Post.Service.DTO.Response;
using Post.Service.Models.Tables;
using Post.Service.Services.Extensions;
using System.Linq.Expressions;

namespace Post.Service.Services.CQRS.Queries
{

    // This class represents a query to retrieve a list of blog posts with filters and pagination.
    public class GetPostQuery : IRequest<BlogPostListResponse>
    {
        public SearchRequestModel Filters { get; set; } // Search filters and pagination parameters

        // Constructor to initialize the query with the provided search filters and pagination parameters
        public GetPostQuery(SearchRequestModel filters)
        {
            Filters = filters;
        }

        // Handler class responsible for processing the GetPostQuery.
        public class GetPostQueryHandler : IRequestHandler<GetPostQuery, BlogPostListResponse>
        {
            private readonly IMapper _mapper; // Object mapper
            private readonly PostBlogsContext _dbContext; // Database context for interacting with the database

            // Constructor to inject necessary dependencies
            public GetPostQueryHandler(IMapper mapper, PostBlogsContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            // Method to handle the retrieval of a list of blog posts with filters and pagination
            public async Task<BlogPostListResponse> Handle(GetPostQuery query, CancellationToken cancellationToken)
            {
                // Initialize ordering expression
                Expression<Func<BlogPost, dynamic>> TOrderBy = null;

                // Determine ordering expression based on provided sorting column
                TOrderBy = !string.IsNullOrEmpty(query.Filters.SortColumn)
                    ? FilterExtensions.GetDynamicQueryForBlogPosts(query.Filters.SortColumn)
                    : d => d.LastModifiedByUserId;

                // Initialize status filter criteria
                Expression<Func<BlogPost, bool>> criteria = p => p.Status != 3;

                Expression<Func<BlogPost, bool>> allSearchCriteria = null;

                // Combine search criteria for title and technique using OR operation
                if (!string.IsNullOrEmpty(query.Filters.SearchText))
                {
                    var title = FilterExtensions.GetDynamicQueryForBlogPosts(CommonConstants.PostName, query.Filters.SearchText);
                    var technique = FilterExtensions.GetDynamicQueryForBlogPosts(CommonConstants.PostDescription, query.Filters.SearchText);
                    allSearchCriteria = ExpressionHelper.CombineOR(allSearchCriteria, title);
                    allSearchCriteria = ExpressionHelper.CombineOR(allSearchCriteria, technique);

                    criteria = ExpressionHelper.CombineAnd(criteria, allSearchCriteria);
                }

                IQueryable<BlogPost> queryableRecords = null;

                if (!string.IsNullOrEmpty(query.Filters.SearchText))
                {
                    queryableRecords = _dbContext.BlogPosts.Where(criteria).AsNoTracking();
                }
                else
                {
                    queryableRecords = _dbContext.BlogPosts.Where(a => a.Status != 3).AsNoTracking();
                }

                // Apply sorting based on sort order
                if (query.Filters.SortOrder == CommonConstants.Desc)
                    queryableRecords = queryableRecords.OrderByDescending(TOrderBy);
                else
                    queryableRecords = queryableRecords.OrderBy(TOrderBy);

                // Initialize a PagedViewResponse to hold paginated data
                var postList = new PagedViewResponse<BlogPost>();

                // Populate the PagedViewResponse using pagination utility
                await Pagination<BlogPost>.Data(query.Filters.PageSize, query.Filters.Page, postList, queryableRecords);

                // Map the paginated response into a list of BlogPostResponse
                var blogpostList = _mapper.Map<PagedViewResponse<BlogPostResponse>>(postList);

                // Create and return BlogPostListResponse containing the paginated blog post list
                return new BlogPostListResponse
                {
                    BlogPostList = blogpostList,
                };
            }
        }
    }

}
