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
    protected void cblRoles_PreRender(object sender, EventArgs e)
    {
        CheckBoxList cbl = (CheckBoxList)sender;
        GridViewRow drv = (GridViewRow)cbl.Parent.BindingContainer;

        if (User.IsInRole("administrators"))
            cbl.Items[0].Selected = true;
        if (User.IsInRole("editors"))
            cbl.Items[1].Selected = true;


    }
    protected void cblRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBoxList cbl = (CheckBoxList)sender;

        for (int i = 0; i < cbl.Items.Count; i++)
        {
            if (cbl.Items[i].Selected == false)
            {
                if (Roles.GetUsersInRole(cbl.Items[0].Value).Length == 1)
                    Roles.RemoveUserFromRole(User.Identity.Name, cbl.Items[0].Value);
            }
            else
            {
                if (!Roles.IsUserInRole(cbl.Items[0].Value))
                    Roles.AddUserToRole(User.Identity.Name, cbl.Items[0].Value);
            }

        }

    }
}
