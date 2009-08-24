using System.Threading;
using System.ComponentModel;

namespace BlogEngine.Core
{
    ///<summary>
    ///</summary>
    public static class CommentHandlers
    {
        ///<summary>
        /// Initiate event listener
        ///</summary>
        public static void Listen()
        {
            Post.AddingComment += PostAddingComment;           
        }

        #region Event handlers

        static void PostAddingComment(object sender, CancelEventArgs e)
        {
            if(!BlogSettings.Instance.IsCommentsEnabled) return;
            if(!BlogSettings.Instance.EnableCommentsModeration) return;

            Comment newComment = (Comment)sender;

            #region Rules
            // trust authenticated users
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                newComment.IsApproved = true;
                newComment.ModeratedBy = Comment.Moderator.Rule;
                return;
            }

            int blackCnt = 0;
            int whiteCnt = 0;

            foreach (Post p in Post.Posts)
            {
                foreach (Comment c in p.Comments)
                {
                    if(c.Author == newComment.Author)
                    {
                        if(c.IsApproved)
                            whiteCnt++;
                        else
                            blackCnt++;
                    }
                }
            }

            // user is in the white list - approve comment
            if(whiteCnt >= BlogSettings.Instance.CommentWhiteListCount)
            {
                newComment.IsApproved = true;
                newComment.ModeratedBy = Comment.Moderator.Rule;
                return;
            }

            // user is in the black list - reject comment
            if(blackCnt >= BlogSettings.Instance.CommentBlackListCount)
            {
                newComment.IsApproved = false;
                newComment.ModeratedBy = Comment.Moderator.Rule;
                return;
            }
            #endregion
        }

        #endregion
    }
}