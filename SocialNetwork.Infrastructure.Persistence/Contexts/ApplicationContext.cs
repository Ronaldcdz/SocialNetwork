using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Domain.Common;
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext (DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach(var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        break;

                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent Api

            #region tables

            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Friend>().ToTable("Friends");

            #endregion


            #region "primary keys"

            modelBuilder.Entity<Post>().HasKey(post=> post.Id);
            modelBuilder.Entity<Comment>().HasKey(comment => comment.Id);
            modelBuilder.Entity<Friend>().HasKey(friend => friend.Id);

            #endregion


            #region relationship


            modelBuilder.Entity<Post>()
                .HasMany(post => post.Comments)
                .WithOne(comment => comment.Post)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Cascade);
                

            #endregion


            #region "property configurations"


            #region posts

            modelBuilder.Entity<Post>()
                .Property(post => post.Description)
                .IsRequired();

            modelBuilder.Entity<Post>()
                .Property(post => post.ImagePath);

            modelBuilder.Entity<Post>()
                .Property(post => post.UserId);

            modelBuilder.Entity<Post>()
                .Property(post => post.ProfilePicture);
            #endregion


            #region Comments
            modelBuilder.Entity<Comment>()
                .Property(comment => comment.Description)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .Property(comment => comment.UserId);

            modelBuilder.Entity<Comment>()
               .Property(comment => comment.ProfilePicture);

            modelBuilder.Entity<Comment>()
               .Property(comment => comment.PostId);
            #endregion



            #region Friends

            modelBuilder.Entity<Friend>()
                .Property(friend => friend.UserId)
                .IsRequired();

            modelBuilder.Entity<Friend>()
                .Property(friend => friend.FriendId);

            #endregion

            #endregion
        }



    }
}
