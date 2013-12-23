/*
   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.

   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.opensource.org/licenses/ms-rl

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace CodeOwls.PowerShell.Host.Utility
{
    [Flags]
    internal enum CreduiFlags
    {
        ALWAYS_SHOW_UI = 0x80,
        COMPLETE_USERNAME = 0x800,
        DO_NOT_PERSIST = 2,
        EXCLUDE_CERTIFICATES = 8,
        EXPECT_CONFIRMATION = 0x20000,
        GENERIC_CREDENTIALS = 0x40000,
        INCORRECT_PASSWORD = 1,
        KEEP_USERNAME = 0x100000,
        PASSWORD_ONLY_OK = 0x200,
        PERSIST = 0x1000,
        REQUEST_ADMINISTRATOR = 4,
        REQUIRE_CERTIFICATE = 0x10,
        REQUIRE_SMARTCARD = 0x100,
        SERVER_CREDENTIAL = 0x4000,
        SHOW_SAVE_CHECK_BOX = 0x40,
        USERNAME_TARGET_CREDENTIALS = 0x80000,
        VALIDATE_USERNAME = 0x400
    }

    internal enum CredUiReturnCodes
    {
        ERROR_CANCELLED = 0x4c7,
        ERROR_INSUFFICIENT_BUFFER = 0x7a,
        ERROR_INVALID_ACCOUNT_NAME = 0x523,
        ERROR_INVALID_FLAGS = 0x3ec,
        ERROR_INVALID_PARAMETER = 0x57,
        ERROR_NO_SUCH_LOGON_SESSION = 0x520,
        ERROR_NOT_FOUND = 0x490,
        NO_ERROR = 0
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CreduiInfo
    {
        public int cbSize;
        public IntPtr hwndParent;
        [MarshalAs(UnmanagedType.LPWStr)] public string pszMessageText;
        [MarshalAs(UnmanagedType.LPWStr)] public string pszCaptionText;
        public IntPtr hbmBanner;
    }


    internal static class NativeUtils
    {
        [DllImport("credui", EntryPoint = "CredUIPromptForCredentialsW", CharSet = CharSet.Unicode)]
        private static extern CredUiReturnCodes CredUIPromptForCredentials(ref CreduiInfo pUiInfo, string pszTargetName,
                                                                           IntPtr Reserved, int dwAuthError,
                                                                           StringBuilder pszUserName,
                                                                           int ulUserNameMaxChars,
                                                                           StringBuilder pszPassword,
                                                                           int ulPasswordMaxChars, ref int pfSave,
                                                                           CreduiFlags dwFlags);

        internal static PSCredential CredUIPromptForCredential(string caption, string message, string userName,
                                                               string targetName,
                                                               PSCredentialTypes allowedCredentialTypes,
                                                               PSCredentialUIOptions options, IntPtr parentHWND)
        {
            PSCredential credential = null;

            CreduiInfo structure = new CreduiInfo();
            structure.pszCaptionText = caption;
            structure.pszMessageText = message;
            StringBuilder pszUserName = new StringBuilder(userName, 0x201);
            StringBuilder pszPassword = new StringBuilder(0x100);
            bool flag = false;
            int pfSave = Convert.ToInt32(flag);
            structure.cbSize = Marshal.SizeOf(structure);
            structure.hwndParent = parentHWND;
            CreduiFlags dwFlags = CreduiFlags.DO_NOT_PERSIST;
            if ((allowedCredentialTypes & PSCredentialTypes.Domain) != PSCredentialTypes.Domain)
            {
                dwFlags |= CreduiFlags.GENERIC_CREDENTIALS;
                if ((options & PSCredentialUIOptions.AlwaysPrompt) == PSCredentialUIOptions.AlwaysPrompt)
                {
                    dwFlags |= CreduiFlags.ALWAYS_SHOW_UI;
                }
            }
            CredUiReturnCodes codes = CredUiReturnCodes.ERROR_INVALID_PARAMETER;
            if ((pszUserName.Length <= 0x201) && (pszPassword.Length <= 0x100))
            {
                codes = CredUIPromptForCredentials(ref structure, targetName, IntPtr.Zero, 0, pszUserName, 0x201,
                                                   pszPassword, 0x100, ref pfSave, dwFlags);
            }
            if (codes == CredUiReturnCodes.NO_ERROR)
            {
                string str = null;
                if (pszUserName != null)
                {
                    str = pszUserName.ToString();
                }
                SecureString password = new SecureString();
                for (int i = 0; i < pszPassword.Length; i++)
                {
                    password.AppendChar(pszPassword[i]);
                    pszPassword[i] = '\0';
                }
                if (!string.IsNullOrEmpty(str))
                {
                    credential = new PSCredential(str, password);
                }
                else
                {
                    credential = null;
                }
            }
            else
            {
                credential = null;
            }
            return credential;
        }
    }
}
