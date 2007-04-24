/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/12/2007	brian.kuhn		Created Rfc822DateTime Class
****************************************************************************/
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BlogEngine.Core.Syndication
{
    /// <summary>
    /// Represents an RFC 822 formatted datetime structure.
    /// </summary>
    /// <remarks>See http://asg.web.cmu.edu/rfc/rfc822.html for for details on the RFC822 date time guidelines.</remarks>
    [Serializable()]
    public class Rfc822DateTime
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the DateTime for the RFC822 datetime structure.
        /// </summary>
        private DateTime baseDateTime;
        #endregion

        //============================================================
		//	CONSTRUCTORS
        //============================================================
        #region Rfc822DateTime(DateTime dateTme)
        /// <summary>
        /// Initializes a new instance of the <see cref="Rfc822DateTime"/> class using the supplied <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime">The datetime to represent in a RFC822 format.</param>
        public Rfc822DateTime(DateTime dateTime)
		{
			//------------------------------------------------------------
			//	Attempt to handle class initialization
			//------------------------------------------------------------
			try
			{
				//------------------------------------------------------------
				//	Set class members
				//------------------------------------------------------------
                baseDateTime    = dateTime;
			}
			catch
			{
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
			}
		}
		#endregion

        #region Rfc822DateTime(DateTime rfcDate)
        /// <summary>
        /// Initializes a new instance of the <see cref="Rfc822DateTime"/> class using the supplied RFC822 formatted string.
        /// </summary>
        /// <param name="rfcDate">The datetime represented in a RFC822 format.</param>
        public Rfc822DateTime(string rfcDate)
        {
            //------------------------------------------------------------
            //	Attempt to handle class initialization
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set class members
                //------------------------------------------------------------
                baseDateTime    = Rfc822DateTime.FromString(rfcDate);
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region DateTime
        /// <summary>
        /// Gets the <see cref="DateTime"/> to represent as a RFC 822 formatted datetime structure.
        /// </summary>
        /// <value>The <b>DateTime</b> to represent as a RFC 822 formatted datetime structure.</value>
        public DateTime DateTime
        {
            get
            {
                return baseDateTime;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC STATIC ROUTINES
        //============================================================
        #region FromString(string date)
        /// <summary>
        /// Returns a System.DateTime structure by parsing the supplied RFC822 datetime string.
        /// </summary>
        /// <param name="date">RFC822 datetime to parse</param>
        /// <returns>DateTime</returns>
        public static DateTime FromString(string date)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            System.DateTime dateTime;
            int position;
            
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            if (String.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("date");
            }

            //------------------------------------------------------------
            //	Initialize local members
            //------------------------------------------------------------
            position = date.LastIndexOf(" ");

            //------------------------------------------------------------
            //	Attempt to parse from string
            //------------------------------------------------------------
            try
            {
                dateTime        = Convert.ToDateTime(date, DateTimeFormatInfo.InvariantInfo);

                if (date.Substring(position + 1) == "Z")
                {
                    dateTime    = dateTime.ToUniversalTime();
                }
                else if (date.Substring(position + 1) == "GMT")
                {
                    dateTime    = dateTime.ToUniversalTime();
                }

                //------------------------------------------------------------
                //	Return result
                //------------------------------------------------------------
                return dateTime;
            }
            catch (System.FormatException formatException)
            {
                System.Diagnostics.Trace.WriteLine(formatException.Message);
            }

            //------------------------------------------------------------
            //	Attempt to parse from string at alternate position
            //------------------------------------------------------------
            dateTime    = Convert.ToDateTime(date.Substring(0, position), DateTimeFormatInfo.InvariantInfo);

            if (date[position + 1] == '+')
            {
                int hours   = Convert.ToInt32(date.Substring(position + 2, 2), NumberFormatInfo.InvariantInfo);
                dateTime    = dateTime.AddHours(-hours);
                int minutes = Convert.ToInt32(date.Substring(position + 4, 2), NumberFormatInfo.InvariantInfo);
                dateTime    = dateTime.AddMinutes(-minutes);
            }
            else if (date[position + 1] == '-')
            {
                int hours   = Convert.ToInt32(date.Substring(position + 2, 2), NumberFormatInfo.InvariantInfo);
                dateTime    = dateTime.AddHours(hours);
                int minutes = Convert.ToInt32(date.Substring(position + 4, 2), NumberFormatInfo.InvariantInfo);
                dateTime    = dateTime.AddMinutes(minutes);
            }
            else
            {
                //------------------------------------------------------------
                //	Return result
                //------------------------------------------------------------
                dateTime = ApplyDateTimeFormat(date.Substring(position + 1), dateTime);
            }

            return dateTime;
        }
        #endregion

        #region Parse(string date)
        /// <summary>
        /// Returns a <see cref="Rfc822DateTime"/> by parsing the supplied RFC822 datetime string.
        /// </summary>
        /// <param name="date">RFC822 datetime to parse</param>
        /// <returns>DateTime</returns>
        public static Rfc822DateTime Parse(string date)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            if (String.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("date");
            }

            //------------------------------------------------------------
            //	Generate DateTime for RFC date string
            //------------------------------------------------------------
            DateTime dateTime   = Rfc822DateTime.FromString(date);

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return new Rfc822DateTime(dateTime);
        }
        #endregion

        #region TryParse(string date, out Rfc822DateTime rfcDate)
        /// <summary>
        /// Returns a value indicating if sucessfully able to parse RFC822 formatted datetime string.
        /// </summary>
        /// <param name="date">RFC822 datetime to parse.</param>
        /// <param name="rfcDate">The <see cref="Rfc822DateTime"/> represented by the datetime string.</param>
        /// <returns><b>true</b> if able to parse string representation, otherwise returns false.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryParse(string date, out Rfc822DateTime rfcDate)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            bool parseSucceeded = false;

            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            if (String.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("date");
            }

            //------------------------------------------------------------
            //	Attempt to parse
            //------------------------------------------------------------
            try
            {
                rfcDate         = Rfc822DateTime.Parse(date);
                parseSucceeded  = true;
            }
            catch(Exception)
            {
                rfcDate         = null;
                parseSucceeded  = false;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return parseSucceeded;
        }
        #endregion

        #region ToString(DateTime date)
        /// <summary>
        /// Returns the RFC822 datetime format for the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">DateTime to convert to RFC822 format.</param>
        /// <returns>The RFC822 datetime format for the specified <b>DateTime</b>.</returns>
        public static string ToString(DateTime date)
        {
            //------------------------------------------------------------
            //	Attempt to return RFC 822 formatted datetime
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Get offset and time zone
                //------------------------------------------------------------
                int offset      = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                string timeZone = "+" + offset.ToString(NumberFormatInfo.InvariantInfo).PadLeft(2, '0');

                //------------------------------------------------------------
                //	Adjust time zone based on offset
                //------------------------------------------------------------
                if (offset < 0)
                {
                    int i       = offset * -1;
                    timeZone    = "-" + i.ToString(NumberFormatInfo.InvariantInfo).PadLeft(2, '0');

                }

                //------------------------------------------------------------
                //	Return RFC 822 formatted DateTime
                //------------------------------------------------------------
                return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), DateTimeFormatInfo.InvariantInfo);
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDDEN ROUTINES
        //============================================================
        #region ToString()
        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of this instance.</returns>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return string representation of RFC 822 date time.
                //------------------------------------------------------------
                return Rfc822DateTime.ToString(baseDateTime);
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
        }
        #endregion

        //============================================================
        //	PRIVATE STATIC ROUTINES
        //============================================================
        #region ApplyDateTimeFormat(string formatCode, DateTime dateTime)
        /// <summary>
        /// Private routine to add appropriate hours to DateTime based on supplied format code.
        /// </summary>
        /// <param name="formatCode">Code to use in determining hours to add.</param>
        /// <param name="dateTime">DateTime structure to modify.</param>
        /// <returns>DateTime structure modified based on format code.</returns>
        private static DateTime ApplyDateTimeFormat(string formatCode, DateTime dateTime)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            Dictionary<string, int> dictionary  = new Dictionary<string,int>();
            DateTime result;

            //------------------------------------------------------------
            //	Attempt to add hours
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Set default result
                //------------------------------------------------------------
                result  = dateTime;

                //------------------------------------------------------------
                //	Fill lookup dictionary
                //------------------------------------------------------------
                dictionary.Add("A", 1);
                dictionary.Add("B", 2);
                dictionary.Add("C", 3);
                dictionary.Add("D", 4);
                dictionary.Add("E", 5);
                dictionary.Add("F", 6);
                dictionary.Add("G", 7);
                dictionary.Add("H", 8);
                dictionary.Add("I", 9);
                dictionary.Add("K", 10);
                dictionary.Add("L", 11);
                dictionary.Add("M", 12);

                dictionary.Add("N", -1);
                dictionary.Add("O", -2);
                dictionary.Add("P", -3);
                dictionary.Add("Q", -4);
                dictionary.Add("R", -5);
                dictionary.Add("S", -6);
                dictionary.Add("T", -7);
                dictionary.Add("U", -8);
                dictionary.Add("V", -9);
                dictionary.Add("W", -10);
                dictionary.Add("X", -11);
                dictionary.Add("Y", -12);

                dictionary.Add("EST", 5);
                dictionary.Add("EDT", 4);

                dictionary.Add("CST", 6);
                dictionary.Add("CDT", 5);

                dictionary.Add("MST", 7);
                dictionary.Add("MDT", 6);

                dictionary.Add("PST", 8);
                dictionary.Add("PDT", 7);

                //------------------------------------------------------------
                //	Add hours for specified format code via lookup
                //------------------------------------------------------------
                result  = dateTime.AddHours(dictionary[formatCode.ToUpperInvariant()]);
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Return formatted result
            //------------------------------------------------------------
            return result;
        }
        #endregion
    }
}
