// Copyright (c) 2007 Adrian Godong, Ben Maurer
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

// Adapted for dotnetblogengine by Filip Stanek ( http://www.bloodforge.com )

using System;
using System.Data;
using System.Collections.Generic;

namespace Recaptcha
{
    /// <summary>
    /// Summary description for RecaptchaLogItem
    /// </summary>
    [Serializable]
    public class RecaptchaLogItem
    {
        public string Response = String.Empty;
        public string Challenge = String.Empty;
        public Guid CommentID = Guid.Empty;
        public double TimeToComment = 0; // in seconds - this is the time from the initial page load until a captcha was successfully solved
        public double TimeToSolveCapcha = 0; // in seconds - this is the time from the last time the captcha was refreshed until it was successfully solved.
        public UInt16 NumberOfAttempts = 0;
        public bool Enabled = true;
        public bool Necessary = true;
    }

    /// <summary>
    /// Methods to save and retrieve reCaptcha logs
    /// </summary>
    public static class RecaptchaLogger
    {
        /// <summary>
        /// Saves log data to datastore as extension settings
        /// </summary>
        /// <param name="items">List of log items</param>
        public static void SaveLogItems(List<RecaptchaLogItem> items)
        {
            if (items.Count > 0)
            {
                ExtensionSettings settings = ExtensionManager.GetSettings("Recaptcha", "RecaptchaLog");
                DataTable table = settings.GetDataTable();

                if (table.Rows.Count > 0)
                {
                    for (int i = table.Rows.Count -1; i > -1; i--)
                    {
                        foreach (ExtensionParameter par in settings.Parameters)
                            par.DeleteValue(i);
                    }
                }
                
                foreach (RecaptchaLogItem item in items)
                {
                    settings.AddValues(new string[] { 
                        item.Response, 
                        item.Challenge, 
                        item.CommentID.ToString(),
                        item.TimeToComment.ToString(),
                        item.TimeToSolveCapcha.ToString(),
                        item.NumberOfAttempts.ToString(),
                        item.Enabled.ToString(),
                        item.Necessary.ToString() });
                }
                ExtensionManager.SaveSettings("Recaptcha", settings);
            }
        }

        /// <summary>
        /// Read log data from data store
        /// </summary>
        /// <returns>List of log items</returns>
        public static List<RecaptchaLogItem> ReadLogItems()
        {
            ExtensionSettings settings = ExtensionManager.GetSettings("Recaptcha", "RecaptchaLog");
            DataTable table = settings.GetDataTable();
            List<RecaptchaLogItem> log = new List<RecaptchaLogItem>();

            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    RecaptchaLogItem Item = new RecaptchaLogItem();
                    Item.Response = (string)row["Response"];
                    Item.Challenge = (string)row["Challenge"];
                    Item.CommentID = new Guid((string)row["CommentID"]);
                    Item.Enabled = bool.Parse(row["Enabled"].ToString());
                    Item.Necessary = bool.Parse(row["Necessary"].ToString());
                    Item.NumberOfAttempts = ushort.Parse(row["NumberOfAttempts"].ToString());
                    Item.TimeToComment = double.Parse(row["TimeToComment"].ToString());
                    Item.TimeToSolveCapcha = double.Parse(row["TimeToSolveCapcha"].ToString());
                    log.Add(Item);
                }
            }
            return log;
        }
    }
}