using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Post.Service.Base.BaseResponse;
using Post.Service.DTO.Constants;
using Post.Service.Models.Tables;
using System.Security.Cryptography;

namespace Post.Service.CQRS.Commands
{
    // This class represents a command to delete a blog post.
    public class DeletePostCommand : IRequest
    {
        public string Sid { get; set; } // Identifier for the post to be deleted


        // Constructor to initialize the Sid property
        public DeletePostCommand(string sid)
        {
            Sid = sid;
        }

        // Handler class responsible for processing the DeletePostCommand.
        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
        {
            private readonly PostBlogsContext _dbContext; // Database context for interacting with the database

            // Constructor to inject the database context
            public DeletePostCommandHandler(PostBlogsContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task Handle(DeletePostCommand model, CancellationToken cancellationToken)
            {
                // Retrieve the blog post from the repository
                BlogPost? post = _dbContext.BlogPosts.FirstOrDefault(x => x.BlogPostSid == model.Sid && x.Status != (int)Status.Delete);
                // Check if the post was not found
                if (post == null)
                {
                    // Throw an exception indicating that the post was not found
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound, CommonConstants.PostNotFoundMessage);

                }
                // Soft delete the blog post by updating its status and last modified timestamp
                post.Status = (int)Status.Delete;
                post.LastModifiedDatetime = DateTime.UtcNow;
                // Save the changes to the database
                _dbContext.SaveChanges();

            }
        }
    }
}
