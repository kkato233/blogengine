using BlogEngine.Core.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace BlogEngine.Core
{
    public class BlogGenerator
    {
        public static Blog CreateNewBlog(string blogName, string userName, string email, string password, out string message)
        {
            message = null;

            if (!ValidateProperties(blogName, userName, email, out message))
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "Validation for new blog failed.";
                }
                return null;
            }

            Blog newBlog = new Blog()
            {
                Name = blogName,
                StorageContainerName = blogName.Trim().ToLower(),
                Hostname = "",
                IsAnyTextBeforeHostnameAccepted = true,
                VirtualPath = "~/" + blogName,
                IsActive = true,
                IsSiteAggregation = false
            };

            bool setupResult = false;
            try
            {
                setupResult = BlogService.SetupNewBlog(newBlog, userName, email, password);
            }
            catch (Exception ex)
            {
                Utils.Log("BlogGenerator.CreateNewBlog", ex);
                message = "Failed to create new blog. Error: " + ex.Message;
                return null;
            }

            if (!setupResult)
            {
                message = "Failed during process of setting up new blog instance.";
                return null;
            }

            // save the blog for the first time.
            newBlog.Save();

            return newBlog;
        }

        /// <summary>
        /// Validates the blog properties.
        /// </summary>
        public static bool ValidateProperties(string blogName, string userName, string email, out string message)
        {
            message = null;

            if (string.IsNullOrWhiteSpace(blogName))
            {
                message = "Blog Name is Required.";
                return false;
            }

            Regex validChars = new Regex("^[a-z0-9-_]+$", RegexOptions.IgnoreCase);

            if (!string.IsNullOrWhiteSpace(blogName) && !validChars.IsMatch(blogName))
            {
                message = "Blog Name contains invalid characters.";
                return false;
            }

            if (Blog.Blogs.Where(b => b.Name.ToLower() == blogName.ToLower()).FirstOrDefault() != null)
            {
                message = "Blog with this name already exists; Please select different name.";
                return false;
            }

            return true;
        }

        public static bool CopyTemplateBlogFolder(string blogName, string userName, string email, string password)
        {
            string templateUrl = Path.Combine(BlogConfig.StorageLocation, "blogs", "_new");
            string newBlogUrl = Path.Combine(BlogConfig.StorageLocation, "blogs", blogName);
            string templateFolderPath = HostingEnvironment.MapPath(templateUrl);
            string newBlogFolderPath = HostingEnvironment.MapPath(newBlogUrl);
            try
            {

                if (!Directory.Exists(templateFolderPath))
                {
                    throw new Exception(string.Format("Template folder for new blog does not exist.  Directory not found is: {0}", templateFolderPath));
                }
            }
            catch (Exception ex)
            {
                Utils.Log("BlogGenerator.CopyTemplateBlogFolder", ex);
                throw;  // re-throw error so error message bubbles up.
            }

            // If newBlogFolderPath already exists, throw an exception as this may be a mistake
            // and we don't want to overwrite any existing data.
            try
            {
                if (Directory.Exists(newBlogFolderPath))
                {
                    throw new Exception(string.Format("Blog destination folder already exists. {0}", newBlogFolderPath));
                }
            }
            catch (Exception ex)
            {
                Utils.Log("BlogGenerator.CopyTemplateBlogFolder", ex);
                throw;  // re-throw error so error message bubbles up.
            }
            if (!Utils.CreateDirectoryIfNotExists(newBlogFolderPath))
                return false;

            // Copy the entire directory contents.
            DirectoryInfo source = new DirectoryInfo(templateFolderPath);
            DirectoryInfo target = new DirectoryInfo(newBlogFolderPath);

            try
            {
                // if the primary blog directly in App_Data is the 'source', when all the directories/files are
                // being copied to the new location, we don't want to copy the entire BlogInstancesFolderName
                // (by default ~/App_Data/blogs) to the new location.  Everything except for that can be copied.
                // If the 'source' is a blog instance under ~/App_Data/blogs (e.g. ~/App_Data/blogs/template),
                // then this is not a concern.

                Utils.CopyDirectoryContents(source, target, new List<string>() { BlogConfig.BlogInstancesFolderName });

                // replace generic "admin" with new blog user
                ReplaceInFile(newBlogFolderPath + @"\users.xml", "<UserName>Admin</UserName>", "<UserName>" + userName + "</UserName>");
                ReplaceInFile(newBlogFolderPath + @"\users.xml", "<Password>jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=</Password>", "<Password>" + Utils.HashPassword(password) + "</Password>");
                ReplaceInFile(newBlogFolderPath + @"\users.xml", "<Email>post@example.com</Email>", "<Email>" + email + "</Email>");
                ReplaceInFile(newBlogFolderPath + @"\roles.xml", "<user>Admin</user>", "<user>" + userName + "</user>");

                string postFile = newBlogFolderPath + @"\posts\c3b491e5-59ac-4f6a-81e5-27e971b903ed.xml";
                string blogPost = "Dear {0}, welcome to you new blog! <br/>Log in <a href=\"{1}\">here</a> to start using it. <br/>Happy blogging!";

                ReplaceInFile(postFile, "<author>Admin</author>", "<author>" + userName + "</author>");
                ReplaceInFile(postFile, "Welcome to your new blog!", string.Format(blogPost, userName, Utils.RelativeWebRoot + blogName + "/Account/login.aspx"));
                
            }
            catch (Exception ex)
            {
                Utils.Log("BlogGenerator.CopyTemplateBlogFolder", ex);
                throw;  // re-throw error so error message bubbles up.
            }

            return true;
        }

        static void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            var cnt = 0;
            StreamReader reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();
            cnt = content.Length;
            reader.Close();

            content = Regex.Replace(content, searchText, replaceText);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

    }
}
