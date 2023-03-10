using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Posts
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must type a description")]
        public string Description { get; set; }


        public string? ImagePath { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public string? UserId { get; set; }

        public string? CreatedBy { get; set; }
        public string? Created { get; set; }

        public string? ProfilePicture { get; set; }



    }
}
