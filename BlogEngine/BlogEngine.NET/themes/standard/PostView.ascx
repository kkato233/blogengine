<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<div class="post">
  <h1><a href="<%=Post.RelativeLink %>"><%=Post.Title %></a></h1>
  <span class="author">by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Post.Author %>.aspx"><%=Post.Author %></a></span>
  <span class="pubDate"><%=Post.DateCreated.ToString("d. MMMM yyyy") %></span>
  <div class="text"><%=Post.Content %></div>
  
  <div class="bottom">
    <p class="tags"><%=TagLinks(", ") %></p>
    <p class="categories"><%=CategoryLinks(" | ") %></p>
  </div>

  <div class="footer">    
    <div class="bookmarks">
      <a rel="nofollow" href="mailto:?subject=<%=Post.Title %>&amp;body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>">E-mail</a> | 
      <a rel="nofollow" href="http://www.dotnetkicks.com/submit?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">Kick it!</a> | 
      <a rel="nofollow" href="http://www.dzone.com/links/add.html?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">DZone it!</a> | 
      <a rel="nofollow" href="http://del.icio.us/post?url=<%=Server.UrlEncode(Post.AbsoluteLink.ToString()) %>&amp;title=<%=Server.UrlEncode(Post.Title) %>">del.icio.us</a>      
    </div>
    
    <%=AdminLinks %>
    <a rel="nofollow" href="<%=Post.PermaLink %>">Permalink</a> |
    <a rel="nofollow" href="<%=Post.RelativeLink %>#comment">Comments (<%=Post.Comments.Count %>)</a> |
    <a rel="nofollow" href="<%=Post.TrackbackLink %>">Trackback</a>
  </div>
</div>