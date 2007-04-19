<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="False" CodeFile="post.aspx.cs" Inherits="post" %>
<%@ Register Src="User controls/CommentView.ascx" TagName="CommentView" TagPrefix="uc2" %>
<%@ Register Src="~/themes/standard/PostView.ascx" TagName="PostView" TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="cphBody" runat="Server">
  <asp:placeholder runat="server" id="pwPost" />
  <!-- 
  <rdf:RDF xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
             xmlns:dc="http://purl.org/dc/elements/1.1/"
             xmlns:trackback="http://madskills.com/public/xml/rss/module/trackback/">
    <rdf:Description
        rdf:about="<%=Post.AbsoluteLink %>"
        dc:identifier="<%=Post.AbsoluteLink %>"
        dc:title="<%=Post.Title %>"
        trackback:ping="<%=Post.TrackbackLink %>" />
    </rdf:RDF>
  -->
  
  <blog:RelatedPosts runat="server" ID="related" MaxResults="3" ShowDescription="true" DescriptionMaxLength="100" Headline="Related posts" />
  <asp:label runat="server" id="lbCommentsDisabled" visible="false">Comments are disabled</asp:label>
  <uc2:CommentView ID="CommentView1" runat="server" />
</asp:content>