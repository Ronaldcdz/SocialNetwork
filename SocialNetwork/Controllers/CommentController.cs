using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Enums;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Comments;

namespace SocialNetwork.Controllers
{
    [Authorize(Roles = "Basic")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userviewModel;
        public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor)
        {
            _commentService = commentService;
            _httpContextAccessor = httpContextAccessor;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int postId, string userName)
        {
            SaveCommentViewModel saveCommentViewModel = new SaveCommentViewModel();
            saveCommentViewModel.PostId = postId;
            saveCommentViewModel.CreatedBy = userName;
            return View("SaveComment", saveCommentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveComment", vm);
            }

            string createdBy = vm.CreatedBy;


            await _commentService.AddComment(vm);

            if (createdBy == userviewModel.Username)
            {
                return RedirectToRoute(new { controller = "Post", action = "Index" });

            }
            else
            {
                return RedirectToRoute(new { controller = "Friend", action = "Index" });
            }


        }

    }
}
