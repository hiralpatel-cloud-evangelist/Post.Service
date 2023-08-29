using AutoMapper;
using Post.Service.DTO.FilterDto;
using Post.Service.DTO.Response;
using Post.Service.Models.Tables;

namespace Post.Service.Services.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            ////ContactList
            CreateMap<BlogPost, BlogPostResponse>();
            CreateMap<PagedViewResponse<BlogPost>, PagedViewResponse<BlogPostResponse>>();
            //CreateMap<ContactListEmail, ContactListEmailResponse>();
            //CreateMap<PagedViewResponse<ContactListEmail>, PagedViewResponse<ContactListEmailResponse>>();


        }
    }
}
