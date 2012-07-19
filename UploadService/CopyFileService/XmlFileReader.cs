using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace CopyFileService
{
    public class XmlFileReader
    {
        /// <summary>
        /// 读取节点内的文本
        /// </summary>
        /// <param name="filePath">xml文件路径</param>
        /// <param name="nodePath">节点路径</param>
        /// <returns></returns>
        public static string GetNodeInnerText(string filePath, string nodePath)
        {
            string reVal = string.Empty;

            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(GetValidXmlStr(filePath));
                XmlNode node = xd.SelectSingleNode(nodePath);
                reVal = node.InnerText;
            }
            catch (Exception e)
            {
                e.Data.Add("nodePath", nodePath);
                throw;
            }

            return reVal;
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        /// <summary>
        /// 返回文件中有效的xml字符串
        /// </summary>
        /// xml合法字符：
        /// #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]  
        /// /*any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.*/
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private static string GetValidXmlStr(string filePath)
        {
            string outXmlStr = string.Empty;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                outXmlStr = sr.ReadToEnd();
            }

            return SanitizeXmlString(outXmlStr);
        }

        /// <summary>
        /// Remove illegal XML characters from a string.
        /// </summary>
        private static string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }
    }
}
