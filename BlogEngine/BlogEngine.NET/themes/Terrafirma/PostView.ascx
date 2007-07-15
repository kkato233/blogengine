<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<div class="post xfolkentry">
    <div class="header">
        <h3><a href="<%=Post.RelativeLink %>" class="taggedlink"><%=Post.Title %></a></h3>
        <div class="date">
        Posted on
        <%=Post.DateCreated.ToString("MMMM d, yyyy HH:mm")%>
        by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Post.Author %>.aspx">
            <%=Post.Author %>        </a>
        </div>
    </div>
    
    <div class="content">
        <%=Post.Content %>
        <%=Rating %><br />
    </div>
    
    <div class="footer">
        
        <%=TagLinks(", ") %>
        <br />
        Actions:
        <%=AdminLinks %>
        <a rel="nofollow" href="mailto:?subject=<%=Post.Title %>&amp;body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>">E-mail</a> | <a href="<%=Post.RelativeLink %>#comment">Comments (<%=Post.Comments.Count %>)</a>
        |<a rel="nofollow" href="http://www.dzone.com/links/add.html?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">dzone it!</a>
    </div>
</div>
