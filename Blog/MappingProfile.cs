using AutoMapper;
using Blog.Data.Models.Db;
using Blog.ViewModels;

namespace Blog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, User>();
            CreateMap<LoginViewModel, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<PostViewModel, Post>();
            CreateMap<Post, PostViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<TagViewModel, Tag>();
            //CreateMap<List<TagViewModel>, List<Tag>>();
            //CreateMap<List<CommentViewModel>, List<Comment>>();
        }
    }
}
