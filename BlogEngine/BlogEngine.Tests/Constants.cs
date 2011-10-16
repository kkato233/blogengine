namespace BlogEngine.Tests
{
    public class Constants
    {
        // page urls
        public static readonly string Root = "http://localhost:53265";
        public static readonly string AccountLogin = Root + "/Account/login.aspx";

        public static readonly string GalleryExtensions = Root + "/admin/Extensions/Extensions.cshtml";
        public static readonly string LocalExtensions = Root + "/admin/Extensions/default.cshtml";
        public static readonly string GalleryThemes = Root + "/admin/Extensions/Themes.cshtml?t=2";
        public static readonly string LocalThemes = Root + "/admin/Extensions/Themes.cshtml";

        public static readonly string UrlAdminAddNewPost = Root + "/admin/Posts/Add_entry.aspx";
        public static readonly string UrlAdminPosts = Root + "/admin/Posts/Posts.aspx";

        // login
        public static readonly string LoginLinkId = "ctl00_aLogin";
        public static readonly string LogOff = "Log off";
        public static readonly string UserName = "UserName";
        public static readonly string Password = "Password";
        public static readonly string LoginButton = "LoginButton";
        public static readonly string LoginFailedMsg = "Login failed";
        public static readonly string WelcomeToBeMsg = "Welcome to BlogEngine.NET";
        public static readonly string LinkForgotPassword = "linkForgotPassword";
        public static readonly string PasswordRetrievalMsg = "Password Retrieval";

    }
}