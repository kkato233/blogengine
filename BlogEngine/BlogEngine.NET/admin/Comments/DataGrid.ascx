<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataGrid.ascx.cs" Inherits="admin_Comments_DataGrid" %>
<script language="javascript">
    function editComment(id) {
        //alert(id);
        window.scrollTo(0, 0);
        var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
        var height = document.documentElement.clientHeight + document.documentElement.scrollTop;
        var layer = document.createElement('div');

        layer.style.zIndex = 2;
        layer.id = 'layer';
        layer.style.position = 'absolute';
        layer.style.top = '0px';
        layer.style.left = '0px';
        layer.style.height = document.documentElement.scrollHeight + 'px';
        layer.style.width = width + 'px';
        layer.style.backgroundColor = 'black';
        layer.style.opacity = '.6';
        layer.style.filter += ("progid:DXImageTransform.Microsoft.Alpha(opacity=60)");
        document.body.style.position = 'static';
        document.body.appendChild(layer);

        var size = { 'height': 450, 'width': 750 };
        var iframe = document.createElement('iframe');

        iframe.name = 'Comment Editor';
        iframe.id = 'CommentEditor';
        iframe.src = 'Editor.aspx?id=' + id;
        iframe.style.height = size.height + 'px';
        iframe.style.width = size.width + 'px';
        iframe.style.position = 'fixed';
        iframe.style.zIndex = 3;
        iframe.style.backgroundColor = 'white';
        iframe.style.border = '4px solid silver';
        iframe.frameborder = '0';

        iframe.style.top = ((height + document.documentElement.scrollTop) / 2) - (size.height / 2) + 'px';
        iframe.style.left = (width / 2) - (size.width / 2) + 'px';

        document.body.appendChild(iframe);
    }
    
    function closeEditor(reload)
    {
      var v = document.getElementById('CommentEditor');
      var l = document.getElementById('layer');
      document.body.removeChild(document.getElementById('CommentEditor'));
      document.body.removeChild(document.getElementById('layer'));
      document.body.style.position = '';

      if (reload) {
          location.reload();
      }
    }
  </script>
   
<asp:GridView ID="gridComments" 
    BorderColor="Silver" 
    BorderWidth="1px"
    RowStyle-BorderWidth="0px"
    RowStyle-BorderStyle="None"
    runat="server"  
    width="100%"
    AlternatingRowStyle-BackColor="#f8f8f8" 
    HeaderStyle-BackColor="#f3f3f3"
    cellpadding="2"
    AutoGenerateColumns="False"
    AllowPaging="True"
    OnPageIndexChanging="gridView_PageIndexChanging"
    ShowFooter="true"
    AllowSorting="True"       
    onrowdatabound="gridComments_RowDataBound">
  <Columns>
    <asp:BoundField DataField = "Id" Visible="false" />       
    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
        <ItemTemplate>
             <asp:CheckBox ID="chkSelect" 
             Enabled='<%#HasNoChildren((Guid)DataBinder.Eval(Container.DataItem, "Id"))%>' 
             runat="server"/>
        </ItemTemplate>
    </asp:TemplateField> 
    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="24">
        <ItemTemplate>
             <%#Gravatar(DataBinder.Eval(Container.DataItem, "Email").ToString(), DataBinder.Eval(Container.DataItem, "Author").ToString())%>
        </ItemTemplate>
    </asp:TemplateField>   
    <asp:BoundField HeaderText="Author" HeaderStyle-HorizontalAlign="Left" DataField="Author" />
    <asp:BoundField HeaderText="IP" HeaderStyle-HorizontalAlign="Left" DataField="IP" HtmlEncode="false" DataFormatString="<a href='http://www.domaintools.com/go/?service=whois&q={0}' target='_new'>{0}</a>" />          
	<asp:BoundField HeaderText="Email" HeaderStyle-HorizontalAlign="Left" DataField="Email" HtmlEncode="False" DataFormatString="<a href='mailto:{0}'>{0}</a>" />		
    <asp:TemplateField HeaderText="Website" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
           <span><%# GetWebsite(DataBinder.Eval(Container.DataItem, "Website"))%></span>
        </ItemTemplate>
    </asp:TemplateField>
    
    <asp:BoundField DataField="IsApproved" Visible="false" />                                    
    <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
           <asp:LinkButton ID="lnkEditComment" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Teaser").ToString()%>' OnClientClick='<%#GetEditHtml(DataBinder.Eval(Container.DataItem, "Id").ToString())%>' />
        </ItemTemplate>
    </asp:TemplateField>
    <%-- <asp:ButtonField ButtonType="Link" HeaderText="Comment" CommandName="btnInspect" DataTextField="Teaser" HeaderStyle-HorizontalAlign="Left" />--%>    
    <asp:BoundField HeaderText="Date" DataField="DateCreated" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Left" />
    
    <asp:TemplateField HeaderText="Moderator" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
             <asp:literal ID="ltModerator" 
             Text='<%#DataBinder.Eval(Container.DataItem, "ModeratedBy") + "" %>' 
             runat="server"/>
        </ItemTemplate>
    </asp:TemplateField>  
  </Columns>
  <pagersettings Mode="NumericFirstLast" position="Bottom" pagebuttoncount="20" />
  <PagerStyle HorizontalAlign="Center"/>
</asp:GridView>

<div style="text-align:center;padding-top:10px">
    <asp:Button ID="btnSelectAll" runat="server" Text="Select All" OnClick="btnSelectAll_Click"/>
    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" OnClick="btnClearAll_Click"/>
    <asp:Button ID="btnApproveAll" runat="server" Text="Approve" OnClick="btnApproveAll_Click" OnClientClick="return confirm('Are you sure you want to approve selected comments?');" />
    <asp:Button ID="btnDeleteAll" runat="server" Text="Delete" OnClick="btnDeleteAll_Click" OnClientClick="return confirm('Are you sure you want to delete selected comments?');" />   
</div>

<div id="ErrorMsg" runat="server" style="color: Red; display: block;"></div>
<div id="InfoMsg" runat="server" style="color: Green; display: block;"></div>