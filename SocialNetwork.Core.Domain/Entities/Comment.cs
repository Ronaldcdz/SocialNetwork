using SocialNetwork.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Description { get; set; }

        public string? UserId { get; set; }     // User who did the coment

        public string? ProfilePicture { get; set; }
        public int? PostId { get; set; }


        //Navigation Property

        public Post? Post { get; set; }
    }
}
