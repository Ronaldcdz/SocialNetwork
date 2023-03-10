using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Posts;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userviewModel;
        private readonly IMapper _mapper;


        public PostService(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userviewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel vm)
        {
            vm.UserId = userviewModel.Id;
            vm.CreatedBy = userviewModel.Username;
            vm.ProfilePicture = userviewModel.ImagePath;
            return await base.Add(vm);
        }

        public override async Task Update(SavePostViewModel vm, int id)
        {
            vm.UserId = userviewModel.Id;
            vm.CreatedBy = userviewModel.Username;
            await base.Update(vm, id);
        }

        public async Task<SavePostViewModel> GetByIdSaveViewModelWithDate(int id, string created)
        {
            SavePostViewModel vm = await base.GetByIdSaveViewModel(id);
            vm.Created = created;
            return vm;

        }


        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var postList = await _postRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            return postList.Where(post => post.UserId == userviewModel.Id).OrderByDescending(x => x.Created).Select(post => new PostViewModel
            {
                Id = post.Id,
                ProfilePicture = post.ProfilePicture,
                CreatedBy = post.CreatedBy,
                Created = post.Created.ToString(),
                Description = post.Description,
                UserId = post.UserId,
                ImagePath = post.ImagePath,
                Comments = post.Comments.ToList(),

            }).ToList();
        }

        public async Task<List<PostViewModel>> GetAllUsersAddedPostsWithInclude(string id)
        {
            var postList = await _postRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            return postList.Where(post => post.UserId == id).OrderByDescending(x => x.Created).Select(post => new PostViewModel
            {
                Id = post.Id,
                ProfilePicture = post.ProfilePicture,
                CreatedBy = post.CreatedBy,
                Created = post.Created.ToString(),
                Description = post.Description,
                UserId = post.UserId,
                ImagePath = post.ImagePath,
                Comments = post.Comments.ToList(),

            }).ToList();
        }

    }
}
