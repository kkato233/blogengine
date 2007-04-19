using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admin_newuser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        if (Request.QueryString["delete"] != null)
        {
          Membership.Provider.DeleteUser(Request.QueryString["delete"], false);
          Response.Redirect("manage_users.aspx", true);
        }

        int count = 0;
        gridUsers.DataSource = Membership.Provider.GetAllUsers(0, 0, out count);
        gridUsers.DataBind();
      }

      CreateUserWizard1.CreatedUser += new EventHandler(CreateUserWizard1_CreatedUser);
    }

  void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
  {
    Response.Redirect("manage_users.aspx", true);
  }
}
