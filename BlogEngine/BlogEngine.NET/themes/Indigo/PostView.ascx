<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<div class="post xfolkentry">
    <h1><a class="postheader taggedlink" href="<%=Post.RelativeLink %>"><%=Post.Title %></a></h1>
    <div class="descr"><img id="Img1" src="~/themes/indigo/img/timeicon.gif" runat="server" alt="clock" /> <%=Post.DateCreated.ToString("MMMM d, yyyy HH:mm")%> by <img id="Img2" src="~/themes/indigo/img/author.gif" runat="server" alt="author" /> <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Post.Author %>.aspx"><%=Post.Author %></a></div>
    <div class="postcontent"><%=Body %></div>
    <%=Rating %>
    <br />
    <div class="postfooter">
        <%=TagLinks(", ") %><br />
        Categories: <%=CategoryLinks(" | ") %><br />
        Actions: <%=AdminLinks %>
        <a rel="nofollow" href="mailto:?subject=<%=Post.Title %>&amp;body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>">E-mail</a> | 
        <a rel="nofollow" href="http://www.dotnetkicks.com/submit?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">Kick it!</a> |
        <a href="<%=Post.PermaLink %>">Permalink</a> |
        <a href="<%=Post.RelativeLink %>#comment"><img id="Img3" src="~/themes/indigo/img/comments.gif" runat="server" alt="comment" /> Comments (<%=Post.Comments.Count %>)</a> |
        <a rel="nofollow" href="<%=CommentFeed %>"><asp:Image ID="Image1" runat="Server" ImageUrl="~/pics/rssbutton.gif" AlternateText="RSS comment feed" style="margin-right:3px" />Comment RSS</a>
    </div>
    <br />
</div>