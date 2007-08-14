using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;


public partial class admin_Pages_Categories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindGrid();
        }

        grid.RowEditing += new GridViewEditEventHandler(grid_RowEditing);
        grid.RowUpdating += new GridViewUpdateEventHandler(grid_RowUpdating);
        grid.RowCancelingEdit += delegate { Response.Redirect(Request.RawUrl); };
        grid.RowDeleting += new GridViewDeleteEventHandler(grid_RowDeleting);
        btnAdd.Click += new EventHandler(btnAdd_Click);
        btnAdd.Text = Resources.labels.add + " " + Resources.labels.category.ToLowerInvariant();
        valExist.ServerValidate += new ServerValidateEventHandler(valExist_ServerValidate);
        Page.Title = Resources.labels.categories;
    }

    private void valExist_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;

        //foreach (string cat in CategoryDictionary.Instance.Values)
        //{
        //  if (cat.Equals(txtNewCategory.Text.Trim(), StringComparison.OrdinalIgnoreCase))
        //    args.IsValid = false;
        //}

        foreach (Category category in Category.Categories)
        {
            if (category.Title.Equals(txtNewCategory.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                args.IsValid = false;
        }


    }

    void btnAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //CategoryDictionary.Instance.Add(txtNewCategory.Text);
            //CategoryDictionary.Instance.Save();

            Category cat = new Category(txtNewCategory.Text, txtNewNewDescription.Text);
            cat.Save();

            BindGrid();
        }
    }

    void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Guid id = (Guid)grid.DataKeys[e.RowIndex].Value;
        Category cat = Category.GetCategory(id);
        cat.Delete();
        cat.Save();
        //CategoryDictionary.Instance.Remove(id);
        //CategoryDictionary.Instance.Save();
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Guid id = (Guid)grid.DataKeys[e.RowIndex].Value;
        TextBox textboxTitle = (TextBox)grid.Rows[e.RowIndex].FindControl("txtTitle");
        TextBox textboxDescription = (TextBox)grid.Rows[e.RowIndex].FindControl("txtDescription");
        Category cat = Category.GetCategory(id);
        cat.Title = textboxTitle.Text;
        cat.Description = textboxDescription.Text;
        cat.Save();

        //CategoryDictionary.Instance[id] = textbox.Text;
        //CategoryDictionary.Instance.Save();
        Response.Redirect(Request.RawUrl);
    }

    void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grid.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    private void BindGrid()
    {
        grid.DataKeyNames = new string[] { "Id" };
        grid.DataSource = Category.Categories;
        //grid.DataSource = CategoryDictionary.Instance;
        grid.DataBind();
    }

}
