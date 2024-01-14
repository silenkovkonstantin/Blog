using AutoMapper;
using Blog.Data.Models.Db;
using BlogAPI.Contracts.Models;
using BlogAPI.Contracts.Models.Posts;
using BlogAPI.Contracts.Models.Roles;
using BlogAPI.Contracts.Models.Tags;
using BlogAPI.Contracts.Models.Users;
using static BlogAPI.Contracts.Models.Tags.GetTagsResponse;

namespace BlogAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserView>().ReverseMap();
            CreateMap<AddUserRequest, User>();
            CreateMap<Tag, TagView>();
            CreateMap<AddTagRequest, Tag>();
            CreateMap<EditTagRequest, Tag>();
            CreateMap<AddPostRequest, Post>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Where(t => t.IsChecked == true)));
            CreateMap<EditPostRequest, Post>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Where(t => t.IsChecked == true)));
            CreateMap<RoleView, Role>();
            CreateMap<Post, PostView>();
        }
    }
}
