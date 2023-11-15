using AutoMapper;
using Blog.Data.Models.Db;
using Blog.ViewModels;

namespace Blog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ConstructUsing(v => new UserViewModel(v));
            CreateMap<IEnumerable<User>, UserViewModel>();
            CreateMap<IEnumerable<Post>, PostsViewModel>();
            CreateMap<PostsViewModel, Post>();
            CreateMap<IEnumerable<Comment>, CommentsViewModel>();
            CreateMap<CommentsViewModel, Comment>();
            CreateMap<IEnumerable<Tag>, TagsViewModel>();
            CreateMap<TagsViewModel, Tag>();
        }
    }
}
