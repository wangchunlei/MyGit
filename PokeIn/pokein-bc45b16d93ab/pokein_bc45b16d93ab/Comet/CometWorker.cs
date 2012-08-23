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
using System;
using System.Collections.Generic;

namespace PokeIn.Comet
{
    /// <summary>
    /// Definitions of initialized server side class list
    /// </summary>
    public delegate void DefineClassObjects(string clientId, ref Dictionary<string, object> classList);

    /// <summary>
    /// Main class to use ajax functionalities
    /// </summary>
    public class CometWorker
    {
        #region Members
        static DynamicCode _code = new DynamicCode();
        static Dictionary<string, CometMessage> _clients = new Dictionary<string, CometMessage>();
        static Dictionary<string, List<string>> _clientScriptsLog = new Dictionary<string, List<string>>();
        public static Dictionary<string, ClientCodeStatus> ClientStatus = new Dictionary<string, ClientCodeStatus>();
        static long _hClientId;

        #endregion 

        #region NewClientId
        /// <summary>
        /// Creates a unique client id.
        /// </summary>
        /// <value>a unique client id.</value>
        static string NewClientId
        {
            get
            {
                lock (_clientScriptsLog)
                {
                    if (_hClientId == 0)
                    {
                        _hClientId = DateTime.Now.ToFileTime() % 1000;
                    }

                    _hClientId++;
                    string clientId = "C" + (_hClientId).ToString(); 
                    return clientId;
                }
            }
        }
        #endregion

        #region UpdateUserTime
        static bool UpdateUserTime(string clientId, DateTime date, bool isSend)
        {
            bool hasClient;

            lock (ClientStatus)
            {
                hasClient = ClientStatus.ContainsKey(clientId);
            }

            if (hasClient)
            {
                lock (ClientStatus[clientId])
                {
                    if(isSend)
                        ClientStatus[clientId].LastSend = date;
                    else
                        ClientStatus[clientId].LastListen = date;
                }
            }

            return hasClient;
        }
        #endregion

        #region Binds
        /// <summary>
        /// Binds the specified handler URL.
        /// </summary>
        /// <param name="handlerUrl">The handler URL</param>
        /// <param name="page">Page object</param>
        /// <param name="classDefs">Class definition handler</param>
        /// <param name="clientId">The client id</param>
        /// <returns></returns>
        public static bool Bind(string handlerUrl, System.Web.UI.Page page, DefineClassObjects classDefs, out string clientId)
        {
            return Bind(handlerUrl, handlerUrl, page, classDefs, out clientId, true);
        }

        /// <summary>
        /// Binds the specified listen URL.
        /// </summary>
        /// <param name="listenUrl">The listen URL.</param>
        /// <param name="sendUrl">The send URL.</param>
        /// <param name="page">Page object</param>
        /// <param name="classDefs">Class definition handler</param>
        /// <param name="clientId">The client id</param>
        /// <returns></returns>
        public static bool Bind(string listenUrl, string sendUrl, System.Web.UI.Page page, DefineClassObjects classDefs, out string clientId)
        {
            return Bind(listenUrl, sendUrl, page, classDefs, out clientId, true); 
        }

        /// <summary>
        /// Binds the specified listen URL.
        /// </summary>
        /// <param name="listenUrl">The listen URL.</param>
        /// <param name="sendUrl">The send URL.</param>
        /// <param name="page">Page object</param>
        /// <param name="classDefs">Class definition handler</param>
        /// <param name="clientId">The client id</param>
        /// <param name="cometEnabled">if set to <c>true</c> [comet enabled].</param>
        /// <returns></returns>
        public static bool Bind(string listenUrl, string sendUrl, System.Web.UI.Page page, DefineClassObjects classDefs, out string clientId, bool cometEnabled)
        {  
            clientId = NewClientId; 

            lock (_clients)
            {
                _clients.Add(clientId, new CometMessage());
            }

            Dictionary<string, object> classList = new Dictionary<string, object>();
            classDefs(clientId, ref classList);

            bool anyAdd = false;

            lock (DynamicCode.Definitions)
            {
                foreach (KeyValuePair<string, object> en in classList)
                {
                    object ba = en.Value;
                    DynamicCode.Definitions.Add(en.Key, ref ba, clientId);
                    anyAdd = true;
                }
                object brO = new BrowserEvents(clientId);
                DynamicCode.Definitions.Add("BrowserEvents", ref brO, clientId);
                System.Reflection.FieldInfo fi = page.GetType().GetField("PokeInSafe") ;
                if (fi != null)
                {
                    object br1 = page;
                    DynamicCode.Definitions.Add("MainPage", ref br1, clientId);
                }
            }

            if (!anyAdd)
            {
                page.Response.Write("<script>alert('There is no server side class!');</script>");
                return false;
            } 

            JWriter.WriteClientScript(ref page, clientId, listenUrl, sendUrl, cometEnabled); 

            CometWorker worker = new CometWorker(clientId);

            lock (ClientStatus)
            {
                ClientStatus.Add(clientId, new ClientCodeStatus(worker));

                //Oguz Bastemur
                //to-do::smart threads through the core units
                if (cometEnabled)
                {
                    System.Threading.ThreadStart ts = new System.Threading.ThreadStart(ClientStatus[clientId].Worker.ClientThread);
                    ClientStatus[clientId].Worker._thread = new System.Threading.Thread(ts);
                    ClientStatus[clientId].Worker._thread.SetApartmentState(System.Threading.ApartmentState.MTA);
                    ClientStatus[clientId].Worker._thread.Start();
                }
            } 
             
            return true;
        }
        #endregion

        #region non-static

        string _clientId;

        System.Threading.Thread _thread;

        CometWorker(string clientId) { _clientId = clientId; _codesToRun = new List<string>(); }

        List<string> _codesToRun;

        void ClientThread()
        {
            int roundCounter = 1;
            int totalWait = 0;
            bool hasClient;
            lock (ClientStatus)
            {
                hasClient = ClientStatus.ContainsKey(_clientId);
            }

            try
            {
                while (hasClient)
                {
                    int recordCount;
                    lock (_codesToRun)
                    {
                        recordCount = _codesToRun.Count;
                    }
                    if (recordCount == 0)
                    {
                        int timer = 30 + ((roundCounter / 100) * 15);
                        totalWait += timer;
                        System.Threading.Thread.Sleep(timer);
                        roundCounter++; 

                        if (roundCounter % 30 == 0)
                        {
                            lock (ClientStatus)
                            {
                                hasClient = ClientStatus.ContainsKey(_clientId);
                            }
                            if (hasClient)
                            {
                                lock (ClientStatus[_clientId])
                                {
                                    if ( (DateTime.Now - ClientStatus[_clientId].LastListen).TotalMilliseconds > CometSettings.ConnectionLostTimeout)
                                    {
                                        hasClient = false;
                                        SendToClient(_clientId, "PokeIn.Closed();");
                                        break;
                                    }
                                }
                            }
                            if (CometSettings.ClientTimeout == 0)
                                continue; 
                            if (hasClient)
                            {
                                lock (ClientStatus[_clientId])
                                {
                                    if ((DateTime.Now - ClientStatus[_clientId].LastSend).TotalMilliseconds > CometSettings.ClientTimeout)
                                    {
                                        hasClient = false;
                                        SendToClient(_clientId, "PokeIn.Closed();");
                                        break;
                                    }
                                }
                            }
                        }

                        if (CometSettings.ClientTimeout == 0)
                            continue; 

                        if (totalWait > CometSettings.ClientTimeout)
                        {
                            SendToClient(_clientId, "PokeIn.Closed();");
                            break;
                        } 
                    }
                    else
                    {
                        totalWait = 0;
                        roundCounter = 1;
                        ExecuteJobs(_clientId);
                    }
                }
            }
            catch
            {
                hasClient = false;
            }

            try
            {
                if (totalWait > CometSettings.ClientTimeout || !hasClient)
                    RemoveClient(_clientId);
            }
            catch{}
        }

        void ExecuteJobs(string clientId)
        {
            string code = "";
            lock (_codesToRun)
            {
                int recordCount = _codesToRun.Count;

                for (int i = 0; i < recordCount; i++)
                {
                    if (i != 0)
                    {
                        code += "\r";
                    }
                    code += _codesToRun[i];
                }
                _codesToRun.Clear();
            }
            if (code.Length <= 0) return;
            if (!_code.Run(code))
            {
                SendToClient(clientId, "PokeIn.CompilerError('" + _code.ErrorMessage +"');");
            }
        }
        #endregion 

        #region Handlers
        /// <summary>
        /// Handles the specified page.
        /// </summary>
        /// <param name="page">Page object</param>
        public static void Handle(System.Web.UI.Page page)
        {
            if (!page.Request.Params.HasKeys())
                return;

            string message = page.Request.Params["ms"]; 
            if (message == null)
            {
                Listen(page);
            }
            else
            {
                Send(page);
            } 
        }

        /// <summary>
        /// Sends through the specified page object.
        /// </summary>
        /// <param name="page">Page object</param>
        public static void Send(System.Web.UI.Page page)
        {
            if (!page.Request.Params.HasKeys())
                return;

            string clientId = page.Request.Params["c"];
            if (clientId == null)
            {
                return;
            }
            
            string message = page.Request.Params["ms"];

            if (message == null)
            {
                return;
            }

            if (message.Trim().Length == 0)
            {
                return;
            }

            bool isSecure = true;
            if (page.Request.Params["sc"] != null)
            {
                bool.TryParse(page.Request.Params["sc"], out isSecure);
            }

            bool cometEnabled = true; 
            if (page.Request.Params["ce"] != null)
            {
                bool.TryParse(page.Request.Params["ce"], out cometEnabled);
            }

            bool ijStatus = false;
            if (page.Request.Params["ij"] != null)
            {
                ijStatus = page.Request.Params["ij"].ToString() == "1";
            } 

            message = JWriter.CreateText(clientId, message, true, isSecure);

            if (CometSettings.LogClientScripts)
            {
                lock (_clientScriptsLog)
                {
                    if (!_clientScriptsLog.ContainsKey(clientId))
                    {
                        _clientScriptsLog.Add(clientId, new List<string>());
                    }
                }
                lock(_clientScriptsLog[clientId])
                {
                    _clientScriptsLog[clientId].Add(message);
                }
            } 

            if ( UpdateUserTime(clientId, DateTime.Now, true) )
            { 
                if (message.Trim().StartsWith(clientId + ".CometBase.Close();"))
                {
                    RemoveClient(clientId);
                    message = JWriter.CreateText(clientId, "PokeIn.Closed();", false, isSecure);
                    if (ijStatus)
                        message = "PokeIn.CreateText('" + message + "',true);";
                    page.Response.Write(message);
                }
                else
                {
                    lock (ClientStatus[clientId])
                    {
                        if (ClientStatus[clientId].Worker == null)
                        {
                            RemoveClient(clientId);
                            message = JWriter.CreateText(clientId, "PokeIn.Closed();", false, isSecure);
                            page.Response.Write(message);
                            return;
                        }

                        ClientStatus[clientId].Worker._codesToRun.Add(message); 
                        if (!cometEnabled)
                        {
                            ClientStatus[clientId].Worker.ExecuteJobs(clientId);
                        }
                    }

                    string messages;
                    if (GrabClientMessages(clientId, out messages))
                    {
                        if (messages.Length > 0)
                        {
                            message = JWriter.CreateText(clientId, messages, false, isSecure);
                            if (ijStatus)
                                message = "PokeIn.CreateText('" + message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r") + "',true);";
                            else
                                message = message.Replace("\\\'", "\'");
                            page.Response.Write(message);
                        }
                    }
                    else if (messages == null)
                    {
                        RemoveClient(clientId);
                        message = JWriter.CreateText(clientId, "PokeIn.ClientObjectsDoesntExist();PokeIn.Closed();", false, isSecure);
                        if (ijStatus)
                            message = "PokeIn.CreateText('" + message + "',true);";
                        page.Response.Write(message);
                    }

                    page.Response.Write(" ");
                    page.Response.Flush();

                    if (!page.Response.IsClientConnected)
                    {
                        RemoveClient(clientId);
                    }
                }
            }
            else
            {
                RemoveClient(clientId);
                message = JWriter.CreateText(clientId, "PokeIn.ClientObjectsDoesntExist();PokeIn.Closed();", false, isSecure);
                if (ijStatus)
                    message = "PokeIn.CreateText('" + message + "',true);";
                page.Response.Write(message);
            } 
        } 

        /// <summary>
        /// Listens through the specified page object.
        /// </summary>
        /// <param name="page">Page object</param>
        public static void Listen(System.Web.UI.Page page)
        {
            if (!page.Request.Params.HasKeys())
                return;

            string clientId = page.Request.Params["c"];

            if (clientId == null)
            {
                return;
            }

            DateTime pageStart = DateTime.Now.AddMilliseconds(CometSettings.ListenerTimeout);

            bool isSecure = true;
            if (page.Request.Params["sc"] != null)
            {
                bool.TryParse(page.Request.Params["sc"], out isSecure);
            }

            bool ijStatus = false;
            if (page.Request.Params["ij"] != null)
            {
                ijStatus = page.Request.Params["ij"].ToString() == "1";
            }

            UpdateUserTime(clientId, pageStart, false);

            string message;
            int clientTester = 0;
 
            page.Response.Buffer = false;

            while (true)
            {
                string messages;
                if (GrabClientMessages(clientId, out messages))
                {
                    if (messages.Length > 0)
                    {
                        message = messages + "PokeIn.Listen();";
                        message = JWriter.CreateText(clientId, message, false, isSecure);

                        if (ijStatus)
                            message = "PokeIn.CreateText('" + message.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r") + "',true);";
                        else
                            message = message.Replace("\\\'", "\'");

                        page.Response.Write(message);
                        break;
                    }
                }

                if (clientTester % 25 == 0)
                {
                    page.Response.Write(" ");
                }
                clientTester++;
                
                if (messages == null || !page.Response.IsClientConnected)
                {
                    RemoveClient(clientId);
                    message = JWriter.CreateText(clientId, "PokeIn.ClientObjectsDoesntExist();PokeIn.Closed();", false, isSecure);
                    if(ijStatus)
                        message = "PokeIn.CreateText('" + message + "',true);";
                    page.Response.Write(message); 
                    break;
                }

                if (pageStart < DateTime.Now)
                {
                    message = JWriter.CreateText(clientId, "PokeIn.Listen();", false, isSecure);
                    if (ijStatus)
                        message = "PokeIn.CreateText('" + message + "',true);";
                    page.Response.Write(message); 
                    break;
                }
                System.Threading.Thread.Sleep(50);
            }

            UpdateUserTime(clientId, DateTime.Now, false);
        }
        #endregion 

        #region SendToClient
        /// <summary>
        /// Sends message to client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="message">The message.</param>
        public static void SendToClient(string clientId, string message)
        {
            bool hasClient;

            lock (ClientStatus)
            {
                hasClient = ClientStatus.ContainsKey(clientId);
            }

            if (!hasClient) return;
            lock (_clients[clientId])
            {
                _clients[clientId].PushMessage(message);
            }
        }
        #endregion

        #region SendToAll
        /// <summary>
        /// Sends message to all.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void SendToAll(string message)
        {
            foreach (string clientId in _clients.Keys)
            {
                SendToClient(clientId, message);
            }
        }
        #endregion

        #region GetClientIds
        /// <summary>
        /// Gets the active client ids.
        /// </summary>
        /// <returns></returns>
        public static string[] GetClientIds()
        {
            string[] clientIds;
            lock (_clients)
            {
               clientIds = new string[_clients.Keys.Count];
               _clients.Keys.CopyTo(clientIds, 0);
            }
            return clientIds;
        }
        #endregion

        #region SendToClients
        /// <summary>
        /// Sends message to multiple clients.
        /// </summary>
        /// <param name="clientIds">The client ids.</param>
        /// <param name="message">The message.</param>
        public static void SendToClients(string[] clientIds, string message)
        {
            for (int i = 0, lmt = clientIds.Length; i < lmt; i++)
            {
                SendToClient(clientIds[i], message);
            }
        }
        #endregion

        #region GrabClientMessages
        static bool GrabClientMessages(string clientId, out string message)
        {
            bool hasClient;

            lock (ClientStatus)
            {
                hasClient = ClientStatus.ContainsKey(clientId);
            }

            if (hasClient)
            {
                try
                {
                    lock (_clients[clientId])
                    {
                        _clients[clientId].PullMessages(out message);
                    }
                    return true;
                }
                catch{ }//Thread Differences
            } 
            message = null;
            return false;
        }
        #endregion

        #region RemoveClient
        /// <summary>
        /// Removes the client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        public static void RemoveClient(string clientId)
        {
            bool hasClient;

            System.Threading.Thread duplicate = null;

            lock (ClientStatus)
            {
                hasClient = ClientStatus.ContainsKey(clientId);

                if (hasClient)
                { 
                    try
                    {
                        ClientStatus[clientId].Worker._codesToRun.Clear();
                    }
                    catch{ }
                    try
                    {
                        ClientStatus[clientId].Events.Clear();
                    }
                    catch{ }

                    duplicate = ClientStatus[clientId].Worker._thread;

                    try
                    {
                        ClientStatus[clientId].Worker = null;
                    }
                    catch{ }

                    ClientStatus.Remove(clientId); 
                }
            }

            bool inClients;
            lock (_clients)
            {
                inClients = _clients.ContainsKey(clientId);
                if (inClients)
                {
                    _clients.Remove(clientId);
                }
            }

            if (inClients && hasClient)
            {
                lock (DynamicCode.Definitions.DefinedClasses)
                {
                    List<string> lstKeys = new List<string>();
                    lock (DynamicCode.Definitions)
                    {
                        DynamicCode.Definitions.DefinedClasses.Remove(clientId);
                        foreach (string key in DynamicCode.Definitions.ClassObjects.Keys)
                        {
                            if (key.StartsWith(clientId + "."))
                            {
                                lstKeys.Add(key);
                            }
                        }
                        foreach (string key in lstKeys)
                            DynamicCode.Definitions.ClassObjects.Remove(key);

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                    lstKeys.Clear();
                }
            }

            try
            {
                if (duplicate != null)
                {
                    duplicate.Abort();
                }
            }
            catch { }
        }
        #endregion

        #region ClientLog
        /// <summary>
        /// Container for client logs
        /// </summary>
        public class ClientLog
        {
            /// <summary>
            /// Gets the client script log.
            /// </summary>
            /// <param name="clientId">The client id.</param>
            /// <returns></returns>
            public static string[] GetClientScriptLog(string clientId)
            {
                string[] arrLog = null;
                if(CometSettings.LogClientScripts)
                {
                    bool hasClient;

                    lock (_clientScriptsLog)
                    {
                        hasClient = _clientScriptsLog.ContainsKey(clientId);
                    }

                    if (hasClient)
                    {
                        lock (_clientScriptsLog[clientId])
                        {
                            arrLog = _clientScriptsLog[clientId].ToArray();
                        }
                    }
                }

                return arrLog;
            }

            /// <summary>
            /// Clears the client script log.
            /// </summary>
            /// <param name="clientId">The client id.</param>
            public static void ClearClientScriptLog(string clientId)
            {
                if (!CometSettings.LogClientScripts) return;
                bool hasClient;

                lock (_clientScriptsLog)
                {
                    hasClient = _clientScriptsLog.ContainsKey(clientId);
                }

                if (!hasClient) return;
                lock (_clientScriptsLog[clientId])
                {
                    _clientScriptsLog[clientId].Clear();
                }
            }
        }
        #endregion
    }
}
