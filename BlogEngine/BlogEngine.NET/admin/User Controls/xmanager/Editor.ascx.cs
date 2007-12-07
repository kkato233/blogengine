using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class User_controls_xmanager_SourceEditor : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Enabled = true;
        txtEditor.Text = ReadFile(GetExtFileName());
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ext = Request.QueryString["ext"];
        WriteFile(GetExtFileName(), txtEditor.Text);
        Response.Redirect("Default.aspx");
    }

    string GetExtFileName()
    {
        string ext = Request.QueryString["ext"];
        string fileName = HttpContext.Current.Request.PhysicalApplicationPath;
        fileName += "App_Code\\Extensions\\" + ext + ".cs";
        return fileName;
    }

    string ReadFile(string fileName)
    {
        string val = "Source for [" + fileName + "] not found";
        try
        {
            val = File.ReadAllText(fileName);
        }
        catch (Exception) 
        {
            btnSave.Enabled = false;
        }       
        return val;
    }

    static bool WriteFile(string fileName, string val)
    {
        try
        {
            StreamWriter sw = File.CreateText(fileName);
            sw.Write(val);
            sw.Close();
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}
