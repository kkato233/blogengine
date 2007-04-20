/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/12/2007	brian.kuhn	Created W3CDateTime Class
****************************************************************************/
using System;
using System.Globalization;
using System.Text.RegularExpressions;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication
{
    /// <summary>
    /// Represents a W3C DateTime structure.
    /// </summary>
    /// <remarks>See http://www.w3.org/TR/NOTE-datetime for details on the W3C date time guidelines.</remarks>
    [Serializable()]
    public class W3CDateTime
    {
        //============================================================
		//	PUBLIC/PRIVATE/PROTECTED MEMBERS
		//============================================================
		#region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold the datetime in a UTC format.
        /// </summary>
        private DateTime utcDateTime;
        /// <summary>
        /// Private member to hold the UTC format timspan offest.
        /// </summary>
        private TimeSpan utcOffset;
		#endregion

		//============================================================
		//	CONSTRUCTORS
        //============================================================
        #region W3CDateTime(DateTime dateTme)
        /// <summary>
        /// Initializes a new instance of the <see cref="W3CDateTime"/> class.
        /// </summary>
        /// <param name="dateTime">The datetime to represent in a W3C format.</param>
        public W3CDateTime(DateTime dateTime)
		{
			//------------------------------------------------------------
			//	Attempt to handle class initialization
			//------------------------------------------------------------
			try
			{
				//------------------------------------------------------------
				//	Set class members
				//------------------------------------------------------------
                utcDateTime = dateTime;
                utcOffset   = TimeSpan.Zero;
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

        #region W3CDateTime(DateTime dateTme, TimeSpan offset)
        /// <summary>
        /// Initializes a new instance of the <see cref="W3CDateTime"/> class.
        /// </summary>
        /// <param name="dateTime">The datetime to represent in a W3C format.</param>
        /// <param name="offset">The UTC offest for the datetime.</param>
        public W3CDateTime(DateTime dateTime, TimeSpan offset)
		{
			//------------------------------------------------------------
			//	Attempt to handle class initialization
			//------------------------------------------------------------
			try
			{
				//------------------------------------------------------------
				//	Set class members
				//------------------------------------------------------------
                utcDateTime = dateTime;
                utcOffset   = offset;
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
        /// Gets the W3C datetime. 
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return utcDateTime + utcOffset;
            }
        }
        #endregion

        #region UtcOffset
        /// <summary>
        /// Gets the UTC offset.
        /// </summary>
        public TimeSpan UtcOffset
        {
            get
            {
                return utcOffset;
            }
        }
        #endregion

        #region UtcTime
        /// <summary>
        /// Gets the UTC datetime.
        /// </summary>
        public DateTime UtcTime
        {
            get
            {
                return utcDateTime;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Parse(string s)
        /// <summary>
        /// Converts the specified string representation of a W3C date and time to its <see cref="W3CDateTime"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <returns>A W3CDateTime equivalent to the date and time contained in s.</returns>
        public static W3CDateTime Parse(string s)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            const string w3cDateFormat  = @"^(?<year>\d\d\d\d)" + @"(-(?<month>\d\d)(-(?<day>\d\d)(T(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?" + @"(?<ofs>(Z|[+\-]\d\d:\d\d)))?)?)?$";
            TimeSpan offset             = TimeSpan.Zero;
            W3CDateTime w3cDateTime;
            Regex regularExpression;
            Match match;
            int year;
            int month;
            int day;
            int hour;
            int minute;
            int second;
            int millisecond;

            //------------------------------------------------------------
            //	Attempt to convert string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (String.IsNullOrEmpty(s))
                {
                    throw new ArgumentNullException("s");
                }

                //------------------------------------------------------------
                //	Initialize regular expression
                //------------------------------------------------------------
                regularExpression   = new Regex(w3cDateFormat);

                //------------------------------------------------------------
                //	Extract results of regular expression match
                //------------------------------------------------------------
                match               = regularExpression.Match(s);

                //------------------------------------------------------------
                //	Determine if string represents a W3C DateTime
                //------------------------------------------------------------
                if (!match.Success)
                {
                    //------------------------------------------------------------
                    //	Raise exception
                    //------------------------------------------------------------
                    throw new FormatException(Resources.ExceptionParseFormatException);
                }

                //------------------------------------------------------------
                //	Attempt to parse string
                //------------------------------------------------------------
                try
                {
                    //------------------------------------------------------------
                    //	Extract year and handle 2/3 digit years
                    //------------------------------------------------------------
                    year    = int.Parse(match.Groups["year"].Value, CultureInfo.InvariantCulture);
                    if (year < 1000)
                    {
                        if (year < 50)
                        {
                            year    = year + 2000;
                        }
                        else
                        {
                            year    = year + 1999;
                        }
                    }

                    //------------------------------------------------------------
                    //	Extract other date time parts
                    //------------------------------------------------------------
                    month       = (match.Groups["month"].Success) ? int.Parse(match.Groups["month"].Value, CultureInfo.InvariantCulture) : 1;
                    day         = match.Groups["day"].Success ? int.Parse(match.Groups["day"].Value, CultureInfo.InvariantCulture) : 1;
                    hour        = match.Groups["hour"].Success ? int.Parse(match.Groups["hour"].Value, CultureInfo.InvariantCulture) : 0;
                    minute      = match.Groups["min"].Success ? int.Parse(match.Groups["min"].Value, CultureInfo.InvariantCulture) : 0;
                    second      = match.Groups["sec"].Success ? int.Parse(match.Groups["sec"].Value, CultureInfo.InvariantCulture) : 0;
                    millisecond = match.Groups["ms"].Success ? (int)Math.Round((1000 * double.Parse(match.Groups["ms"].Value, CultureInfo.InvariantCulture))) : 0;

                    //------------------------------------------------------------
                    //	Calculate offset
                    //------------------------------------------------------------
                    if (match.Groups["ofs"].Success)
                    {
                        offset  = ParseW3cOffSet(match.Groups["ofs"].Value);
                    }

                    //------------------------------------------------------------
                    //	Generate result
                    //------------------------------------------------------------
                    w3cDateTime = new W3CDateTime(new DateTime(year, month, day, hour, minute, second, millisecond) - offset, offset);
                }
                catch(Exception exception)
                {
                    //------------------------------------------------------------
                    //	Raise exception
                    //------------------------------------------------------------
                    throw new FormatException(Resources.ExceptionParseFormatException, exception);
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return w3cDateTime;
        }
        #endregion

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
                //	Return string representation of W3C date time.
                //------------------------------------------------------------
                return (utcDateTime + utcOffset).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + FormatOffset(utcOffset, ":");
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

        #region TryParse(string date, out W3CDateTime w3cDate)
        /// <summary>
        /// Returns a value indicating if sucessfully able to parse W3C formatted datetime string.
        /// </summary>
        /// <param name="date">The W3C datetime formatted string to parse.</param>
        /// <param name="w3cDate">The <see cref="W3CDateTime"/> represented by the datetime string.</param>
        /// <returns><b>true</b> if able to parse string representation, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="date"/> is an empty string or is a null reference (Nothing in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryParse(string date, out W3CDateTime w3cDate)
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
                w3cDate         = W3CDateTime.Parse(date);
                parseSucceeded  = true;
            }
            catch(Exception)
            {
                w3cDate         = null;
                parseSucceeded  = false;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return parseSucceeded;
        }
        #endregion

        //============================================================
        //	PRIVATE ROUTINES
        //============================================================
        #region FormatOffset(TimeSpan offset, string separator)
        /// <summary>
        /// Converts the value of the specified <see cref="TimeSpan"/> to its equivalent string representation.
        /// </summary>
        /// <param name="offset">The <see cref="TimeSpan"/> to convert.</param>
        /// <param name="separator">Separator used to deliminate hours and minutes.</param>
        /// <returns>A string representation of the TimeSpan.</returns>
        private static string FormatOffset(TimeSpan offset, string separator)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            string formattedOffset  = String.Empty;

            //------------------------------------------------------------
            //	Attempt to return string representation
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Generate formatted result
                //------------------------------------------------------------
                if (offset >= TimeSpan.Zero)
                {
                    formattedOffset = "+";
                }

                formattedOffset = String.Concat(formattedOffset, offset.Hours.ToString("00", CultureInfo.InvariantCulture), separator, offset.Minutes.ToString("00", CultureInfo.InvariantCulture));
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return formattedOffset;
        }
        #endregion

        #region ParseW3cOffSet(string s)
        /// <summary>
        /// Converts the specified string representation of an offset to its <see cref="TimeSpan"/> equivalent.
        /// </summary>
        /// <param name="s">A string containing a offset to convert.</param>
        /// <returns>A TimeSpan equivalent to the offset contained in s.</returns>
        private static TimeSpan ParseW3cOffSet(string s)
        {
            //------------------------------------------------------------
            //	Attempt to parse offset string
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Return timespan offset
                //------------------------------------------------------------
                if (String.IsNullOrEmpty(s) || s == "Z")
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    if (s[0] == '+')
                    {
                        return TimeSpan.Parse(s.Substring(1));
                    }
                    else
                    {
                        return TimeSpan.Parse(s);
                    }
                }
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
    }
}
