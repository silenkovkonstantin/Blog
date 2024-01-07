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
            CreateMap<UserEditViewModel, User>().ReverseMap();
            CreateMap<PostViewModel, Post>()
                .ForMember(x => x.Tags, opt => opt.MapFrom(src => src.Tags.Where(t => t.IsChecked == true)));
            CreateMap<Post, PostViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<TagViewModel, Tag>();
            CreateMap<Tag, TagViewModel>();
            CreateMap<RoleViewModel, Role>().ReverseMap();
            CreateMap<CreateRoleViewModel, Role>();
        }
    }
}
