using System;

namespace BlogEngine.Core.Json
{
    /// <summary>
    /// Custom filter
    /// </summary>
    public class JsonCustomFilter
    {
        /// <summary>
        /// Short name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Long (class) name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// If filter enabled
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Number of comments checked by filter
        /// </summary>
        public int Checked { get; set; }
        /// <summary>
        /// Spam comments identified
        /// </summary>
        public int Spam { get; set; }
        /// <summary>
        /// Number of mistakes made
        /// </summary>
        public int Mistakes { get; set; }
        /// <summary>
        /// Accuracy
        /// </summary>
        public string Accuracy
        {
            get
            {
                try
                {
                    double t = double.Parse(Spam.ToString());
                    double m = double.Parse(Mistakes.ToString());

                    if (m == 0 || t == 0) return "100";

                    double a = 100 - (m / t * 100);

                    return String.Format("{0:0.00}", a);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
    }
}
