<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="widgets_Visitor_info_widget" %>

<span runat="server" id="pName" />
<br /><br />
<span runat="server" id="pComment" />

<asp:PlaceHolder runat="Server" ID="phScript" Visible="false">
  <div id="visitor_widget_apml" style="margin-top:5px">
  
  </div>
  
  <script type="text/javascript">
  function beginGetApml(website)
  {
    <%=Page.ClientScript.GetCallbackEventReference(this, "website", "endGetApml", "website") %>;
  }

  function endGetApml(arg, context)
  {
    if (arg.length > 10)
    {
      var text = 'I can see you have an APML file at <a href=\"'+context+'">'+context+'</a>. ';
      text += 'Do you wish to filter the content of this blog based on your APML file? ';
      text += '<a href="javascript:void(location.href=\''+KEYwebRoot+'?apml='+encodeURIComponent(arg)+'\')">Apply filter</a>';
      $('visitor_widget_apml').innerHTML = text;
    }
  }

  setTimeout("beginGetApml('<asp:literal runat="server" id="ltWebsite" />')", 1000);
  </script>
</asp:PlaceHolder>