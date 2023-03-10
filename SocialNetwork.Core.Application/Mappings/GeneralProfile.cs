using AutoMapper;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.ViewModels.Comments;
using SocialNetwork.Core.Application.ViewModels.Friends;
using SocialNetwork.Core.Application.ViewModels.Posts;
using SocialNetwork.Core.Application.ViewModels.Users;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {

        public GeneralProfile()
        {

            #region UserProfile
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.File, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
               .ForMember(x => x.HasError, opt => opt.Ignore())
               .ForMember(x => x.Error, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UserFriendViewModel, UserInfo>()
                .ReverseMap();
            #endregion


            #region Post

            CreateMap<Post, PostViewModel>()
                .ForMember(x => x.Comments, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.Comments, opt => opt.Ignore());


            CreateMap<Post, SavePostViewModel>()
              .ForMember(x => x.File, opt => opt.Ignore())
              .ForMember(x => x.Created, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.Comments, opt => opt.Ignore())
              .ForMember(x => x.LastModified, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());



            #endregion


            #region Comments

            CreateMap<Comment, SaveCommentViewModel>()
              .ForMember(x => x.Id, opt => opt.Ignore())
               .ReverseMap()
              .ForMember(x => x.Post, opt => opt.Ignore());


            #endregion


            #region Friends

            CreateMap<Friend, SaveFriendViewModel>()
              .ForMember(x => x.Username, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.Created, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<Friend, FriendViewModel>()
              .ForMember(x => x.ProfilePicture, opt => opt.Ignore())
              .ForMember(x => x.ImagePath, opt => opt.Ignore())
              .ForMember(x => x.Comments, opt => opt.Ignore())
              .ForMember(x => x.Created, opt => opt.Ignore())
              .ForMember(x => x.Description, opt => opt.Ignore())
              .ForMember(x => x.FirstName, opt => opt.Ignore())
              .ForMember(x => x.LastName, opt => opt.Ignore())
              .ForMember(x => x.Username, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.Id, opt => opt.Ignore())
              .ForMember(x => x.Created, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<UserFriendViewModel, SaveFriendViewModel>()
              .ForMember(x => x.UserId, opt => opt.Ignore())
              .ForMember(x => x.FriendId, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.FirstName, opt => opt.Ignore())
              .ForMember(x => x.LastName, opt => opt.Ignore());


            CreateMap<FriendViewModel, UserFriendViewModel>()
              .ForMember(x => x.Id, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.Id, opt => opt.Ignore())
              .ForMember(x => x.ProfilePicture, opt => opt.Ignore())
              .ForMember(x => x.CreatedBy, opt => opt.Ignore())
              .ForMember(x => x.Description, opt => opt.Ignore())
              .ForMember(x => x.UserId, opt => opt.Ignore())
              .ForMember(x => x.FriendId, opt => opt.Ignore())
              .ForMember(x => x.ImagePath, opt => opt.Ignore())
              .ForMember(x => x.Comments, opt => opt.Ignore());

            CreateMap<FriendViewModel, PostViewModel>()
                .ReverseMap()
                .ForMember(x => x.FirstName, opt => opt.Ignore())
                .ForMember(x => x.LastName, opt => opt.Ignore())
                .ForMember(x => x.FriendId, opt => opt.Ignore());


            #endregion
        }
    }
}
