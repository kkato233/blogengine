using System;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using BlogEngine.Core;

public partial class Account_PasswordRetrieval : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text;

        if (string.IsNullOrEmpty(email))
        {
            ((Account_Account)this.Master).SetStatus("warning", "Email is required");
            return;
        }

        string userName = Membership.Provider.GetUserNameByEmail(email);

        if (string.IsNullOrEmpty(userName))
        {
            ((Account_Account)this.Master).SetStatus("warning", "Email does not exist in our system");
            return;
        }

        string pwd = Membership.Provider.ResetPassword(userName, "");

        if (!string.IsNullOrEmpty(pwd))
        {
            SendMail(email, userName, pwd);
        }
    }

    void SendMail(string email, string  user, string pwd)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(BlogSettings.Instance.Email);
        mail.To.Add(email);

        mail.Subject = "Your password has been reset";

        mail.Body = "<div style=\"font: 11px verdana, arial\">";
        mail.Body += "Dear " + user + ":";
        mail.Body += "<br/><br/>Your password at \"" + BlogSettings.Instance.Name + "\" has been reset to: " + pwd;
        mail.Body += "<br/><br/>If it wasn't you who initiated the reset, please let us know immediately (use contact form on our site)";
        mail.Body += "<br/><br/>Sincerely,<br/><br/><a href=\"" + Utils.AbsoluteWebRoot.ToString() + "\">" + BlogSettings.Instance.Name + "</a> team.";
        mail.Body += "</div>";
        
        Utils.SendMailMessageAsync(mail);

        ((Account_Account)this.Master).SetStatus("success", "The password has been sent");
    }
}