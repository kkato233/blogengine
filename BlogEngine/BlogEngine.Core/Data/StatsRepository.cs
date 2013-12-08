using BlogEngine.Core.Data.Contracts;
using BlogEngine.Core.Data.Models;
using System.Linq;

namespace BlogEngine.Core.Data
{
    /// <summary>
    /// Statistics
    /// </summary>
    public class StatsRepository : IStatsRepository
    {
        /// <summary>
        /// Get stats info
        /// </summary>
        /// <returns>Stats counters</returns>
        public Stats Get()
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            var stats = new Stats();

            stats.PublishedPostsCount = Post.Posts.Where(p => p.IsPublished == true && p.IsDeleted == false).Count();
            stats.DraftPostsCount = Post.Posts.Where(p => p.IsPublished == false && p.IsDeleted == false).Count();
            stats.PublishedPagesCount = Page.Pages.Where(p => p.IsPublished == true && p.IsDeleted == false).Count();
            stats.DraftPagesCount = Page.Pages.Where(p => p.IsPublished == false && p.IsDeleted == false).Count();

            CountComments(stats);

            stats.CategoriesCount = Category.Categories.Count;
            stats.TagsCount = 2;
            stats.UsersCount = 3;

            return stats;
        }

        void CountComments(Stats stats)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.AccessAdminPages))
                throw new System.UnauthorizedAccessException();

            foreach (var post in Post.Posts)
            {
                stats.PublishedCommentsCount += post.Comments.Where(c => c.IsPublished == true && c.IsDeleted == false).Count();
                stats.UnapprovedCommentsCount += post.Comments.Where(c => c.IsPublished == false && c.IsSpam == false && c.IsDeleted == false).Count();
                stats.SpamCommentsCount += post.Comments.Where(c => c.IsPublished == false && c.IsSpam == true && c.IsDeleted == false).Count();
            }
        }
    }
}
