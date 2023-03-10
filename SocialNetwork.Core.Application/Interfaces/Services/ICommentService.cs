﻿using SocialNetwork.Core.Application.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task AddComment(SaveCommentViewModel comment);
    }
}
