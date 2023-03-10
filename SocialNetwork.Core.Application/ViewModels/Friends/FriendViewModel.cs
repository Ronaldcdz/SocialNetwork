using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Friends
{
    public class FriendViewModel
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }



        public string? ProfilePicture { get; set; }
        public string? CreatedBy { get; set; }
        public string? Created { get; set; }
        public string Description { get; set; }

        public string? UserId { get; set; }
        public string? FriendId { get; set; }
        public string? Username { get; set; }

        public string? ImagePath { get; set; }

        public List<Comment>? Comments { get; set; }
    }
}
