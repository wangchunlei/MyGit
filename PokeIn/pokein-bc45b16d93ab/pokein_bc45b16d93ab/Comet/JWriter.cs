/* 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

 * 
 * PokeIn Comet Library  
 * Copyright © 2010 Oguz Bastemur http://pokein.codeplex.com (info@pokein.com)
 */
namespace PokeIn.Comet
{
    internal class JWriter
    {
        static string _js = "";
        public static void WriteClientScript(ref System.Web.UI.Page page, string clientId, string listenUrl, string sendUrl, bool cometEnabled = true)
        {
            if (_js.Length == 0)
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                System.IO.Stream stm = asm.GetManifestResourceStream("PokeIn.Comet.PokeIn.js");
                if (stm != null)
                {
                    byte[] bt = new byte[stm.Length];
                    stm.Read(bt, 0, (System.Int32)stm.Length);
                    _js = System.Text.Encoding.UTF8.GetString(bt, 0, (System.Int32)stm.Length);
                    stm.Close();
                    _js = _js.Replace("\r\n", "");  
                    _js = _js.Replace("   ", ""); 

                    string[] obfs = new[] { "_callback_", "_Send", "ListenUrl", "SendUrl", "XMLString", "js_class", "RequestList", "ListenCounter", "RepHelper", "connector", "call_id" };
                    int counter = 0;
                    foreach (string obf in obfs)
                        _js = _js.Replace(obf, "_"+(counter++).ToString());
                }
            }

            string clientJs = _js;

            clientJs = clientJs.Replace("[$$ClientId$$]", clientId);
            clientJs = clientJs.Replace("[$$Listen$$]", listenUrl);
            clientJs = clientJs.Replace("[$$Send$$]", sendUrl);

            if (!cometEnabled)
            {
                clientJs += "\nPokeIn.CometEnabled = false;";
            }
            else
            {
                clientJs += "\nPokeIn.CometEnabled = true;";
            }

            page.Response.Write("<script>\n" + clientJs + "\n" + DynamicCode.Definitions.JSON + "</script>");
        }

        public static string CreateText(string clientId, string mess, bool @in, bool isSecure)
        {
            if (isSecure)
                return CreateText(clientId, mess, @in);

            if (@in)
            {
                mess = mess.Replace("&quot;", "&");
                mess = mess.Replace("&#92;", "\\");
                mess = mess.Replace("\n", "\\n").Replace("\r", "\\r");
            }
            else
            {
                mess = mess.Replace("\n", "\\n").Replace("\r", "\\r");
                mess = mess.Replace("\\", "&#92;");
            }
            return mess;
        }

        const string Definitions = ".(){},@? ][{};&\"'#";
        static string CreateText(string clientId, string mess, bool @in)
        {
            string clide = clientId.Substring(1, clientId.Length - 1);
            if (@in)
            {
                for (int i = 0, lmt = Definitions.Length; i < lmt; i++)
                {
                    mess = mess.Replace(":" + clide + i.ToString() + ":", Definitions[i].ToString());
                }
                mess = mess.Replace("&quot;", "&");
                mess = mess.Replace("&#92;", "\\");
                mess = mess.Replace("\n", "\\n").Replace("\r", "\\r");
            }
            else
            {
                mess = mess.Replace("\n", "\\n").Replace("\r", "\\r");
                mess = mess.Replace("\\", "&#92;");
                for (int i = 0, lmt = Definitions.Length; i < lmt; i++)
                {
                    mess = mess.Replace(Definitions[i].ToString(), ":" + clide + i.ToString() + ":");
                }
            }
            return mess;

        }
    }
}
