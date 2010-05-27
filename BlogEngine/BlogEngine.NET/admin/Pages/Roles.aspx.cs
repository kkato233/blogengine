using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class admin_Account_Roles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((admin_admin)this.Master).ResetStatus();

        if (!Page.IsPostBack)
        {
            if (!Roles.RoleExists("Administrators"))
            {
                txtCreateRole.Text = "Administrators";
            }
        }
    }

    protected void btnCreateRole_Click(object sender, EventArgs e)
    {
        string roleName = txtCreateRole.Text;

        if (string.IsNullOrEmpty(roleName))
        {
            ((admin_admin)this.Master).SetStatus("warning", "Role name is required field");
            return;
        }

        if(!string.IsNullOrEmpty(roleName))
        {
            try
            {
                Roles.CreateRole(roleName);
                txtCreateRole.Text = null;
            }
            catch (Exception ex)
            {
                ((admin_admin)this.Master).SetStatus("warning", "Could not create the role: " + Server.HtmlEncode(ex.Message));
            }
        }

        RefreshAvailableRolesListBox();
    }

    protected void btnDeleteRole_Click(object sender, EventArgs e)
    {
        if (lbxAvailableRoles.SelectedIndex != -1)
        {
            try
            {
                Roles.DeleteRole(lbxAvailableRoles.SelectedValue);
            }
            catch (Exception ex)
            {
                ((admin_admin)this.Master).SetStatus("warning", "Could not delete the role: " + Server.HtmlEncode(ex.Message));
            }
        }

        RefreshAvailableRolesListBox();
    }

    private void RefreshAvailableRolesListBox()
    {
        lbxAvailableRoles.SelectedIndex = -1;
        lbxAvailableRoles.DataSource = Roles.GetAllRoles();
        lbxAvailableRoles.DataBind();

        if (lbxAvailableRoles.Items.Count == 0)
        {
            lbxAvailableRoles.Visible = false;
            btnDeleteRole.Visible = false;
        }
        else
        {
            lbxAvailableRoles.Visible = true;
            btnDeleteRole.Visible = true;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        RefreshAvailableRolesListBox();
    }  
}