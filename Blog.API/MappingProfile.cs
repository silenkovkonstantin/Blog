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
            CreateMap<Tag, TagView>().ReverseMap();
            CreateMap<AddTagRequest, Tag>();
            CreateMap<EditTagRequest, Tag>();
            CreateMap<AddPostRequest, Post>();
            CreateMap<EditPostRequest, Post>();
            CreateMap<RoleView, Role>().ReverseMap();
            CreateMap<Post, PostView>();
        }
    }
}
