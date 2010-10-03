﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Rules.aspx.cs" Inherits="admin.Comments.Rules" %>
<%@ Import Namespace="Resources"%>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

        <div class="content-box-hdr">
        <span class="SectionHeader"><%=labels.rules %> & <%=labels.filters %></span>
    </div>
	<div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu1" runat="server" />
		</div>
		<div class="content-box-left">

            <fieldset class="rounded">
                <legend><%=labels.rules%></legend>
                <div class="subsection">
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbTrustAuthenticated.ClientID %>"><%=labels.trustAuthenticated%></label>
                        <asp:CheckBox runat="server" ID="cbTrustAuthenticated" />
                        <label class="lbl"><%=labels.alwaysTrust%></label>
                    </div>
                    <div class="sectionRow">
                        <span class="lbl200"><%=labels.addToWhiteList%></span>
                        <asp:DropDownList runat="server" ID="ddWhiteListCount">
                            <asp:ListItem Text="0" />
                            <asp:ListItem Text="1" />
                            <asp:ListItem Text="2" />
                            <asp:ListItem Text="3" />
                            <asp:ListItem Text="5" />
                        </asp:DropDownList> 
                        <span class="lbl"><%=labels.authorApproved%></span> 
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=ddBlackListCount.ClientID %>"><%=labels.commentsBlacklist%></label>
                        <asp:DropDownList runat="server" ID="ddBlackListCount">
                            <asp:ListItem Text="0" />
                            <asp:ListItem Text="1" />
                            <asp:ListItem Text="2" />
                            <asp:ListItem Text="3" />
                            <asp:ListItem Text="5" />
                        </asp:DropDownList> 
                        <span class="lbl"><%=labels.authorRejected%></span>
                    </div>
                    <div class="sectionRow">
                        <label class="lbl200" for="<%=cbBlockOnDelete.ClientID %>"><%=labels.commentsBlockOnDelete%></label>
                        <asp:CheckBox runat="server" ID="cbBlockOnDelete" />
                        <span class="lbl"><%=labels.authorBlocked%></span>
                    </div>
                </div>
            </fieldset>

            <fieldset class="rounded">
                <legend>Anti-spam Services</legend>
                <div class="subsection">
                    <div class="sectionRow">
                        <div style="border:1px solid #f3f3f3">       
                            <asp:GridView ID="gridCustomFilters" 
                                BorderColor="#f8f8f8" 
                                BorderStyle="solid" 
                                BorderWidth="1px"
                                gridlines="None"
                                AlternatingRowStyle-BackColor="#f8f8f8" 
                                HeaderStyle-BackColor="#f3f3f3"
                                cellpadding="2"
                                runat="server"  
                                width="100%" 
                                datakeynames="FullName"
                                AutoGenerateColumns="False" 
                                onrowcommand="gridCustomFilters_RowCommand">
                                <Columns>
                                <asp:BoundField DataField = "FullName" Visible="false" />
                                <asp:TemplateField HeaderText="<%$ Resources:labels, enabled%>" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEnabled" Checked='<%# CustomFilterEnabled(DataBinder.Eval(Container.DataItem, "FullName").ToString()) %>' Enabled="false" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:BoundField DataField = "FullName" HeaderText="<%$ Resources:labels, filterName %>" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField = "Checked" HeaderText="<%$ Resources:labels, cheked %>" HeaderStyle-HorizontalAlign="Left" />
                                
                                <asp:BoundField DataField = "Cought" HeaderText="<%$ Resources:labels, spam %>" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField = "Reported" HeaderText="<%$ Resources:labels, mistakes %>" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="<%$ Resources:labels, accuracy %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# Accuracy(DataBinder.Eval(Container.DataItem, "Checked"), DataBinder.Eval(Container.DataItem, "Reported"))%> % 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:labels, resetCounters %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btnResetCnt" CommandName="btnResetCnt"  CommandArgument='<%# Eval("FullName") %>' OnClientClick="return ConfirmReset();" Text="Reset"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField = "Priority" HeaderText="<%$ Resources:labels, priority %>" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnPriorityUp" runat="server" OnClick="btnPriorityUp_click" ImageAlign="middle" CausesValidation="false" ImageUrl="~/pics/up_arrow_small.gif" CommandName="btnPriorityUp" AlternateText="<%=labels.up%>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnPriorityDwn" runat="server" OnClick="btnPriorityDwn_click" ImageAlign="middle" CausesValidation="false" ImageUrl="~/pics/down_arrow_small.gif" CommandName="btnPriorityDwn" AlternateText="<%=labels.down %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                    </div>
        
                    <div style="padding-top:8px">
                        <asp:CheckBox runat="server" ID="cbReportMistakes" />
                        <span><%=labels.reportMistakesToService %></span>
                    </div>
                    </div>
                </div>
            </fieldset>

            <fieldset class="rounded">
                <legend><%=labels.filters%></legend>
                <div class="subsection">
                    <div class="sectionRow">
                        <asp:DropDownList ID="ddAction" runat="server" CssClass="txt">
                            <asp:ListItem Text="<%$ Resources:labels, block %>" Value="Block" Selected=true></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, allow %>" Value="Allow" Selected=false></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, delete %>" Value="Delete" Selected=false></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddSubject" runat="server" CssClass="txt">
                            <asp:ListItem Text="<%$ Resources:labels, ip %>" Value="IP" Selected=true></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, author %>" Value="Author" Selected=false></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, website %>" Value="Website" Selected=false></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, email %>" Value="Email" Selected=false></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, comment %>" Value="Comment" Selected=false></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddOperator" runat="server" CssClass="txt">
                            <asp:ListItem Text="<%$ Resources:labels, eqls %>" Value="Equals" Selected=true></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:labels, contains %>" Value="Contains" Selected=false></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtFilter" runat="server" CssClass="txt" MaxLength="250" Width="300px"></asp:TextBox>
                        <asp:Button ID="btnAddFilter" class="btn rounded" runat="server" Text="<%$ Resources:labels, addFilter %>" OnClick="btnAddFilter_Click"/>
                        <span runat="Server" ID="FilterValidation" style="color:Red"></span>
                    </div>
                </div>

                <div style="border:1px solid #f3f3f3">
                    <asp:GridView ID="gridFilters" 
                            PageSize="10" 
                            BorderColor="#f8f8f8" 
                            BorderStyle="solid" 
                            BorderWidth="1px"
                            cellpadding="2"
                            runat="server"  
                            width="100%"
                            gridlines="None"
                            AlternatingRowStyle-BackColor="#f8f8f8" 
                            HeaderStyle-BackColor="#f3f3f3"
                            AutoGenerateColumns="False"
                            AllowPaging="True"
                            datakeynames="ID"
                            OnPageIndexChanging="gridView_PageIndexChanging"
                            AllowSorting="True">
                          <Columns>
                            <asp:BoundField DataField = "ID" Visible="false" />
                            <asp:TemplateField HeaderText="<%$ Resources:labels, action %>" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%# Eval("Action") %> 
                                </ItemTemplate>
                            </asp:TemplateField>
                
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <%=Resources.labels.commentsWhere %>
                                </ItemTemplate>
                            </asp:TemplateField>
                
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <%# Eval("Subject") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <%# Eval("Operator") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                
                            <asp:TemplateField HeaderText="<%$ Resources:labels, filter %>" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                     <%# Eval("Filter") %> 
                                </ItemTemplate>
                            </asp:TemplateField>
                        
                            <asp:TemplateField ShowHeader="False" ItemStyle-VerticalAlign="middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageAlign="middle" CausesValidation="false" ImageUrl="~/admin/images/del.png" OnClick="btnDelete_Click" CommandName="btnDelete" AlternateText="<%=labels.delete%>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                          </Columns>
                          <pagersettings Mode="NumericFirstLast" position="Bottom" pagebuttoncount="20" />
                          <PagerStyle HorizontalAlign="Center"/>
                    </asp:GridView>
                </div>
            </fieldset>

            <div class="action_buttons">
                <asp:Button runat="server" class="btn rounded" ID="btnSave" />
                <span class="loader">&nbsp;</span>
            </div>
            
		</div>
	</div> 

</asp:Content>