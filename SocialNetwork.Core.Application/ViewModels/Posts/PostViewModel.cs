using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Posts
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string? ProfilePicture { get; set; }
        public string? CreatedBy { get; set; }
        public string? Created { get; set; }
        public string Description { get; set; }

        public string? UserId { get; set; }

        public string? ImagePath { get; set; }

        public List<Comment>? Comments { get; set; }

    }
}
