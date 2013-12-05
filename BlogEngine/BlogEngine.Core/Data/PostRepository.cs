﻿using BlogEngine.Core.Data.Contracts;
using BlogEngine.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;

namespace BlogEngine.Core.Data
{
    /// <summary>
    /// Post repository
    /// </summary>
    public class PostRepository : IPostRepository
    {
        /// <summary>
        /// Post list
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="order">Order expression</param>
        /// <param name="skip">Records to skip</param>
        /// <param name="take">Records to take</param>
        /// <returns>List of posts</returns>
        public IEnumerable<PostItem> Find(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.ViewPublicPosts))
                throw new System.UnauthorizedAccessException();

            if (take == 0) take = Post.ApplicablePosts.Count;
            if (string.IsNullOrEmpty(filter)) filter = "1==1";
            if (string.IsNullOrEmpty(order)) order = "DateCreated desc";

            var posts = new List<PostItem>();
            var query = Post.ApplicablePosts.AsQueryable().Where(filter);

            foreach (var item in query.OrderBy(order).Skip(skip).Take(take))
                posts.Add(ToJson((BlogEngine.Core.Post)item));

            return posts;
        }

        /// <summary>
        /// Get single post
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Post object</returns>
        public PostDetail FindById(Guid id)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.ViewPublicPosts))
                throw new System.UnauthorizedAccessException();
            try
            {
                return ToJsonDetail((from p in Post.Posts.ToList() where p.Id == id select p).FirstOrDefault());
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Add new post
        /// </summary>
        /// <param name="detail">Post</param>
        /// <returns>Saved post with new ID</returns>
        public PostDetail Add(PostDetail detail)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.CreateNewPosts))
                throw new System.UnauthorizedAccessException();

            var post = new Post();
            Save(post, detail);
            return ToJsonDetail(post);
        }

        /// <summary>
        /// Update post
        /// </summary>
        /// <param name="detail">Post to update</param>
        /// <returns>True on success</returns>
        public bool Update(PostDetail detail, string action)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.EditOwnPosts))
                throw new System.UnauthorizedAccessException();

            var post = (from p in Post.Posts.ToList() where p.Id == detail.Id select p).FirstOrDefault();

            if (post != null)
            {
                if (action == "publish")
                {
                    post.IsPublished = true;
                    post.DateModified = DateTime.Now;
                    post.Save();
                }
                else if (action == "unpublish")
                {
                    post.IsPublished = false;
                    post.DateModified = DateTime.Now;
                    post.Save();
                }
                else
                {
                    Save(post, detail);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete post
        /// </summary>
        /// <param name="id">Post ID</param>
        /// <returns>True on success</returns>
        public bool Remove(Guid id)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.DeleteOwnPosts))
                throw new System.UnauthorizedAccessException();
            try
            {
                var post = (from p in Post.Posts.ToList() where p.Id == id select p).FirstOrDefault();
                post.Delete();
                post.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Private Methods

        static PostItem ToJson(Post post)
        {
            return new PostItem
            {
                Id = post.Id,
                Author = post.Author,
                Title = post.Title,
                Slug = post.Slug,
                RelativeLink = post.RelativeLink,
                DateCreated = post.DateCreated.ToString("MM/dd/yyyy HH:mm"),
                Categories = GetCategories(post.Categories),
                Tags = GetTags(post.Tags),
                Comments = GetComments(post),
                IsPublished = post.IsPublished,
            };
        }

        static PostDetail ToJsonDetail(Post post)
        {
            return new PostDetail
            {
                Id = post.Id,
                Author = post.Author,
                Title = post.Title,
                Slug = post.Slug,
                Description = post.Description,
                RelativeLink = post.RelativeLink,
                Content = post.Content,
                DateCreated = post.DateCreated.ToString("MM/dd/yyyy HH:mm"),
                Categories = GetCategories(post.Categories),
                Tags = GetTags(post.Tags),
                Comments = GetComments(post),
                HasCommentsEnabled = post.HasCommentsEnabled,
                IsPublished = post.IsPublished,
                IsDeleted = post.IsDeleted,
                CanUserEdit = post.CanUserEdit,
                CanUserDelete = post.CanUserDelete
            };
        }

        static void Save(Post post, PostDetail detail)
        {
            post.Title = detail.Title;
            post.Author = detail.Author;
            post.Description = detail.Description;
            post.Content = detail.Content;
            post.Slug = detail.Slug;
            post.IsPublished = detail.IsPublished;
            post.HasCommentsEnabled = detail.HasCommentsEnabled;
            post.IsDeleted = detail.IsDeleted;
            post.DateCreated = DateTime.ParseExact(detail.DateCreated, "M/d/yyyy HH:mm", CultureInfo.InvariantCulture);

            UpdatePostCategories(post, detail.Categories);
            UpdatePostTags(post, detail.Tags);

            post.Save();
        }

        static List<CategoryItem> GetCategories(ICollection<Category> categories)
        {
            if (categories == null || categories.Count == 0)
                return null;

            //var html = categories.Aggregate("", (current, cat) => current + string.Format
            //("<a href='#' onclick=\"ChangePostFilter('Category','{0}','{1}')\">{1}</a>, ", cat.Id, cat.Title));
            var categoryList = new List<CategoryItem>();
            foreach (var coreCategory in categories)
            {
                var item = new CategoryItem();
                item.Id = coreCategory.Id;
                item.Title = coreCategory.Title;
                item.Description = coreCategory.Description;
                item.Parent = ItemParent(coreCategory.Parent);
                categoryList.Add(item);
            }
            return categoryList;
        }

        static SelectOption ItemParent(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return null;

            var item = Category.Categories.Where(c => c.Id == id).FirstOrDefault();
            return new SelectOption { OptionName = item.Title, OptionValue = item.Id.ToString() };
        }

        static List<TagItem> GetTags(ICollection<string> tags)
        {
            if (tags == null || tags.Count == 0)
                return null;

            var items = new List<TagItem>();
            foreach (var item in tags)
            {
                items.Add(new TagItem { TagName = item });
            }
            return items;
        }

        static string[] GetComments(Post post)
        {
            if (post.Comments == null || post.Comments.Count == 0)
                return null;

            string[] comments = new string[3];
            comments[0] = post.NotApprovedComments.Count.ToString();
            comments[1] = post.ApprovedComments.Count.ToString();
            comments[2] = post.SpamComments.Count.ToString();
            return comments;
        }

        static void UpdatePostCategories(Post post, List<CategoryItem> categories)
        {
            post.Categories.Clear();
            foreach (var cat in categories)
            {
                // add if category does not exist
                var existingCat = Category.Categories.Where(c => c.Title == cat.Title).FirstOrDefault();
                if (existingCat == null)
                {
                    var repo = new CategoryRepository();
                    post.Categories.Add(Category.GetCategory(repo.Add(cat).Id));
                }
                else
                {
                    post.Categories.Add(Category.GetCategory(existingCat.Id));
                }
                
            }
        }

        static void UpdatePostTags(Post post, List<TagItem> tags)
        {
            post.Tags.Clear();
            foreach (var t in tags)
            {
                post.Tags.Add(t.TagName);
            }
        }

        #endregion
    }
}