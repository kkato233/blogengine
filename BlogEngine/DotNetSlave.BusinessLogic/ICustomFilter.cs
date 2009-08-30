﻿namespace BlogEngine.Core
{
    /// <summary>
    /// An interface implemented by anti-spam 
    /// services like Waegis, Akismet etc.
    /// </summary>
    public interface ICustomFilter
    {
        /// <summary>
        /// Initializes anti-spam service
        /// </summary>
        /// <returns>True if service online and credentials validated</returns>
        bool Initialize(string site, string key);
        /// <summary>
        /// Check if comment is spam
        /// </summary>
        /// <param name="comment">BlogEngine comment</param>
        /// <returns>True if comment is spam</returns>
        bool Check(Comment comment);
        /// <summary>
        /// Report mistakes back to service
        /// </summary>
        /// <param name="comment">BlogEngine comment</param>
        /// <param name="isSpam">True if spam wasn't blocked</param>
        void Report(Comment comment, bool isSpam);
    }
}
