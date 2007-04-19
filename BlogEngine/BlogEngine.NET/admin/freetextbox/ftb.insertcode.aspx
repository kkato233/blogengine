<%@ Page Language="c#" ValidateRequest="false" %>

<!doctype html public "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
  <title>Insert Code</title>
  <base target="_self">
  <meta http-equiv="Expires" content="0">
</head>
<body style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px; padding-top: 10px">
  <form id="Form1" runat="server">
    <asp:DropDownList ID="languageDropDown" runat="server">
      <asp:ListItem Value="C#">C#</asp:ListItem>
      <asp:ListItem Value="VB.NET">VB.NET</asp:ListItem>
      <asp:ListItem Value="J#">J#</asp:ListItem>
      <asp:ListItem Value="T-SQL">T-SQL</asp:ListItem>
      <asp:ListItem Value="ASPX">ASPX</asp:ListItem>
    </asp:DropDownList>
    <br>
    <asp:TextBox ID="sourceTextBox" TextMode="multiline" Columns="60" Rows="10" runat="server"
      Width="100%" />
    <br>
    <asp:Button ID="parseButton" OnClick="parseButton_Click" Text="Parse Code" runat="server" />
    <input id="Button1" type="button" onclick="returnCode()" value="Insert Code" name="Button1"
      runat="server">&nbsp;
    <hr>
    <pre style="font-size: 11px; font-family: Courier New"><asp:Literal ID="resultLabel"
      runat="server" />
<asp:TextBox ID="codeText" runat="server" Width="0px" Height="0px" TextMode="MultiLine"></asp:TextBox>
  </form>

  <script language="javascript">
function returnCode() {
	var text;
	

	text = document.getElementById('codeText').value;
	window.returnValue = text;


	window.close();	
}		
  </script>

  <script language="c#" runat="server">
    void parseButton_Click(object sender, EventArgs e)
    {
      AylarSolutions.Highlight.Highlighter h =
            new AylarSolutions.Highlight.Highlighter();
      h.ConfigurationFile = Server.MapPath("CodeHighlightDefinitions.xml");
      h.OutputType = AylarSolutions.Highlight.OutputType.Html;
      string result = h.Highlight(sourceTextBox.Text, languageDropDown.SelectedValue);
      result = result.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
      result = result.Replace(Environment.NewLine, "<br>");
      resultLabel.Text = result;
      codeText.Text = "<p class=\"code\">" + result + "</p>";
    }
  </script>

</body>
</html>
