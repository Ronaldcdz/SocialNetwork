using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Friends;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialNetwork.Core.Application.Services
{
    public class FriendService : GenericService<SaveFriendViewModel, FriendViewModel, Friend>, IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userviewModel;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPostService _postService;


        public FriendService(IFriendRepository friendRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserService userService, IPostService postService) : base(friendRepository, mapper)
        {
            _friendRepository = friendRepository;
            _httpContextAccessor = httpContextAccessor;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _mapper = mapper;
            _userService = userService;
            _postService = postService;
        }


        public override async Task<SaveFriendViewModel> Add(SaveFriendViewModel vm)
        {
            vm.UserId = userviewModel.Id;

            var userFound = await _userService.GetByUsernameBasicUserAsync(vm.Username);

            if(userFound != null && userFound.Username != userviewModel.Username)
            {

                vm.FriendId = userFound.Id;
                vm.CreatedBy = userviewModel.Username;

                if(await UserAdded(vm.UserId, vm.FriendId))
                {
                    return null;

                }


                return await base.Add(vm);

            }


            else
            {
                return null;
            }

        }

        public async Task<bool> UserAdded(string userId, string friendId)
        {
            var friendList = await base.GetAllViewModel();
            bool result = new bool();

            foreach (var item in friendList)
            {
                if (item.UserId == userId && item.FriendId == friendId)
                {
                    result = true;
                }

                else
                {
                    result = false;
                }
            }

            return result;

        }

       public async Task <List<FriendViewModel>> GetAllFriendsAdded ()
        {
            var friendList = await base.GetAllViewModel();

            List<FriendViewModel> result = new List<FriendViewModel>();



            friendList = friendList.Where(friend => friend.UserId == userviewModel.Id).ToList();

            if(friendList != null)
            {
                UserFriendViewModel userFound = new UserFriendViewModel();
                FriendViewModel friendFound = new FriendViewModel();

                foreach (var item in friendList)
                {
                    userFound = await _userService.GetByIdBasicUserAsync(item.FriendId);
                    friendFound = _mapper.Map<FriendViewModel>(userFound);
                    friendFound.CreatedBy = userviewModel.Username;
                    friendFound.FriendId = userFound.Id;
                    friendFound.UserId = userviewModel.Id;
                    friendFound.Id = item.Id;
                    result.Add(friendFound);
                }

            }

            else
            {
                FriendViewModel friendNotFound = new FriendViewModel();
                friendNotFound.Description = "";
                result.Add(friendNotFound);
            }

            return result;


        }

        public async Task<List<FriendViewModel>> GetAllFriendsPostsAdded()
        {
            var friendList = await base.GetAllViewModel();

            List<FriendViewModel> resultFriends = new List<FriendViewModel>();
            List<FriendViewModel> resultPosts = new List<FriendViewModel>();



            friendList = friendList.Where(friend => friend.UserId == userviewModel.Id).ToList();

            if (friendList != null)
            {
                UserFriendViewModel userFound = new UserFriendViewModel();
                FriendViewModel friendFound = new FriendViewModel();

                foreach (var item in friendList)
                {
                    userFound = await _userService.GetByIdBasicUserAsync(item.FriendId);
                    friendFound = _mapper.Map<FriendViewModel>(userFound);
                    friendFound.CreatedBy = userFound.Username;
                    friendFound.FriendId = userFound.Id;   
                    resultFriends.Add(friendFound);

                    var postsFriendFound = await _postService.GetAllUsersAddedPostsWithInclude(friendFound.FriendId);
                    var postsMapped = _mapper.Map<List<FriendViewModel>>(postsFriendFound);

                    foreach (var item2 in postsMapped)
                    {
                        resultPosts.Add(item2);

                    }

                    //resultPosts.Add(postsMapped);
                }

            }

            else
            {
                return null;
            }

            return resultPosts;

        }

        public async Task DeleteFriend(SaveFriendViewModel vm)
        {
            var friendsAdded = await GetAllFriendsAdded();

            foreach (var item in friendsAdded)
            {
                if (item.UserId == vm.UserId && item.FriendId == vm.FriendId && item.CreatedBy == vm.CreatedBy)
                {
                    await base.Delete(item.Id);
                }
            }
        }


    }
}
