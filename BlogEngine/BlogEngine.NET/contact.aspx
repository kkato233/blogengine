<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contact.aspx.cs" Inherits="contact" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="contact">
    <div id="divForm" runat="server">
      <h1>Contact</h1>
      
      <p><%=BlogSettings.Instance.ContactFormMessage %></p>
      
      <label for="<%=txtName.ClientID %>">Name</label>
      <asp:TextBox runat="server" id="txtName" cssclass="field" />
      <asp:requiredfieldvalidator runat="server" controltovalidate="txtName" ErrorMessage="Please specify your name" /><br />
      
      <label for="<%=txtEmail.ClientID %>">E-mail</label>
      <asp:TextBox runat="server" id="txtEmail" cssclass="field" />
      <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" display="dynamic" ErrorMessage="Please enter a valid email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
      <asp:requiredfieldvalidator runat="server" controltovalidate="txtEmail" ErrorMessage="Please specify your e-mail" /><br />
      
      <label for="<%=txtSubject.ClientID %>">Subject</label>
      <asp:TextBox runat="server" id="txtSubject" cssclass="field" />
      <asp:requiredfieldvalidator runat="server" controltovalidate="txtSubject" ErrorMessage="Please specify a subject" /><br />
      
      <label for="<%=txtMessage.ClientID %>">Message</label>
      <asp:TextBox runat="server" id="txtMessage" textmode="multiline" rows="5" columns="30" />
      <asp:requiredfieldvalidator runat="server" controltovalidate="txtMessage" ErrorMessage="Please write a message" display="dynamic" />    
      
      <br /><br />
      
      <asp:button runat="server" id="btnSend" Text="Send" />    
      <asp:label runat="server" id="lblStatus" visible="false">An error occured. Please try again.</asp:label>
    </div>
    
    <div id="divThank" runat="Server" visible="False">      
      <%=BlogSettings.Instance.ContactFormMessage %>
    </div>
  </div>
</asp:Content>