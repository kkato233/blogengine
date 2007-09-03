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
using BlogEngine.Core.Providers;

public partial class admin_newuser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["delete"] != null)
            {
                Membership.Provider.DeleteUser(Request.QueryString["delete"], false);
                Response.Redirect("users.aspx", true);
            }

            int count = 0;
            gridUsers.DataSource = Membership.Provider.GetAllUsers(0, 999, out count);
            gridUsers.DataBind();
        }

        CreateUserWizard1.CreatedUser += new EventHandler(CreateUserWizard1_CreatedUser);
        Page.Title = Resources.labels.users;
    }

    void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        Response.Redirect("users.aspx", true);
    }


    protected void cb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow drv = (GridViewRow)cb.Parent.BindingContainer;
        string _userName = gridUsers.DataKeys[drv.DataItemIndex].Value.ToString();
        string _roleToUse = cb.Text;
        if (cb.Checked == false)
        {
            if (User.Identity.Name !=_userName.ToLower())
                Roles.RemoveUserFromRole(_userName, _roleToUse);
        }
        else
        {
            if (!Roles.IsUserInRole(_userName, _roleToUse))
                Roles.AddUserToRole(_userName, _roleToUse);
        }
        Response.Redirect("users.aspx", true);
    }

    protected void gridUsers_Load(object sender, EventArgs e)
    {
        for (int i = 0; i < gridUsers.Rows.Count; i++)
        {
            string[] allRoles = Roles.GetAllRoles();
            foreach (string _role in allRoles)
            {
                CheckBox cb = new CheckBox();
                cb.Text = _role;
                cb.Checked = Roles.IsUserInRole(gridUsers.DataKeys[i].Value.ToString(), _role);
                cb.AutoPostBack = true;
                cb.TextAlign = TextAlign.Right;
                cb.Style.Add("display", "inline");
                cb.Style.Add("padding-right", "15px");
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                gridUsers.Rows[i].Cells[3].Controls.Add(cb);
            }
        }
    }
}
