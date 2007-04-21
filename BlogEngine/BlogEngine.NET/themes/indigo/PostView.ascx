<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>

<div class="post">
    <h1><a class="postheader" href="<%=Post.RelativeLink %>"><%=Post.Title %></a></h1>
    <div class="descr"><%=Post.DateCreated.ToString("MMMM d, yyyy HH:mm")%> by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Post.Author %>.aspx"><%=Post.Author %></a></div>
    <div class="postcontent"><%=Post.Content %></div>
    <br />
    <div class="postfooter">
        Tags: <%=TagLinks(", ") %><br />
        Categories: <%=CategoryLinks(" | ") %><br />
        Actions: <%=AdminLinks %>
        <a rel="nofollow" href="mailto:?subject=<%=Post.Title %>&body=Thought you might like this: <%=Post.AbsoluteLink.ToString() %>">E-mail</a> | 
        <a href="<%=Post.PermaLink %>">Permalink</a> |
        <a href="<%=Post.RelativeLink %>#comment">Comments (<%=Post.Comments.Count %>)</a> |
        <a href="<%=Post.TrackbackLink %>">Trackback</a>
    </div>
    <br />
</div>