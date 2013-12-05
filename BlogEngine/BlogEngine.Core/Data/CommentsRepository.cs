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
    /// Comments repository
    /// </summary>
    public class CommentsRepository : ICommentsRepository
    {
        /// <summary>
        /// Comments list
        /// </summary>
        /// <param name="commentType">Comment type</param>
        /// <param name="take">Items to take</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="filter">Filter expression</param>
        /// <param name="order">Sort order</param>
        /// <returns>List of comments</returns>
        public IEnumerable<BlogEngine.Core.Data.Models.CommentItem> GetComments(CommentType commentType = CommentType.All, int take = 10, int skip = 0, string filter = "", string order = "")
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.ViewPublicComments))
                throw new System.UnauthorizedAccessException();

            if (string.IsNullOrEmpty(filter)) filter = "1==1";
            if (string.IsNullOrEmpty(order)) order = "DateCreated desc";

            var items = new List<BlogEngine.Core.Comment>();
            var query = items.AsQueryable().Where(filter);
            var comments = new List<BlogEngine.Core.Data.Models.CommentItem>();

            foreach (var p in Post.Posts)
            {
                switch (commentType)
                {
                    case CommentType.Pending:
                        items.AddRange(p.NotApprovedComments);
                        break;
                    case CommentType.Pingback:
                        items.AddRange(p.Pingbacks);
                        break;
                    case CommentType.Spam:
                        items.AddRange(p.SpamComments);
                        break;
                    case CommentType.Approved:
                        items.AddRange(p.ApprovedComments);
                        break;
                    default:
                        items.AddRange(p.Comments);
                        break;
                }
            }

            // if take passed in as 0, return all
            if (take == 0) take = items.Count;        

            foreach (var item in query.OrderBy(order).Skip(skip).Take(take))
                comments.Add(CreateJsonCommentFromComment(item));

            return comments;
        }

        /// <summary>
        /// Single commnet by ID
        /// </summary>
        /// <param name="id">
        /// Comment id
        /// </param>
        /// <returns>
        /// A JSON Comment
        /// </returns>
        public BlogEngine.Core.Data.Models.CommentItem FindById(Guid id)
        {
            if (!Security.IsAuthorizedTo(BlogEngine.Core.Rights.ViewPublicComments))
                throw new System.UnauthorizedAccessException();

            return (from p in Post.Posts
                    from c in p.AllComments
                    where c.Id == id
                    select CreateJsonCommentFromComment(c)).FirstOrDefault();
        }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="item">Comment</param>
        /// <returns>Comment object</returns>
        public Data.Models.CommentItem Add(Data.Models.CommentItem item)
        {
            if (!Security.IsAuthorizedTo(Rights.CreateComments))
                throw new System.UnauthorizedAccessException();

            return null;
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">Item to update</param>
        /// <returns>True on success</returns>
        public bool Update(Data.Models.CommentItem item, string action)
        {
            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
                throw new System.UnauthorizedAccessException();

            foreach (var p in Post.Posts.ToArray())
            {
                foreach (var c in p.Comments.Where(c => c.Id == item.Id).ToArray())
                {
                    if (action == "approve")
                    {
                        c.IsApproved = true;
                        c.IsSpam = false;
                        p.DateModified = DateTime.Now;
                        p.Save();
                        return true;
                    }

                    if (action == "unapprove")
                    {
                        c.IsApproved = false;
                        c.IsSpam = true;
                        p.DateModified = DateTime.Now;
                        p.Save();
                        return true;
                    }

                    c.Author = item.Author;
                    c.Email = item.Email;
                    c.Website = string.IsNullOrEmpty(item.Website) ? null : new Uri(item.Website);

                    if (item.IsPending)
                    {
                        c.IsApproved = false;
                        c.IsSpam = false;
                    }
                    if (item.IsApproved)
                    {
                        c.IsApproved = true;
                        c.IsSpam = false;
                    }
                    if (item.IsSpam)
                    {
                        c.IsApproved = false;
                        c.IsSpam = true;
                    }
                    // need to mark post as "dirty"
                    p.DateModified = DateTime.Now;
                    p.Save();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>True on success</returns>
        public bool Remove(Guid id)
        {
            if (!Security.IsAuthorizedTo(Rights.ModerateComments))
                throw new System.UnauthorizedAccessException();

            foreach (var p in Post.Posts.ToArray())
            {
                BlogEngine.Core.Comment item = (from cmn in p.AllComments
                    where cmn.Id == id select cmn).FirstOrDefault();

                if (item != null)
                {
                    p.RemoveComment(item);
                    p.DateModified = DateTime.Now;
                    p.Save();
                    return true;
                }
            }
            return false;
        }

        #region Private methods

        private BlogEngine.Core.Data.Models.CommentItem CreateJsonCommentFromComment(Comment c)
        {
            var jc = new BlogEngine.Core.Data.Models.CommentItem();

            jc.Id = c.Id;
            jc.IsApproved = c.IsApproved;
            jc.IsSpam = c.IsSpam;
            jc.IsPending = !c.IsApproved && !c.IsSpam;
            // set both pingbacks and trackbacks to just pingbacks for UI filtering
            jc.Email = c.Email == "trackback" ? "pingback" : c.Email; 
            jc.Author = c.Author;
            jc.Title = c.Teaser;
            jc.Website = c.Website == null ? "" : c.Website.ToString();
            jc.AuthorAvatar = c.Avatar;
            jc.Ip = c.IP;
            jc.DateCreated = c.DateCreated.ToString("MM/dd/yyyy HH:mm");
            jc.RelativeLink = c.RelativeLink;
            jc.HasChildren = c.ParentId == Guid.Empty || c.ParentId == null;
            jc.Avatar = GetAvatar(c.Website, c.Email, c.Author);

            return jc;
        }

        private BlogEngine.Core.Comment GetCoreFromJson(BlogEngine.Core.Data.Models.CommentItem c)
        {
            BlogEngine.Core.Comment item = (from p in Post.Posts
                from cmn in p.AllComments where cmn.Id == c.Id select cmn).FirstOrDefault();

            if (c.IsPending)
            {
                item.IsApproved = false;
                item.IsSpam = false;
            }
            if (c.IsApproved)
            {
                item.IsApproved = true;
                item.IsSpam = false;
            }
            if (c.IsSpam)
            {
                item.IsApproved = false;
                item.IsSpam = true;
            }

            item.Email = c.Email;
            item.Author = c.Author;
            item.Website = string.IsNullOrEmpty(c.Website) ? null : new Uri(c.Website);
            item.IP = c.Ip;
            return item;
        }

        private static string GetAvatar(Uri website, string email, string author)
        {
            Avatar avatar = Core.Avatar.GetAvatar(32, email, website, null, author);

            if (avatar.HasNoImage || string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                // <img> tag pointing to noavatar.jpg, or no image if Avatar setting is "none".
                return avatar.ImageTag ?? string.Empty;
            }

            const string linkWithImage = "<a href=\"mailto:{2}\" alt=\"{0}\" title=\"{0}\">{1}</a>";
            return string.Format(CultureInfo.InvariantCulture, linkWithImage, author, avatar.ImageTag, email);
        }
        
        #endregion
    }
}