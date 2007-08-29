using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Wilco.Web.SyntaxHighlighting
{
    public partial class SyntaxHighlighter : WebControl
    {
        private class DefaultCodeLayoutTemplate : Control, ITemplate
        {
            public DefaultCodeLayoutTemplate()
            {
                //
            }

            public void InstantiateIn(Control c)
            {
                SourceHeaderItem container = (SourceHeaderItem)c;

                if (c.Page != null && c.Page.Header != null && c.Page.Items["shl_ln"] == null)
                {
                    HtmlGenericControl lineNumberStyle = new HtmlGenericControl("style");
                    lineNumberStyle.Attributes.Add("type", "text/css");
                    lineNumberStyle.InnerHtml = ".shl_ln { border-right: 1px solid #000; padding-right: 2px; }";
                    c.Page.Header.Controls.Add(lineNumberStyle);
                    c.Page.Items["shl_ln"] = new object();
                }

                HtmlTable root = new HtmlTable();
                root.Style.Add(HtmlTextWriterStyle.Width, "100%");
                container.Controls.Add(root);

                HtmlTableRow headerRow = new HtmlTableRow();
                root.Rows.Add(headerRow);

                HtmlTableCell headerCell = new HtmlTableCell();
                headerCell.ColSpan = 2;
                headerCell.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                headerCell.InnerText = container.Language;
                headerRow.Cells.Add(headerCell);

                HtmlTableRow bodyRow = new HtmlTableRow();
                root.Rows.Add(bodyRow);

                HtmlTableCell lineNumberCell = new HtmlTableCell();
                lineNumberCell.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                lineNumberCell.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                lineNumberCell.Style.Add(HtmlTextWriterStyle.Width, "1%");
                bodyRow.Cells.Add(lineNumberCell);

                HtmlGenericControl lineNumberPre = new HtmlGenericControl("pre");
                lineNumberCell.Controls.Add(lineNumberPre);

                LineNumberPlaceHolder lineNumberPH = new LineNumberPlaceHolder();
                lineNumberPre.Controls.Add(lineNumberPH);

                HtmlTableCell sourceLineCell = new HtmlTableCell();
                sourceLineCell.Style.Add(HtmlTextWriterStyle.VerticalAlign, "top");
                sourceLineCell.Style.Add(HtmlTextWriterStyle.Width, "99%");
                bodyRow.Cells.Add(sourceLineCell);

                HtmlGenericControl sourceLinePre = new HtmlGenericControl("pre");
                sourceLineCell.Controls.Add(sourceLinePre);

                SourceLinePlaceHolder sourceLinePH = new SourceLinePlaceHolder();
                sourceLinePre.Controls.Add(sourceLinePH);
            }
        }

        private class DefaultLineNumberTemplate : Control, ITemplate
        {
            public DefaultLineNumberTemplate()
            {
                //
            }

            public void InstantiateIn(Control c)
            {
                SourceItem container = (SourceItem)c;

                Label line = new Label();
                line.CssClass = "shl_ln";
                line.Text = container.LineNumber.ToString();
                container.Controls.Add(line);

                container.Controls.Add(new LiteralControl(Environment.NewLine));
            }
        }

        private class DefaultSourceLineTemplate : Control, ITemplate
        {
            public DefaultSourceLineTemplate()
            {
                //
            }

            public void InstantiateIn(Control c)
            {
                SourceItem container = (SourceItem)c;
                container.Controls.Add(new LiteralControl(container.SourceLine + Environment.NewLine));
            }
        }
    }
}