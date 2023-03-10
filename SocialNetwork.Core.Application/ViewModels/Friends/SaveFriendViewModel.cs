using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Friends
{
    public class SaveFriendViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "You must type the username")]
        public string Username { get; set; }

        public string? UserId { get; set; }
        public string? FriendId { get; set; }

        public string? CreatedBy{ get; set; }


    }
}
