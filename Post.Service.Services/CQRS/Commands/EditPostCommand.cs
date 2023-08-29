using AutoMapper;
using Azure.Storage.Blobs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Post.Service.Base.BaseResponse;
using Post.Service.DTO.Constants;
using Post.Service.DTO.Request;
using Post.Service.DTO.Response;
using Post.Service.Models.Tables;

namespace Post.Service.CQRS.Commands
{
    // This class represents a command to edit an existing blog post.
    public class EditPostCommand : IRequest<BlogPostResponse>
    {
        public BlogPostRequestModel RequestModel { get; set; } // The request model containing updated blog post data
        public string Sid { get; set; } // Identifier for the post to be edited
        public BlobMetadataModel blobMetadataModel { get; set; } // Metadata for Blob storage

        // Constructor to initialize the command with the request model, post identifier, and Blob metadata
        public EditPostCommand(BlogPostRequestModel requestModel, string sid, BlobMetadataModel blobMetadataModel)
        {
            RequestModel = requestModel;
            Sid = sid;
            this.blobMetadataModel = blobMetadataModel;
        }

        // Handler class responsible for processing the EditPostCommand.
        public class EditPostCommandHandler : IRequestHandler<EditPostCommand, BlogPostResponse>
        {
            private readonly IMapper _mapper; // Object mapper
            private readonly PostBlogsContext _dbContext; // Database context for interacting with the database

            // Constructor to inject necessary dependencies
            public EditPostCommandHandler(IMapper mapper, PostBlogsContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            // Method to handle the editing of an existing blog post
            public async Task<BlogPostResponse> Handle(EditPostCommand model, CancellationToken cancellationToken)
            {
                // Retrieve the blog post from the repository
                BlogPost? post = _dbContext.BlogPosts.FirstOrDefault(x => x.BlogPostSid == model.Sid && x.Status != (int)Status.Delete);

                if (post == null)
                {
                    // Throw an exception indicating that the post was not found
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound, CommonConstants.PostNotFoundMessage);
                }

                // Update the blog post properties
                post.PostName = model.RequestModel.PostName;
                post.PostDescription = model.RequestModel.PostDescription;
                post.LastModifiedDatetime = DateTime.UtcNow;

                // Upload file to blob storage if provided
                if (model.RequestModel.File != null)
                {
                    string fileUrl = await BlobFileUpload(model.RequestModel.File, model.blobMetadataModel);
                    post.BlogImage = fileUrl;
                }

                // Save changes to the database
                _dbContext.SaveChanges();

                // Map data entity into response model
                var blogPostDetails = _mapper.Map<BlogPostResponse>(post);

                // Return the updated blog post response
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
