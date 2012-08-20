using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ReadCookie
{
    ///
    /// @struct O4Cookie
    /// 
    /// @ingroup ReadCookie
    /// 
    /// @par Comments:
    ///         Belongs to OpraCookieJar, declared in this namespace for global
    ///          accessability.
    /// 
    /// @brief Opera 4 cookie data structure
    /// 
    public struct O4Cookie
    {
        public string Name;
        public string Value;
        public string Comment;
        public string CommentURL;
        public string RecvDomain;
        public string RecvPath;
        public string PortList;
        public byte Version;
        public bool Authenticate;
        public bool Server;
        public bool Secure;
        public bool Protected;
        public bool ThirdParty;
        public bool Password;
        public bool Prefixed;
        public ulong Expires;
        public ulong LastUsed;
    }

    //////////////////////////////////////////////////////////////////////////////
    ///
    /// @brief      Cookie handling functionality for Opera version 4 cookies.
    ///              Class provides means for parsing the cookie data,
    ///              as well as exposing all data as class objects.
    ///
    /// @ingroup    ReadCookie
    /// 
    /// @par Comments:
    ///         Initial concept borrowed from Delphi source posted to Experts Exchange by Russell Libby,
    ///         see: http://www.experts-exchange.com/Programming/Languages/Pascal/Delphi/Q_20865505.html.
    ///         This implementation only provides for reading of the cookie file.
    /// 
    /// @par References:
    ///         While this site did not provide any source code (other
    ///         than constant values) to go from, it did provide valuable
    ///         technical information in regards to the parsing of the
    ///         cookies4.dat file.
    ///         http://users.westelcom.com/jsegur/O4FE.HTM#TS1
    ///
    ///         Opera's Technical documentation can be found at:
    ///         http://www.opera.com/docs/fileformats
    /// 
    /// @author David MacDermot
    /// 
    /// @date 07-07-11
    /// 
    /// @todo   GMT Handling:  The O4Cookie exposes dates using the LongWord value
    ///          which is the GMT from 1/1/1970. Need to add utility
    ///          routines that will convert the LongWord values to
    ///          the a DateTime (local date/time).
    ///
    /// @bug 
    ///
    //////////////////////////////////////////////////////////////////////////////

    public class OpraCookieJar
    {

        private byte[] _RawBytes;
        private O4Domain _Root;

        #region Opera cookie format elements

        ////////////////////////////////////////////////////////////////////////////////
        //   Opera 4 cookie version constants
        ////////////////////////////////////////////////////////////////////////////////
        const uint O4_VER_FILE = 0x00001000;
        const uint O4_VER_APP = 0x00002001;

        ////////////////////////////////////////////////////////////////////////////////
        //   Opera 4 valid cookie header (Little Endian)
        ////////////////////////////////////////////////////////////////////////////////
        private static O4CookieHeader CookieFileHeader;

        ////////////////////////////////////////////////////////////////////////////////
        // Opera 4 cookie domain tag id constants
        ////////////////////////////////////////////////////////////////////////////////

        // Domain start tag
        const byte O4_ID_DMN = 0x01;

        // Domain element name (string).
        const byte O4_ID_DMN_NAME = 0x1E;

        // Cookie server accept flags. Content is a numerical value:
        // 1 = Accept all from domain.
        // 2 = Refuse all from domain.
        // 3 = Accept all from server.
        // 4 = Refuse all from server.
        const byte O4_ID_DMN_FILTER = 0x1F;

        // Cookie with path not matching:
        // 1 = Refuse.
        // 2 = Accept automatically.
        const byte O4_ID_DMN_MATCH = 0x21;

        // Third party cookie handling:
        // 1 = Accept all from domain.
        // 2 = Refuse all from domain.
        // 3 = Accept all from server.
        // 4 = Refuse all from server.
        const byte O4_ID_DMN_ACCEPT = 0x25;

        // End of domain flag
        const byte O4_ID_DMN_END = 0x84;

        ////////////////////////////////////////////////////////////////////////////////
        // Opera 4 cookie path tag id constants
        ////////////////////////////////////////////////////////////////////////////////

        // Path start tag
        const byte O4_ID_PATH = 0x02;

        // Path element name (string).
        const byte O4_ID_PATH_NAME = 0x1D;

        // End of path flag
        const byte O4_ID_PATH_END = 0x85;

        ////////////////////////////////////////////////////////////////////////////////
        // Opera 4 cookie tag id constants
        ////////////////////////////////////////////////////////////////////////////////

        // Cookie start tag
        const byte O4_ID_COOKIE = 0x03;

        // Cookie name (string)
        const byte O4_ID_COOKIE_NAME = 0x10;

        // Cookie value (string)
        const byte O4_ID_COOKIE_VALUE = 0x11;

        // Cookie expires (time)
        const byte O4_ID_COOKIE_EXPIRES = 0x12;

        // Cookie last used (time)
        const byte O4_ID_COOKIE_USED = 0x13;

        // Cookie2 comment (string)
        const byte O4_ID_COOKIE_DESC = 0x14;

        // Cookie2 Comment URL (string)
        const byte O4_ID_COOKIE_DESCURL = 0x15;

        // Cookie2 Received Domain (string)
        const byte O4_ID_COOKIE_RXDMN = 0x16;

        // Cookie2 Received Path (string)
        const byte O4_ID_COOKIE_RXPATH = 0x17;

        // Cookie2 Portlist (string)
        const byte O4_ID_COOKIE_PORT = 0x18;

        // Cookie secure flag (true if present)
        const byte O4_ID_COOKIE_SECURE = 0x99;

        // Cookie Version (unsigned numerical)
        const byte O4_ID_COOKIE_VER = 0x1A;

        // Cookie sent back to server that sent it (true if present)
        const byte O4_ID_COOKIE_SERVER = 0x9B;

        // Cookie protected flag   (true if present)
        const byte O4_ID_COOKIE_PROTECT = 0x9C;

        // Cookie Path prefix flag (true if present)
        const byte O4_ID_COOKIE_PREFIX = 0xA0;

        // Cookie Password Flag (true if present)
        const byte O4_ID_COOKIE_PWD = 0xA2;

        // Cookie Authenticate Flag (true if present)
        const byte O4_ID_COOKIE_AUTH = 0xA3;

        // Cookie Third party flag (true if present)
        const byte O4_ID_COOKIE_3RD = 0xA4;

        // End of cookie flag (true if present)
        const byte O4_ID_COOKIE_END = 0xA9; //undocumented - educated guess

        #endregion //Opera cookie format elements

        #region Utilities

        //////////////////////////////////////////////////////////////////////////////
        ///
        /// @brief      A utility class for swapping values between big and little endian.
        ///
        /// @ingroup    OpraCookieJar
        ///
        /// @author David MacDermot
        /// 
        /// @date 07-07-11
        /// 
        /// @todo 
        ///
        /// @bug 
        ///
        //////////////////////////////////////////////////////////////////////////////

        internal class Endian
        {
            private static readonly bool _LittleEndian;

            ///
            ///  @brief Class constructor.
            /// 
            ///  @return The static instance of the class.
            ///
            static Endian()
            {
                _LittleEndian = BitConverter.IsLittleEndian;
            }

            ///
            ///  @brief Swap a short.
            /// 
            ///  @param v The value to swap.
            /// 
            ///  @return short The swapped value.
            ///
            public static short SwapInt16(short v)
            {
                return (short)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
            }

            ///
            ///  @brief Swap an unsigned short.
            /// 
            ///  @param v The value to swap.
            /// 
            ///  @return short The swapped value.
            ///
            public static ushort SwapUInt16(ushort v)
            {
                return (ushort)(((v & 0xff) << 8) | ((v >> 8) & 0xff));
            }

            ///
            ///  @brief Swap an int.
            /// 
            ///  @param v The value to swap.
            /// 
            ///  @return short The swapped value.
            ///
            public static int SwapInt32(int v)
            {
                return (int)(((SwapInt16((short)v) & 0xffff) << 0x10) |
                              (SwapInt16((short)(v >> 0x10)) & 0xffff));
            }

            ///
            ///  @brief Swap an unsigned int.
            /// 
            ///  @param v The value to swap.
            /// 
            ///  @return short The swapped value.
            ///
            public static uint SwapUInt32(uint v)
            {
                return (uint)(((SwapUInt16((ushort)v) & 0xffff) << 0x10) |
                               (SwapUInt16((ushort)(v >> 0x10)) & 0xffff));
            }

            ///
            ///  @brief Swap a long.
            /// 
            ///  @param v The value to swap.
            /// 
            ///  @return short The swapped value.
            ///
            public static long SwapInt64(long v)
            {
                return (long)(((SwapInt32((int)v) & 0xffffffffL) << 0x20) |
                               (SwapInt32((int)(v >> 0x20)) & 0xffffffffL));
            }

            ///
            ///  @brief Swap an unsigned long.
            /// 
            ///  @param v The value to swap.
            /// 
            ///  @return short The swapped value.
            ///
            public static ulong SwapUInt64(ulong v)
            {
                return (ulong)(((SwapUInt32((uint)v) & 0xffffffffL) << 0x20) |
                                (SwapUInt32((uint)(v >> 0x20)) & 0xffffffffL));
            }

            ///
            ///  @brief returns true if operating system is Big Endian.
            ///
            public static bool IsBigEndian
            {
                get { return !_LittleEndian; }
            }

            ///
            ///  @brief returns true if operating system is Little Endian.
            ///
            public static bool IsLittleEndian
            {
                get { return _LittleEndian; }
            }
        }

        #endregion //Utilities

        #region Data Structures

        ///
        /// @struct O4CookieHeader
        /// 
        /// @ingroup OpraCookieJar
        /// 
        /// @brief Opera 4 cookie header
        ///
        private struct O4CookieHeader
        {
            public uint dwFileVer;
            public uint dwAppVer;
            public ushort wTagSize;
            public ushort wRecSize;

            ///
            ///  @brief Struct initializer.
            /// 
            ///  @return void.
            ///
            public void Initialize()
            {
                this.dwFileVer = O4_VER_FILE;
                this.dwAppVer = O4_VER_APP;
                this.wTagSize = 1;
                this.wRecSize = 2;
            }

            ///
            ///  @brief Populate struct fields from initial header bytes of cookies.dat data.
            /// 
            ///  @param bData The data bytes read from cookies.dat file.
            ///  @param idx The address of an integer with the current index position.
            /// 
            ///  @return void.
            ///
            public void Load(byte[] bData, ref int idx)
            {
                this.dwFileVer = Endian.SwapUInt32(BitConverter.ToUInt32(bData, idx));
                idx += 4;
                this.dwAppVer = Endian.SwapUInt32(BitConverter.ToUInt32(bData, idx));
                idx += 4;
                this.wTagSize = Endian.SwapUInt16(BitConverter.ToUInt16(bData, idx));
                idx += 2;
                this.wRecSize = Endian.SwapUInt16(BitConverter.ToUInt16(bData, idx));
                idx += 2;
            }

            ///
            ///  @brief Save struct field header bytes to cookies.dat data.
            /// 
            ///  @param bData The data byte buffer to be written to the cookies.dat file.
            ///  @param idx The address of an integer with the current index position.
            /// 
            ///  @return void.
            ///
            public void Save(byte[] bData, ref int idx)
            {
                BitConverter.GetBytes(Endian.SwapUInt32(this.dwFileVer)).CopyTo(bData, idx);
                idx += 4;
                BitConverter.GetBytes(Endian.SwapUInt32(this.dwAppVer)).CopyTo(bData, idx);
                idx += 4;
                BitConverter.GetBytes(Endian.SwapUInt16(this.wTagSize)).CopyTo(bData, idx);
                idx += 2;
                BitConverter.GetBytes(Endian.SwapUInt16(this.wRecSize)).CopyTo(bData, idx);
                idx += 2;
            }
        }

        ///
        /// @struct O4CookieHeader
        /// 
        /// @ingroup OpraCookieJar
        /// 
        /// @brief Opera 4 cookie domain data structure
        ///
        private struct O4Domain
        {
            public List<O4Domain> FDomains;
            public string FName;
            public byte FFilter;
            public byte FMatch;
            public byte FAccept;
            public Dictionary<string, string> FPaths;
            public List<O4Cookie> FCookies;

            ///
            ///  @brief Struct initializer overload.
            /// 
            ///  @param Name The domain name.
            /// 
            ///  @return void.
            ///
            public void initialize(string Name)
            {
                FName = Name;
                this.initialize();
            }

            ///
            ///  @brief Struct initializer.
            /// 
            ///  @return void.
            ///
            public void initialize()
            {
                FDomains = new List<O4Domain>();
                FPaths = new Dictionary<string, string>();
                FCookies = new List<O4Cookie>();
            }
        }

        #endregion //Data Structures

        #region Parser

        ///
        ///  @brief Parse file bytes into searchable memory tree.
        /// 
        ///  @param domain A pointer to an O4Domain struct.
        ///  @param b The file bytes to parse.
        ///  @param idx The address of an integer with the current index position.
        /// 
        ///  @return void.
        ///
        private void ParseCookieBytes(ref O4Domain domain, byte[] b, ref int idx)
        {
            ushort wTagLen;
            byte cbTagID;

            // Keep reading tags in
            while (idx < b.Length)
            {
                cbTagID = b[idx++];

                // Handle the tag
                switch (cbTagID)
                {
                    case O4_ID_DMN: // Sub-domain

                        // Load the domain tag size
                        wTagLen = Endian.SwapUInt16(BitConverter.ToUInt16(b, idx));
                        idx += 2;

                        // Tag indicates how many bytes we should read. We don"t go by this,
                        // because then we lose context of who the record belongs to. For( example,
                        // there may be sub-domains, cookies, etc below this domain. Due to this,
                        // we can't really break when we have read the content bytes of the domain.

                        O4Domain d = new O4Domain();
                        d.initialize();
                        ParseCookieBytes(ref d, b, ref idx);
                        domain.FDomains.Add(d);
                        break;

                    case O4_ID_DMN_NAME: // Domain name string
                        domain.FName = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_DMN_FILTER: // Domain filter value
                        domain.FFilter = this.ReadByte(b, ref idx);
                        break;

                    case O4_ID_DMN_MATCH: // Domain match value
                        domain.FMatch = this.ReadByte(b, ref idx);
                        break;

                    case O4_ID_DMN_ACCEPT: // Domain accept value
                        domain.FAccept = this.ReadByte(b, ref idx);
                        break;

                    case O4_ID_DMN_END: // End of domain record
                        return;

                    case O4_ID_PATH: // Start of path
                        break;

                    case O4_ID_PATH_NAME: // Path name string
                        domain.FPaths.Add(this.ReadString(b, ref idx), string.Empty);
                        break;

                    case O4_ID_PATH_END: // Path end
                        break; //Sub domain end

                    case O4_ID_COOKIE: // Cookie
                        AddCookie(ref domain, b, ref idx);
                        break;

                    default:
                        if (!(0 < (cbTagID & 0x80))) // this is a field tag
                        {
                            // Increment to next field
                            idx += (2 + Endian.SwapUInt16(BitConverter.ToUInt16(b, idx)));
                        }
                        // Ignore undocumented flags (already incremented one byte)
                        break;
                }
            }
        }

        ///
        ///  @brief Add cookie record to searchable memory tree.
        /// 
        ///  @param domain A pointer to an O4Domain struct (parent node).
        ///  @param b The file bytes to parse.
        ///  @param idx The address of an integer with the current index position.
        /// 
        ///  @return void.
        ///
        private void AddCookie(ref O4Domain domain, byte[] b, ref int idx)
        {
            ushort wTagLen;
            byte cbTagID;

            O4Cookie cookie = new O4Cookie();

            // Load the cookie tag size
            wTagLen = Endian.SwapUInt16(BitConverter.ToUInt16(b, idx));
            idx += 2;

            // Tag indicates how many bytes we should read.  We don't go by this,
            // because then we lose context of who the record belongs to.

            // Keep reading tags in
            while (idx < b.Length)
            {
                cbTagID = b[idx++];

                // Handle the tag
                switch (cbTagID)
                {
                    case O4_ID_COOKIE_NAME:// Cookie name
                        cookie.Name = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_VALUE:// Cookie value
                        cookie.Value = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_EXPIRES:// Cookie expires
                        cookie.Expires = this.ReadLong(b, ref idx);
                        break;

                    case O4_ID_COOKIE_USED:// Cookie last used
                        cookie.LastUsed = this.ReadLong(b, ref idx);
                        break;

                    case O4_ID_COOKIE_DESC:// Cookie2 comment
                        cookie.Comment = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_DESCURL:// Cookie2 Comment URL
                        cookie.CommentURL = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_RXDMN:// Cookie2 Received Domain
                        cookie.RecvDomain = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_RXPATH:// Cookie2 Received Path
                        cookie.RecvPath = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_PORT:// Cookie2 Portlist
                        cookie.PortList = this.ReadString(b, ref idx);
                        break;

                    case O4_ID_COOKIE_SECURE:// Cookie secure flag
                        cookie.Secure = true;
                        break;

                    case O4_ID_COOKIE_VER:// Cookie Version
                        cookie.Version = this.ReadByte(b, ref idx);
                        break;

                    case O4_ID_COOKIE_SERVER:// Cookie sent back to server that sent it
                        cookie.Server = true;
                        break;

                    case O4_ID_COOKIE_PROTECT:// Cookie protected flag
                        cookie.Protected = true;
                        break;

                    case O4_ID_COOKIE_PREFIX:// Cookie Path prefix flag
                        cookie.Prefixed = true;
                        break;

                    case O4_ID_COOKIE_PWD:// Cookie Password Flag
                        cookie.Password = true;
                        break;

                    case O4_ID_COOKIE_AUTH:// Cookie Authenticate Flag
                        cookie.Authenticate = true;
                        break;

                    case O4_ID_COOKIE_3RD:// Cookie Third party flag
                        cookie.ThirdParty = true;
                        break;

                    case O4_ID_COOKIE_END:
                        goto EXIT_WHILE;

                    default:// Handle undocumented flags and field tags
                        if (!(0 < (cbTagID & 0x80))) // this is a field tag
                        {
                            // Increment to next field
                            idx += (2 + Endian.SwapUInt16(BitConverter.ToUInt16(b, idx)));
                        }
                        // Ignore undocumented flags (already incremented one byte)
                        break;
                }
            }
        EXIT_WHILE:
            domain.FCookies.Add(cookie);
        }

        ///
        ///  @brief Read a string field from the file bytes.
        /// 
        ///  @param b The file bytes to parse.
        ///  @param idx The address of an integer with the current index position.
        /// 
        ///  @return void.
        ///
        private string ReadString(byte[] b, ref int idx)
        {
            ushort wStrLen = 0;

            // Set default
            string lpszStr = string.Empty;
            ASCIIEncoding encoding = new ASCIIEncoding();

            // Read a string value from the stream
            wStrLen = Endian.SwapUInt16(BitConverter.ToUInt16(b, idx));
            idx += 2;
            if (0 < wStrLen)
            {
                // Allocate string to read in
                lpszStr = encoding.GetString(b, idx, wStrLen);
                idx += wStrLen;
            }

            // Return string value
            return lpszStr;
        }

        ///
        ///  @brief Read an unsigned long field from the file bytes.
        /// 
        ///  @param b The file bytes to parse.
        ///  @param idx The address of an integer with the current index position.
        /// 
        ///  @return void.
        ///
        private ulong ReadLong(byte[] b, ref int idx)
        {
            ushort wIntLen = 0;

            // Set default
            ulong dwRtn = 0;

            // Read the length field value
            wIntLen = Endian.SwapUInt16(BitConverter.ToUInt16(b, idx));
            idx += 2;
            if (0 < wIntLen)
            {
                // Read single long
                dwRtn = Endian.SwapUInt64(BitConverter.ToUInt64(b, idx));
                idx += 8;
            }
            return dwRtn;
        }

        ///
        ///  @brief Read a single byte field from the file bytes.
        /// 
        ///  @param b The file bytes to parse.
        ///  @param idx The address of an integer with the current index position.
        /// 
        ///  @return void.
        ///
        private byte ReadByte(byte[] b, ref int idx)
        {
            ushort wByteLen = 0;

            // Set default
            byte bRtn = 0;

            // Read the length field value
            wByteLen = Endian.SwapUInt16(BitConverter.ToUInt16(b, idx));
            idx += 2;
            if (0 < wByteLen)
            {
                // Read single byte
                bRtn = b[idx++];
            }
            return bRtn;
        }

        #endregion //Parser

        #region Class Constructor

        ///
        ///  @brief Class constructor.
        /// 
        ///  @return An instance of the class.
        ///
        public OpraCookieJar() { }

        ///
        ///  @brief Class constructor overload.
        /// 
        ///  @param Bytes The file bytes streamed from cookies.dat
        /// 
        ///  @return An instance of the class.
        ///
        public OpraCookieJar(byte[] Bytes)
        {
            _RawBytes = Bytes;
        }

        ///
        ///  @brief Class constructor overload.
        /// 
        ///  @param FilePath The file path string for cookies.dat
        /// 
        ///  @return An instance of the class.
        ///
        public OpraCookieJar(string FilePath)
        {
            string strTemp;
            FileStream f;
            FileInfo fi;
            ASCIIEncoding encoder;
            try
            {
                byte[] bytes;

                encoder = new ASCIIEncoding();
                strTemp = FilePath + ".temp";

                // First copy the cookie jar so that we can read the cookies from unlocked copy while
                // Opera is running
                File.Copy(FilePath, strTemp, true);

                // Read Temp
                fi = new FileInfo(strTemp);
                bytes = new byte[fi.Length];
                f = fi.OpenRead();
                f.Read(bytes, 0, bytes.Length);
                f.Close();

                // Delete temp
                File.Delete(strTemp);

                //Load the class
                RawBytes = bytes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion //Class Constructor

        #region Properties

        ///
        ///  @brief Gets or sets the file bytes streamed from cookies.dat.
        ///
        public byte[] RawBytes
        {
            get { return _RawBytes; }
            set
            {
                _RawBytes = value;

                int idx = 0;
                CookieFileHeader.Load(_RawBytes, ref idx);
                _Root = new O4Domain();
                _Root.initialize("Root");
                ParseCookieBytes(ref _Root, _RawBytes, ref idx);
            }
        }

        #endregion //Properties

        ///
        ///  @brief Recursive search function employed by GetCookies.
        /// 
        ///  @param Domain The current domain to search.
        ///  @param Names The array of domain names to find.
        ///  @param iName The index of the name to reference in the search.
        ///
        ///  @return A list of O4Cookie objects if successfull, otherwise null.
        ///
        private List<O4Cookie> GetCookieRecurse(O4Domain Domain, string[] Names, int iName)
        {
            List<O4Cookie> ret = null;

            if (iName < Names.Length)
            {
                foreach (O4Domain d in Domain.FDomains)
                {
                    if (d.FName.ToUpper().Equals(Names[iName].ToUpper())) //Match, search for next domain
                    {
                        ret = GetCookieRecurse(d, Names, iName + 1);
                    }
                    else //Search the next level down for this domain
                    {
                        ret = GetCookieRecurse(d, Names, iName);
                    }

                    if (ret != null)
                        return ret;
                }
            }
            else //We incremented to the end of the domain get the cookies
            {
                if (0 < Domain.FCookies.Count)
                {
                    return Domain.FCookies;
                }
            }
            return null; // If we get here we couldn't find it
        }

        ///
        ///  @brief Search memory tree for cookies stored under the domain name of
        ///          a given server.
        /// 
        ///  @param HostName The domain name to search for.
        ///
        ///  @return A list of O4Cookie objects if successfull, otherwise null.
        ///
        public List<O4Cookie> GetCookies(string HostName)
        {
            List<O4Cookie> cookies = null;
            string[] aryNames = HostName.Split('.');
            Array.Reverse(aryNames);

            // Search for host
            cookies = GetCookieRecurse(_Root, aryNames, 0);
            return cookies;
        }
    }
}
