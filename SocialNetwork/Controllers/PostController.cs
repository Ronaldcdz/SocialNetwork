using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Posts;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Models;
using System.Diagnostics;

namespace SocialNetwork.Controllers
{
    [Authorize (Roles = "Basic")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _postService.GetAllViewModelWithInclude());
        }

        public async Task<IActionResult> Create()
        {
            return View("SavePost", new SavePostViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            SavePostViewModel savePostViewModel = await _postService.Add(vm);

            if (vm.File != null)
            {
                if(savePostViewModel.Id != 0 && savePostViewModel != null)
                {
                    savePostViewModel.ImagePath = UploadImagesHelper.UploadPostImage(vm.File, savePostViewModel.Id);
                    await _postService.Update(savePostViewModel, savePostViewModel.Id);
                }
            }


            return RedirectToRoute(new { controller = "Post", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id, string created)
        {
            SavePostViewModel savePostViewModel = await _postService.GetByIdSaveViewModelWithDate(id, created);
            return View("SavePost", savePostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePostViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            SavePostViewModel savePostViewModel = await _postService.GetByIdSaveViewModelWithDate(vm.Id, vm.Created);
            
            if (vm.File != null)
            {
                if (savePostViewModel.ImagePath == null || savePostViewModel.ImagePath == "")
                {
                    savePostViewModel.ImagePath = UploadImagesHelper.UploadPostImage(vm.File, savePostViewModel.Id);
                    await _postService.Update(savePostViewModel, savePostViewModel.Id);
                }

                else if (savePostViewModel.ImagePath != null && savePostViewModel.ImagePath != "")
                {
                    savePostViewModel.ImagePath = UploadImagesHelper.UploadPostImage(vm.File, savePostViewModel.Id, true, savePostViewModel.ImagePath);
                    await _postService.Update(savePostViewModel, savePostViewModel.Id);
                }
            }
            else
            {
                await _postService.Update(savePostViewModel, vm.Id);
            }


            return RedirectToRoute(new { controller = "Post", action = "Index" });
        }


        public async Task<IActionResult> Delete(int id)
        {
            return View(await _postService.GetByIdSaveViewModel(id));
        }


        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.Delete(id);

            string basePath = $"/Images/Posts/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { controller = "Post", action = "Index" });
        }
    }

}