using AutoMapper;
using Azure.Storage.Blobs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Post.Service.DTO.Constants;
using Post.Service.DTO.Request;
using Post.Service.DTO.Response;
using Post.Service.Models.Tables;

namespace Post.Service.CQRS.Commands
{
    // This class represents a command to create a new blog post.
    public class CreatePostCommand : IRequest<BlogPostResponse>
    {
        public BlogPostRequestModel Model { get; set; } // The request model containing blog post data
        public BlobMetadataModel blobMetadataModel { get; set; } // Metadata for Blob storage

        // Constructor to initialize the command with the request model and Blob metadata
        public CreatePostCommand(BlogPostRequestModel requestModel, BlobMetadataModel blobMetadataModel)
        {
            Model = requestModel;
            this.blobMetadataModel = blobMetadataModel;
        }

        // Handler class responsible for processing the CreatePostCommand.
        public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, BlogPostResponse>
        {
            private readonly IConfiguration _configuration; // Application configuration
            private readonly IMapper _mapper; // Object mapper
            private readonly PostBlogsContext _dbContext; // Database context for interacting with the database

            // Constructor to inject necessary dependencies
            public CreatePostCommandHandler(IConfiguration configuration, IMapper mapper, PostBlogsContext dbContext)
            {
                _configuration = configuration;
                _mapper = mapper;
                _dbContext = dbContext;
            }

            // Method to handle the creation of a new blog post
            public async Task<BlogPostResponse> Handle(CreatePostCommand model, CancellationToken cancellationToken)
            {
                // Create a new BlogPost object with the provided data
                BlogPost blogPost = new BlogPost
                {
                    BlogPostSid = Guid.NewGuid().ToString(),
                    PostName = model.Model.PostName,
                    PostDescription = model.Model.PostDescription,
                    Status = (int)Status.Active,
                    CreatedDatetime = DateTime.UtcNow,
                    LastModifiedDatetime = DateTime.UtcNow
                };

                // Upload file to blob storage if provided
                if (model.Model.File != null)
                {
                    string fileUrl = await BlobFileUpload(model.Model.File, model.blobMetadataModel);
                    blogPost.BlogImage = fileUrl;
                }

                // Add the blog post to the repository
                _dbContext.BlogPosts.Add(blogPost);
                _dbContext.SaveChanges();

                // Map data entity into response model
                var blogPostDetails = _mapper.Map<BlogPostResponse>(blogPost);

                // Return the newly created blog post response
                return blogPostDetails;
            }

            // Private method to upload a file to Blob storage
            private async Task<string> BlobFileUpload(IFormFile file, BlobMetadataModel blobData)
            {
                string connectionString = blobData.BlobConnectionString;
                string containerName = blobData.BlobContainer;

                // Create BlobServiceClient and BlobContainerClient
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Generate a unique blob name
                string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload the file stream to blob storage
                using (Stream stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                return blobClient.Uri.ToString();
            }
        }
    }

}
