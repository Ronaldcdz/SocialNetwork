using SocialNetwork.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        public string? UserId {get; set;}
        
        public string? ProfilePicture { get; set; }


        //Navigation Property
        // public User? User { get; set; }

        public ICollection<Comment>? Comments { get; set; }

    }
}
