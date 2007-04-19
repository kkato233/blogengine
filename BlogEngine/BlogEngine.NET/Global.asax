<%@ Application Language="C#" %>

<script RunAt="server">
  
  void Application_BeginRequest(object sender, EventArgs e)
  {
    Response.AppendHeader("P3P", "CP=\"NOI DSP COR ADMa IVDa OUR NOR\"");
    Response.AppendHeader("Pics-Label", "(pics-1.1 \"http://www.icra.org/ratingsv02.html\" l gen true for \"http://" + Request.Url.Host + "\" r (nz 1 vz 1 lz 1 oz 1 cz 1))");    
  }

  void Application_End(object sender, EventArgs e)
  {
    //  Code that runs on application shutdown

  }

private void Application_Error(object sender, EventArgs e)
{
  if (!Request.IsLocal)
  {
    //HttpException ex = Server.GetLastError() as HttpException;
    //if (ex != null)
    //{
    //  // When a HttpException occurs.
    //  Response.StatusCode = ex.GetHttpCode();
    //}
    //else
    //{
    //  // When any other exception occurs.
    //  Response.StatusCode = 500;
    //}

    //Response.End();
  }
}

  void Session_Start(object sender, EventArgs e)
  {
    // Code that runs when a new session is started

  }

  void Session_End(object sender, EventArgs e)
  {
    // Code that runs when a session ends. 
    // Note: The Session_End event is raised only when the sessionstate mode
    // is set to InProc in the Web.config file. If session mode is set to StateServer 
    // or SQLServer, the event is not raised.

  }
       
</script>

