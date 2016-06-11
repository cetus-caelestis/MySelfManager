using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyUtility
{
    partial class Utility
    {
        // http://stackoverflow.com/questions/8331119/escape-invalid-xml-characters-in-c-sharp
        static public bool IsValidXmlName(string text)
        {
            try
            {
                XmlConvert.VerifyName(text);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
