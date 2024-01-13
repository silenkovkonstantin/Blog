using AutoMapper;
using Blog.Data.Models.Db;
using Blog.ViewModels;
using BlogAPI.Contracts.Models;
using BlogAPI.Contracts.Models.Tags;
using BlogAPI.Contracts.Models.Users;
using static BlogAPI.Contracts.Models.Tags.GetTagsRequest;

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

            CreateMap<PostViewModel, Post>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Where(t => t.IsChecked == true)));
            CreateMap<Post, PostViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
        }
    }
}
