using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Sitecore.Feature.Accounts.Models.HDFC_Payment_Gateway
{
    public class StringUtils
    {
        /* GetStringInBetween function to find string between two string */
        public static string[] GetStringInBetween(string strSource, string strBegin, string strEnd, bool includeBegin, bool includeEnd)
        {
            string[] result = { "", "" };
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired

                if (includeBegin)
                {
                    iIndexOfBegin -= strBegin.Length;
                }
                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {  // include the End string if desired
                    if (includeEnd)
                    { iEnd += strEnd.Length; }
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment
                    if (iEnd + strEnd.Length < strSource.Length)
                    { result[1] = strSource.Substring(iEnd + strEnd.Length); }
                }
            }
            else
            // stay where we are
            { result[1] = strSource; }
            return result;
        }//String function end

        //TODO: Currently this function is limited to one level only. Need to improve this to read any kind of XML data.
        //public static dynamic ParseXMLStream(StreamReader streamReader)
        //{
        //    XmlReaderSettings settings = new XmlReaderSettings
        //    {
        //        ConformanceLevel = ConformanceLevel.Fragment
        //    };

        //    using XmlReader reader = XmlReader.Create(streamReader, settings);

        //    var response = new ExpandoObject();

        //    string currentElementName = null;

        //    while (reader.Read())
        //    {
        //        Console.WriteLine($"NODETYPE: {reader.NodeType}, NAME: {reader.Name}, VALUE: {reader.Value}, ATTR: {reader.HasAttributes}, VAL: {reader.HasValue}");

        //        switch (reader.NodeType)
        //        {
        //            case XmlNodeType.Element: // The node is an element.
        //                currentElementName = reader.Name;
        //                while (reader.MoveToNextAttribute())
        //                {
        //                    //Console.Write(currentNode + " " + reader.Name + "='" + reader.Value + "'");
        //                }
        //                break;
        //            case XmlNodeType.Text: //Display the text in each element.
        //                response.TryAdd(currentElementName, reader.Value);
        //                break;
        //            case XmlNodeType.EndElement: //Display the end of the element.
        //                break;
        //        }
        //    }

        //    return response;
        //}
    }
}