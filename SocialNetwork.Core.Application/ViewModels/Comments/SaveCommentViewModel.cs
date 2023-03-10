using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Comments
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        public string? ProfilePicture { get; set; }


        [Required(ErrorMessage = "You must type the comment")]
        public string Description { get; set; }

        public string? UserId { get; set; }     // User who did the coment

        public string? CreatedBy { get; set; }

        public string? Created { get; set; }
        public int? PostId { get; set; }


    }
}
