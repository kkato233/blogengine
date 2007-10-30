#region Using

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;

#endregion

namespace BlogEngine.Core.Web.HttpModules
{
	/// <summary>
	/// Removes whitespace from the webpage.
	/// </summary>
	public sealed class CleanPageModule : IHttpModule
	{

		#region IHttpModule Members

		void IHttpModule.Dispose()
		{
			// Nothing to dispose; 
		}

		void IHttpModule.Init(HttpApplication context)
		{
			context.PostRequestHandlerExecute += new EventHandler(context_BeginRequest);
		}

		#endregion

		void context_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication app = sender as HttpApplication;
			if (app.Context.CurrentHandler is BlogEngine.Core.Web.Controls.BlogBasePage)
			{
				app.Response.Filter = new CleanPageFilter(app.Response.Filter);
			}
		}

		#region Stream filter

		private class CleanPageFilter : Stream
		{

			public CleanPageFilter(Stream sink)
			{
				_sink = sink;
			}

			private Stream _sink;

			#region Properites

			public override bool CanRead
			{
				get { return true; }
			}

			public override bool CanSeek
			{
				get { return true; }
			}

			public override bool CanWrite
			{
				get { return true; }
			}

			public override void Flush()
			{
				_sink.Flush();
			}

			public override long Length
			{
				get { return 0; }
			}

			private long _position;
			public override long Position
			{
				get { return _position; }
				set { _position = value; }
			}

			#endregion

			#region Methods

			public override int Read(byte[] buffer, int offset, int count)
			{
				return _sink.Read(buffer, offset, count);
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				return _sink.Seek(offset, origin);
			}

			public override void SetLength(long value)
			{
				_sink.SetLength(value);
			}

			public override void Close()
			{
				_sink.Close();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				byte[] data = new byte[count];
				Buffer.BlockCopy(buffer, offset, data, 0, count);
				string html = System.Text.Encoding.Default.GetString(buffer);

				html = RemovePostback(html);
				html = RemoveValidation(html);
				html = RemoveSubmit(html);

				byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
				_sink.Write(outdata, 0, outdata.GetLength(0));
			}

			private static String RemovePostback(string html)
			{
				int start = html.IndexOf("var theForm = ", StringComparison.Ordinal);
				if (start == -1) return html;

				int end = html.IndexOf("// -->", start, StringComparison.Ordinal);
				if (end == -1) end = html.IndexOf("//]]>", start, StringComparison.Ordinal);
				if (end == -1) return html;

				string formId = ((System.Web.UI.Page)HttpContext.Current.CurrentHandler).Form.ClientID;
				return html.Substring(0, start) + "var theForm=$('" + formId + "');" + html.Substring(end);
			}

			private static String RemoveValidation(string html)
			{
				int start = html.IndexOf("var Page_ValidationActive = false;", StringComparison.Ordinal);
				if (start == -1) return html;

				int end = html.IndexOf("// -->", start, StringComparison.Ordinal);
				if (end == -1) end = html.IndexOf("//]]>", start, StringComparison.Ordinal);
				if (end == -1) return html;

				return html.Substring(0, start) + "InitValidators();" + html.Substring(end);
			} 

			private static String RemoveSubmit(string html)
			{
				string js = "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;\r\nreturn true;";
				return html.Replace(js, "return CleanForm_OnSubmit();");
			}

			#endregion

		}

		#endregion

	}
}