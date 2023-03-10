using SocialNetwork.Core.Application.ViewModels.Friends;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendService : IGenericService<SaveFriendViewModel, FriendViewModel, Friend>
    {
        Task<bool> UserAdded(string userId, string friendId);

        Task<List<FriendViewModel>> GetAllFriendsAdded();

        Task<List<FriendViewModel>> GetAllFriendsPostsAdded();

        Task DeleteFriend(SaveFriendViewModel vm);


    }
}
