using App_Code;
using BlogEngine.Core;
using BlogEngine.Core.Data.Contracts;
using BlogEngine.Core.Data.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Http;

public class SettingsController : ApiController
{
    readonly ISettingsRepository repository;

    public SettingsController(ISettingsRepository repository)
    {
        if (!WebUtils.CheckRightsForAdminSettingsPage(true))
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        this.repository = repository;
    }

    public Settings Get()
    {
        try
        {
            return repository.Get();
        }
        catch (UnauthorizedAccessException)
        {
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
        catch (Exception)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }

    public void Put([FromBody]Settings settings, string action = "")
    {
        if (action == "testEmail")
        {
            try
            {
                TestEmail(settings);
            }
            catch (Exception ex)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = "Email failed"
                };
                throw new HttpResponseException(msg);
            }
        }
        else
        {
            try
            {
                repository.Update(settings);
            }
            catch (Exception ex)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = "Failed to save settings"
                };
                throw new HttpResponseException(msg);
            }
        }

    }

    void TestEmail(Settings settings)
    {
        string email = settings.Email;
        string smtpServer = settings.SmtpServer;
        string smtpServerPort = settings.SmtpServerPort.ToString();
        string smtpUserName = settings.SmtpUserName;
        string smtpPassword = settings.SmtpPassword;
        string enableSsl = settings.EnableSsl.ToString();

        var mail = new MailMessage
        {
            From = new MailAddress(email, smtpUserName),
            Subject = string.Format("Test mail from {0}", smtpUserName),
            IsBodyHtml = true
        };
        mail.To.Add(mail.From);
        var body = new StringBuilder();
        body.Append("<div style=\"font: 11px verdana, arial\">");
        body.Append("Success");
        if (HttpContext.Current != null)
        {
            body.Append(
                "<br /><br />_______________________________________________________________________________<br /><br />");
            body.AppendFormat("<strong>IP address:</strong> {0}<br />", Utils.GetClientIP());
            body.AppendFormat("<strong>User-agent:</strong> {0}", HttpContext.Current.Request.UserAgent);
        }

        body.Append("</div>");
        mail.Body = body.ToString();

        string error = Utils.SendMailMessage(mail, smtpServer, smtpServerPort, smtpUserName, smtpPassword, enableSsl.ToString());
        if (!string.IsNullOrEmpty(error))
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
    }
}
