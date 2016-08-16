using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TryParseXML
{
    public class CobolUtilities
    {
        public static XmlQualifiedName ConvertPicture(string picture)
        {
            //var size = Int32.Parse(picture.Substring(1).Replace("(", "").Replace(")", ""));
            if (picture.StartsWith("X"))
            {
                return new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
            }
            else
            {
                return new XmlQualifiedName("integer", "http://www.w3.org/2001/XMLSchema");
            }
        }
    }
}