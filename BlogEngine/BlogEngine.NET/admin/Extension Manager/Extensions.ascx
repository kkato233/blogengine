<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/admin/Extension Manager/Extensions.ascx.cs" Inherits="Admin.ExtensionManager.UserControlsXmanagerExtensionsList" %>
<asp:Literal ID="Literal1" runat="server" Text="<h1>Extensions</h1>" />
<div id="lblErrorMsg" style="padding:5px; color:Red;" runat="server"></div>

<asp:GridView ID="gridExtensionsList" 
    runat="server"  
    AutoGenerateColumns="False"
    datakeynames="Name"
    ShowFooter="true"
    gridlines="None"
    AlternatingRowStyle-BackColor="#f8f8f8"
    AlternatingRowStyle-BorderColor="#f8f8f8" 
    CssClass="beTable" >
  <Columns>
    <asp:BoundField HeaderText="<%$Resources:labels,name %>" HeaderStyle-HorizontalAlign="Left" DataField = "Name" />  
    <asp:BoundField HeaderText="<%$Resources:labels,version %>" DataField = "Version" />
    <asp:BoundField HeaderText="<%$Resources:labels,description %>" HeaderStyle-HorizontalAlign="Left" DataField = "Description" /> 
    <asp:TemplateField HeaderText="<%$Resources:labels,author %>" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
           <span><%# HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container.DataItem, "Author").ToString())%></span>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="<%$Resources:labels,status %>" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
           <%# StatusLink(DataBinder.Eval(Container.DataItem, "Name").ToString())%>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="<%$Resources:labels,source %>" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
           <span><%# string.Format("<a href='?ctrl=editor&ext={0}'>{1}</a>", DataBinder.Eval(Container.DataItem, "Name"), Resources.labels.view)%></span>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="<%$Resources:labels,settings %>" HeaderStyle-HorizontalAlign="Left">
        <ItemTemplate>
           <span><%# SettingsLink(DataBinder.Eval(Container.DataItem, "Name").ToString())%></span>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField HeaderText="<%$Resources:labels,priority %>" DataField = "Priority" />
    <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
        <ItemTemplate>
            <asp:ImageButton ID="btnPriorityUp" runat="server" OnClick="BtnPriorityUpClick" ImageAlign="middle" CausesValidation="false" ImageUrl="~/admin/images/action-up.png" CommandName="btnPriorityUp" AlternateText="Up" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20">
        <ItemTemplate>
            <asp:ImageButton ID="btnPriorityDwn" runat="server" OnClick="BtnPriorityDwnClick" ImageAlign="middle" CausesValidation="false" ImageUrl="~/admin/images/action-down.png" CommandName="btnPriorityDwn" AlternateText="" />
        </ItemTemplate>
    </asp:TemplateField>
  </Columns>
  <pagersettings Mode="NumericFirstLast" position="Bottom" pagebuttoncount="20" />
  <PagerStyle HorizontalAlign="Center"/>
</asp:GridView>

<div style="text-align:right">
  <asp:Button runat="Server" ID="btnRestart" 
    Text="<%$Resources:labels,applyChanges %>"   />
</div>