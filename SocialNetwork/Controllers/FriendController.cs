using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Enums;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Friends;

namespace SocialNetwork.Controllers
{
    [Authorize(Roles = "Basic")]
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;
        private readonly IUserService _userService;

        public FriendController(IFriendService friendService, IUserService userService)
        {
            _friendService = friendService;
            _userService = userService;
        }



        public async Task<IActionResult> Index()
        {
            ViewBag.FriendsAdded = await _friendService.GetAllFriendsAdded();
            return View(await _friendService.GetAllFriendsPostsAdded());
        }

        public async Task<IActionResult> Create()
        {
            return View("SaveFriend", new SaveFriendViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveFriendViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveFriend", vm);
            }

            if(await _friendService.Add(vm) != null)
            {
                return RedirectToRoute(new { controller = "Friend", action = "Index" });

            }

            else
            {
                ViewBag.UsernameError = "User not found or User added";
                return View("SaveFriend", vm);
            }

        }

        public async Task<IActionResult> Delete(string userId, string friendId, string createdBy, int id, string userName)
        {
            var friend = await _friendService.GetByIdSaveViewModel(id);
            friend.UserId = userId;
            friend.FriendId = friendId;
            friend.CreatedBy = createdBy;
            friend.Username = userName;
            return View(friend);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string userId, string friendId, string createdBy, int id, string userName)
        {
            var friend = await _friendService.GetByIdSaveViewModel(id);
            friend.UserId = userId;
            friend.FriendId = friendId;
            friend.CreatedBy = createdBy;
            friend.Username = userName;

            await _friendService.DeleteFriend(friend);

            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }
    }
}
