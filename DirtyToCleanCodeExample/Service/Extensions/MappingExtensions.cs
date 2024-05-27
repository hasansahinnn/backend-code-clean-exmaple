using AutoMapper;
using Data.Models;
using Service.DTOs;

namespace Service.Extensions;

public class MappingExtensions : Profile
{
    public MappingExtensions()
    {
        #region Book Mapping Extensions

        #endregion

        #region BorrowRecord Mapping Extensions

        #endregion

        #region Comment Mapping Extensions

        CreateMap<Comment, CreateCommentDto>().ReverseMap();

        #endregion

        #region Member Mapping Extensions

        #endregion

        #region Post Mapping Extensions

        CreateMap<Post, CreatePostDto>().ReverseMap();
        CreateMap<Post, PostsListDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(u => u.Id))
            .ForMember(x => x.UserId, opt => opt.MapFrom(u => u.UserId))
            .ForMember(x => x.Content, opt => opt.MapFrom(u => u.Content))
            .ForMember(x => x.Status, opt => opt.MapFrom(u => u.Status))
            .ForMember(x => x.CreatedDate, opt => opt.MapFrom(u => u.CreatedDate))
            .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(u => u.UpdatedDate))
            .ForMember(x => x.UserDto, opt => opt.MapFrom(u =>
                u.User != null ?
                    new UserDto(u.User.Id, u.User.Username, u.User.Role, u.User.PostCount, u.User.Email, u.User.UpdatedDate) :
                    null
            )).ReverseMap();

        #endregion

        #region User Mapping Extensions

        #endregion
    }
}