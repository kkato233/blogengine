using WatiN.Core;

namespace BlogEngine.Tests.PageTemplates.Admin
{
    public class EditPost : Page
    {
        public string Url
        {
            get { return Constants.Root + "/admin/Posts/Add_entry.aspx"; }
        }

        public TextField PostTitle
        {
            get { return Document.TextField(Find.ById("ctl00_cphAdmin_txtTitle")); }
        }

        // tinyMCE uses frames to simulate text area, need javascript hack as workaround
        public string JsHack
        {
            get { return "document.getElementById('ctl00_cphAdmin_txtContent_TinyMCE1_txtContent_ifr').contentWindow.document.body.innerHTML = 'This is WATIN test post.';"; }
        }

        public Button Save
        {
            get { return Document.Button(Find.ById("btnSave")); }
        }
    }
}
