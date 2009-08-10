<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataGrid.ascx.cs" Inherits="admin_Comments_DataGrid" %>
<div id="pop1" runat="server" visible="false" style="position:absolute;left:50%;top:50%; margin-top:-135px; width:600px;height:275px;margin-left:-300px;background-color:#FFF;border:1px solid #ccc;border-bottom:3px solid #ccc;border-top:3px solid #ccc">
     <div style="padding:10px">
        <div>
            <span id="popAuthor" runat="server" style="padding:5px"></span>
            <span id="popEmail" runat="server" style="padding:5px"></span>
            <span id="popIp" runat="server" style="padding:5px"></span>
            <span id="popCountry" runat="server" style="padding:5px"></span>
        </div>
        <div>
             <span id="popPost" runat="server" style="padding:5px"></span>
        </div>
        <div>
            <span id="popWebsite" runat="server" style="padding:5px"></span>
        </div>
        <div id="txtComment" style="padding:5px">
            <textarea id="txtArea" runat="server" cols="69" rows="10"></textarea>
        </div>
        <span style="text-align:center; padding-left:220px">
            <asp:Button ID="btnSaveTxt" runat="server" Text="Update" OnClick="btnSaveTxt_Click" />
            <asp:Button ID="btnApprovePop" runat="server" Text="Approve" 
             onclick="btnApprovePop_Click" />
            <asp:Button ID="btnDeletePop" runat="server" Text="Delete" 
             OnClientClick="javascript:return confirm('Are you sure you want to delte this comment?')" 
             onclick="btnDeletePop_Click"  />
            <asp:Button ID="btnCancelPop" runat="server" Text="Cancel" OnClick="btnCancelPop_Click" />
        </span>
        <span id="commId" runat="server" style="visibility:hidden; margin:0; padding:0"></span>
     </div>
</div>
    
<asp:GridView ID="gridComments" 
    BorderColor="Silver" 
    BorderStyle="solid" 
    BorderWidth="1px"
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
        <ItemTemplate></ItemTemplate>
    </asp:TemplateField>  
    <asp:BoundField HeaderText="Author" HeaderStyle-HorizontalAlign="Left" DataField="Author" />
	<asp:BoundField HeaderText="Email" HeaderStyle-HorizontalAlign="Left" DataField="Email" HtmlEncode="False" DataFormatString="<a href='mailto:{0}'>{0}</a>" />		
    <asp:BoundField HeaderText="IP" HeaderStyle-HorizontalAlign="Left" DataField="IP" HtmlEncode="false" DataFormatString="<a href='http://www.domaintools.com/go/?service=whois&q={0}' target='_new'>{0}</a>" />          
    <asp:BoundField DataField="IsApproved" Visible="false" />                                    
    <asp:ButtonField ButtonType="Link" HeaderText="Comment" CommandName="btnInspect" DataTextField="Teaser" HeaderStyle-HorizontalAlign="Left" />           
    <asp:BoundField HeaderText="Date" DataField="DateCreated" DataFormatString="{0:dd-MMM-yyyy HH:mm}" HeaderStyle-HorizontalAlign="Left" />
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