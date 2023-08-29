//using Auth_Service.CustomFilters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Post.Service.CQRS.Commands;
using Post.Service.DTO.Response;
using Post.Service.Helper;
using Post.Service.Models.Tables;
using Post.Service.Services.CQRS.Queries;
using Post.Service.Services.Extensions;
using StackExchange.Redis;
using System.Reflection;
using static Post.Service.CQRS.Commands.CreatePostCommand;
using static Post.Service.CQRS.Commands.DeletePostCommand;
using static Post.Service.CQRS.Commands.EditPostCommand;
using static Post.Service.Services.CQRS.Queries.GetByIdPostQuery;
using static Post.Service.Services.CQRS.Queries.GetPostQuery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml;
using Microsoft.OpenApi.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var radisconnectionString = builder.Configuration.GetSection("RadisCacheConnectionString");

var connectionMultiplexer = ConnectionMultiplexer.Connect(radisconnectionString?.Value);
var radisDatabase = connectionMultiplexer.GetDatabase();

builder.Services.AddScoped<IDatabase>(_ => radisDatabase);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("Read", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement("Post.Read"));
    });

    config.AddPolicy("Write", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement("Post.Write"));
    });

    config.AddPolicy("Update", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement("Post.Update"));
    });

    config.AddPolicy("Delete", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement("Post.Delete"));
    });


});


builder.Services.AddScoped<IAuthorizationHandler, ScopeAuthorizationHandler>();

//builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
////builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
////builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
////builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
///

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();


#region For Database Connection
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<PostBlogsContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);



}
catch (Exception ex)
{
    //logger.Information("serilog added" + ex.Message);
    throw;
}

#endregion
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IRequestHandler<CreatePostCommand, BlogPostResponse>, CreatePostCommandHandler>();
builder.Services.AddScoped<IRequestHandler<EditPostCommand, BlogPostResponse>, EditPostCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetByIdPostQuery, BlogPostResponse>, GetProductByIdQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetPostQuery, BlogPostListResponse>, GetPostQueryHandler>();
builder.Services.AddScoped<IRequestHandler<DeletePostCommand>, DeletePostCommandHandler>();


builder.Services.AddBusinessContexts();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Post Service Microservices",
            Version = "v1"
        }
     );
    c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyApi.xml");
      
    c.IncludeXmlComments(filePath);

});

builder.Services.AddSwaggerGenNewtonsoftSupport();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Post.Service API V1");
});

//}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpStatusCodeExceptionMiddleware();
app.UseIpRateLimiting();
app.Run();
