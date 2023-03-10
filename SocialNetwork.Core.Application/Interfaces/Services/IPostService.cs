using SocialNetwork.Core.Application.ViewModels.Posts;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();

        Task<SavePostViewModel> GetByIdSaveViewModelWithDate(int id, string created);
        
        Task<List<PostViewModel>> GetAllUsersAddedPostsWithInclude(string id);


    }
}
