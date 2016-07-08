using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MasterBox
{
    public class SpecChar
    {
        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty spaces.
            try
            {
                return Regex.Replace(strIn, @"[^A-Za-z0-9]+", " ",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
    }
}