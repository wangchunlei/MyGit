using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.SQLite;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ReadCookie
{

    #region Menu Class

    ///
    ///  @brief Delegate defining a callback to a menu item handler.
    ///
    delegate void MenuCallback();

    ///
    ///  @brief Menu item object class.
    ///
    class Menu
    {
        private class MenuItem
        {
            public MenuCallback mc;
            public string text;

            ///
            ///  @brief The Menu class constructor.
            /// 
            ///  @param Text The text string to be displayed in the menu.
            ///  @param Mc The delegate pointer to the menu event handler.
            /// 
            public MenuItem(string Text, MenuCallback Mc)
            {
                mc = Mc;
                text = Text;
            }
        }

        private ArrayList m_Items = new ArrayList();

        ///
        ///  @brief Add a menu item to the list.
        /// 
        ///  @param Text The text string to be displayed in the menu.
        ///  @param Mc The delegate pointer to the menu event handler.
        /// 
        public void Add(string text, MenuCallback mc)
        {
            m_Items.Add(new MenuItem(text, mc));
        }

        ///
        ///  @brief Display the menu in the console and await console input.
        /// 
        public void Show()
        {
            int choosen = 0;

            for (int i = 0; i < m_Items.Count; ++i)
            {
                MenuItem mi = m_Items[i] as MenuItem;
                Console.WriteLine(" [{0}] {1}", i + 1, mi.text);
            }

            Console.WriteLine(); // add a line

            try
            {
                choosen = Int32.Parse(Console.ReadLine());
            }
            catch { /* Ignore non numeric and mixed */ }

            Console.Clear();

            if (choosen > m_Items.Count || choosen < 1)
            {
                Console.WriteLine("Invalid option.\n");
            }
            else
            {
                MenuItem mi = m_Items[choosen - 1] as MenuItem;
                MenuCallback mc = mi.mc;
                mc();
            }
        }
    }

    #endregion //Menu Class

    ///
    /// @brief      The application class.
    ///
    /// @ingroup    ReadCookie
    ///
    /// @author David MacDermot
    /// 
    /// @date 02-09-12
    /// 
    /// @todo 
    ///
    /// @bug 
    ///
    class Program
    {
        private static string _strHostName;
        private static string _strField;

        ///
        ///  @brief Menu item Exit handler.
        /// 
        private static void Exit()
        {
            Environment.Exit(0);
        }

        #region GetValue Methods

        #region Opera

         ///
        ///  @brief Get the path to the Opera cookie file.
        /// 
        ///  @return string The path if successful, otherwise an empty string
        /// 
        private static string GetOperaCookiePath()
        {
            string s = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
            s += @"\Opera\Opera\cookies4.dat";

            if (!File.Exists(s))
                return string.Empty;

            return s;
        }

        ///
        ///  @brief Get the Value from the Opera cookie file.
        /// 
        ///  @param strHost The host or website name.
        ///  @param strField The cookie field name.
        ///  @param Value a string to recieve the Field Value if any found.
        /// 
        ///  @return bool true if successful
        /// 
        private static bool GetCookie_Opera(string strHost, string strField, ref string Value)
        {
            Value = "";
            bool fRtn = false;
            string strPath;

            // Check to see if Opera Installed
            strPath = GetOperaCookiePath();
            if (string.Empty == strPath) // Nope, perhaps another browser
                return false;

            try
            {
                OpraCookieJar cookieJar = new OpraCookieJar(strPath);
                List<O4Cookie> cookies = cookieJar.GetCookies(strHost);

                if (null != cookies)
                {
                    foreach (O4Cookie cookie in cookies)
                    {
                        if (cookie.Name.ToUpper().Equals(strField.ToUpper()))
                        {
                            Value = cookie.Value;
                            fRtn = true;
                            break;
                        }
                    }
                }
             }
            catch (Exception)
            {
                Value = string.Empty;
                fRtn = false;
            }
            return fRtn;
        }

        #endregion //Opera

        #region Chrome

        ///
        ///  @brief Get the path to the Chrome cookie file.
        /// 
        ///  @return string The path if successful, otherwise an empty string
        /// 
        private static string GetChromeCookiePath()
        {
            string s = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            s += @"\Google\Chrome\User Data\Default\cookies";

            if (!File.Exists(s))
                return string.Empty;

            return s;
        }

        ///
        ///  @brief Get the Value from the Chrome cookie file.
        /// 
        ///  @param strHost The host or website name.
        ///  @param strField The cookie field name.
        ///  @param Value a string to recieve the Field Value if any found.
        /// 
        ///  @return bool true if successful
        /// 
        private static bool GetCookie_Chrome(string strHost, string strField, ref string Value)
        {
            Value = string.Empty;
            bool fRtn = false;
            string strPath, strDb;

            // Check to see if Chrome Installed
            strPath = GetChromeCookiePath();
            if (string.Empty == strPath) // Nope, perhaps another browser
                return false;

            try
            {
                strDb = "Data Source=" + strPath + ";pooling=false";

                using (SQLiteConnection conn = new SQLiteConnection(strDb))
                {
                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT value FROM cookies WHERE host_key LIKE '%" +
                            strHost + "%' AND name LIKE '%" + strField + "%';";

                        conn.Open();
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Value = reader.GetString(0);
                                if (!Value.Equals(string.Empty))
                                {
                                    fRtn = true;
                                    break;
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {
                Value = string.Empty;
                fRtn = false;
            }
            return fRtn;
        }

        #endregion //Chrome

        #region FireFox

        ///
        ///  @brief Get the path to the Fire Fox cookie file.
        /// 
        ///  @return string The path if successful, otherwise an empty string
        /// 
        private static string GetFireFoxCookiePath()
        {
            string s = Environment.GetFolderPath(
                             Environment.SpecialFolder.ApplicationData);
            s += @"\Mozilla\Firefox\Profiles\";

            try
            {
                DirectoryInfo di = new DirectoryInfo(s);
                DirectoryInfo[] dir = di.GetDirectories("*.default");
                if (dir.Length != 1)
                    return string.Empty;

                s += dir[0].Name + @"\" + "cookies.sqlite";
            }
            catch (Exception)
            {
                return string.Empty;
            }
 
            if (!File.Exists(s))
                return string.Empty;

            return s;
        }

        ///
        ///  @brief Get the Value from the Fire Fox cookie file.
        /// 
        ///  @param strHost The host or website name.
        ///  @param strField The cookie field name.
        ///  @param Value a string to recieve the Field Value if any found.
        /// 
        ///  @return bool true if successful
        /// 
        private static bool GetCookie_FireFox(string strHost, string strField, ref string Value)
        {
            Value = string.Empty;
            bool fRtn = false;
            string strPath, strTemp, strDb;
            strTemp = string.Empty;

            // Check to see if FireFox Installed
            strPath = GetFireFoxCookiePath();
            if (string.Empty == strPath) // Nope, perhaps another browser
                return false;

            try
            {
                // First copy the cookie jar so that we can read the cookies from unlocked copy while
                // FireFox is running
                strTemp = strPath + ".temp";
                strDb = "Data Source=" + strTemp + ";pooling=false";

                File.Copy(strPath, strTemp, true);

                // Now open the temporary cookie jar and extract Value from the cookie if
                // we find it.
                using (SQLiteConnection conn = new SQLiteConnection(strDb))
                {
                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT value FROM moz_cookies WHERE host LIKE '%" +
                            strHost + "%' AND name LIKE '%" + strField + "%';";

                        conn.Open();
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Value = reader.GetString(0);
                                if (!Value.Equals(string.Empty))
                                {
                                    fRtn = true;
                                    break;
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {
                Value = string.Empty;
                fRtn = false;
            }

            // All done clean up
            if (string.Empty != strTemp)
            {
                File.Delete(strTemp);
            }
            return fRtn;
        }

        #endregion //FireFox

        #region InternetExplorer

        ///
        ///  @brief Determine if the operating system is Windows 7.
        /// 
        ///  @return bool true if Windows 7 otherwise false
        /// 
        private static bool IsWindows7()
        {
            OperatingSystem osVersion = Environment.OSVersion;

            return (osVersion.Platform == PlatformID.Win32NT) &&
                (osVersion.Version.Major == 6) &&
                (osVersion.Version.Minor == 1);
        }

        ///
        ///  @brief Get the Value from the Internet Explorer cookie file.
        /// 
        ///  @param strHost The host or website name.
        ///  @param strField The cookie field name.
        ///  @param Value a string to recieve the Field Value if any found.
        /// 
        ///  @return bool true if successful
        /// 
        private static bool GetCookie_InternetExplorer(string strHost, string strField, ref string Value)
        {
            Value = string.Empty;
            bool fRtn = false;
            string strPath, strCookie;
            string[] fp;
            StreamReader r;
            int idx;

            try
            {
                strField = strField + "\n";
                strPath = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
                Version v = Environment.OSVersion.Version;

                if (IsWindows7())
                {
                    strPath += @"\low";
                }

                fp = Directory.GetFiles(strPath, "*.txt");

                foreach (string path in fp)
                {
                	idx = -1;
                    r = File.OpenText(path);
                    strCookie = r.ReadToEnd();
                    r.Close();
                    
                    if(System.Text.RegularExpressions.Regex.IsMatch(strCookie, strHost))
                    {
                    	idx = strCookie.ToUpper().IndexOf(strField.ToUpper());
                    }
                    
                    if (-1 < idx)
                    {
                        idx += strField.Length;
                        Value = strCookie.Substring(idx, strCookie.IndexOf('\n', idx) -idx);
                        if (!Value.Equals(string.Empty))
                        {
                            fRtn = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception) //File not found, etc...
            {
                Value = string.Empty;
                fRtn = false;
            }

            return fRtn;
        }

        #endregion //InternetExplorer

        #endregion //GetValue Methods

        ///
        ///  @brief Menu item Get Value all handler.
        /// 
        private static void GetCookie()
        {
            string Value = string.Empty;

            while (true)
            {
                if (GetCookie_InternetExplorer(_strHostName, _strField, ref Value)) break;
                else if (GetCookie_FireFox(_strHostName, _strField, ref Value)) break;
                else if (GetCookie_Chrome(_strHostName, _strField, ref Value)) break;
                else if (GetCookie_Opera(_strHostName, _strField, ref Value)) break;
                else { Value = string.Empty; break; }
            }
            Console.WriteLine("{0} {1} Value {2}", !Value.Equals(string.Empty) ? "Found" : "Not Found", _strField, Value);
            Console.WriteLine("\nPress any key to continue...");
            Console.Read();
        }

        ///
        ///  @brief Menu item Get Value Internet Explorer handler.
        /// 
        private static void InternetExplorer()
        {
            Console.WriteLine("Searching Internet Explorer Cookies:\n");

            string Value = string.Empty;
            bool fFound = GetCookie_InternetExplorer(_strHostName, _strField, ref Value);
            Console.WriteLine("{0} {1} Value {2}", fFound ? "Found" : "Not Found", _strField, Value);
            Console.WriteLine("\nPress any key to continue...");
            Console.Read();
        }

        ///
        ///  @brief Menu item Get Value Fire Fox handler.
        /// 
        private static void FireFox()
        {
            Console.WriteLine("Searching Firefox Cookies:\n");

            string Value = string.Empty;
            bool fFound = GetCookie_FireFox(_strHostName, _strField, ref Value);
            Console.WriteLine("{0} {1} Value {2}", fFound ? "Found" : "Not Found", _strField, Value);
            Console.WriteLine("\nPress any key to continue...");
            Console.Read();
        }

        ///
        ///  @brief Menu item Get Value Chrome handler.
        /// 
        private static void Chrome()
        {
            Console.WriteLine("Searching Chrome Cookies:\n");

            string Value = string.Empty;
            bool fFound = GetCookie_Chrome(_strHostName, _strField, ref Value);
            Console.WriteLine("{0} {1} Value {2}", fFound ? "Found" : "Not Found", _strField, Value);
            Console.WriteLine("\nPress any key to continue...");
            Console.Read();
        }

        ///
        ///  @brief Menu item Get Value Opera handler.
        /// 
        private static void Opera()
        {
            Console.WriteLine("Searching Opera Cookies:\n");

            string Value = string.Empty;
            bool fFound = GetCookie_Opera(_strHostName, _strField, ref Value);
            Console.WriteLine("{0} {1} Value {2}", fFound ? "Found" : "Not Found", _strField, Value);
            Console.WriteLine("\nPress any key to continue...");
            Console.Read();
        }

        ///
        ///  @brief The main entry point for the application.
        /// 
        static void Main(string[] args)
        {
            // Search cookies for the cntid field of the codeproject cookie
            _strHostName = "codeproject.com";
            _strField = "cntid";

            // Configure a menu of selections
            Menu menu = new Menu();
            menu.Add("Internet Explorer", new MenuCallback(InternetExplorer));
            menu.Add("Fire Fox", new MenuCallback(FireFox));
            menu.Add("Chrome", new MenuCallback(Chrome));
            menu.Add("Opera", new MenuCallback(Opera));
            menu.Add("Get Cookie field value from IE, FF, Chrome, or Opera", new MenuCallback(GetCookie)); 
            menu.Add("Exit", new MenuCallback(Exit));

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Search cookie jars for\nField: {0} of Host: {1}.\n", _strField, _strHostName);
                Console.WriteLine("Enter a number to select.\n");
                menu.Show();
            }
        }
    }
}
