<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<div class="post xfolkentry">
  <h1><a href="<%=Post.RelativeLink %>" class="taggedlink"><%=Post.Title %></a></h1>
  <div class="postText"><%=Body %></div>
  
  <div class="bottom">
    <div class="rating"><%=Rating %></div><br />
    <div style="clear:both;"></div>
  </div>
  <div class="bottom">
    <% if (TagLinks(", ") != null) { %>
    <div class="tags">Tags: <%=TagLinks(", ") %></div>
    <%  }
        if (CategoryLinks(" | ") != null)
        {
            if (CategoryLinks(" | ").Length > 0)
            { %>
    <div class="categories">Categories: <%=CategoryLinks(" | ")%></div>
    <% }   } %>
    <div style="clear:both;"></div>
  </div>

  <div class="postFoot">
    <div id="postAuthor">
        <span class="author">Posted by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Post.Author %>.aspx"><%=Post.Author %></a></span>
        <span class="pubDate"> on <%=Post.DateCreated.ToLongDateString() + " " + Post.DateCreated.ToShortTimeString() %></span>
    </div>
    <div class="bookmarks">
      <a rel="nofollow" href="mailto:?subject=<%=Post.Title %>&amp;body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>">E-mail</a> | 
      <a rel="nofollow" href="http://www.dotnetkicks.com/submit?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">Kick it!</a> | 
      <a rel="nofollow" href="http://www.dzone.com/links/add.html?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">DZone it!</a> | 
      <a rel="nofollow" href="http://del.icio.us/post?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">del.icio.us</a>      
    </div>
    
    <%=AdminLinks %>
    <a rel="nofollow" href="<%=Post.PermaLink %>">Permalink</a> |
    <a rel="nofollow" href="<%=Post.RelativeLink %>#comment">Comments (<%=Post.Comments.Count %>)</a> |
    <a rel="nofollow" href="<%=CommentFeed %>">Post RSS<asp:Image runat="Server" ImageUrl="~/pics/rssbutton.gif" AlternateText="RSS comment feed" style="margin-left:3px" /></a>    
  </div>
</div>