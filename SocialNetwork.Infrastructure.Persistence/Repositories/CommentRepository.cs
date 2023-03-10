using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _dbContext;

        public CommentRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddComment(Comment entity)
        {
            await _dbContext.Set<Comment>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

        }
    }
}
