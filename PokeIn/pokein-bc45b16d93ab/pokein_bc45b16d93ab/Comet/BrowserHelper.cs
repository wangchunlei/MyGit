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
 * PokeIn Comet Library (pokein.codeplex.com)
 * Copyright © 2010 Oguz Bastemur http://pokein.codeplex.com (info@pokein.com)
 */

namespace PokeIn.Comet
{
    public class BrowserHelper
    {
        /// <summary>
        /// Delegate definition for Client Element Event Listening
        /// </summary>
        public delegate void ClientElementEventReceived(string clientId, string elementId, string eventName, string returnValue);

        /// <summary>
        /// Redirects the page.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="url">The URL.</param>
        public static void RedirectPage(string clientId, string url)
        {
            CometWorker.SendToClient(clientId, "PokeIn.Close();\nself.location='" + url + "';");
        }

        /// <summary>
        /// Sets a property of a client element
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <param name="domElementId">The DOM element id</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Element value</param>
        public static void SetElementProperty(string clientId, string domElementId, string propertyName, string value)
        {
            CometWorker.SendToClient(clientId, "document.getElementById('" + domElementId + "')."
                                                    + propertyName + "='" + value + "';");
        }

        /// <summary>
        /// Sets a server side function target for an event of client element
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="elementId">The element id.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventTarget">The event target.</param>
        /// <param name="returnValue">The return value.</param>
        public static void SetElementEvent(string clientId, string elementId, string eventName, ClientElementEventReceived eventTarget, string returnValue)
        {
            string fakeId = elementId.ToLower().Trim();
            string objectType = "document.getElementById('" + elementId + "')";
            if (fakeId == "body" || fakeId == "window" || fakeId == "document" || fakeId == "document.body")
            {
                if (fakeId == "body")
                    fakeId = "document.body";
                objectType = fakeId;
            }
            string simpleName = elementId + "_" + eventName;

            bool hasClient;
            lock (CometWorker.ClientStatus)
            {
                hasClient = CometWorker.ClientStatus.ContainsKey(clientId);
            }
            if (hasClient)
            {
                lock (CometWorker.ClientStatus[clientId])
                {
                    if (CometWorker.ClientStatus[clientId].Events.ContainsKey(simpleName))
                        CometWorker.ClientStatus[clientId].Events.Remove(simpleName);
                    CometWorker.ClientStatus[clientId].Events.Add(simpleName, eventTarget);
                }
            }

            if (returnValue.Trim().Length == 0)
            {
                returnValue = "\"\"";
            }

            CometWorker.SendToClient(clientId, @"
            document.__" + simpleName + " = function(ev){PokeIn.Send(PokeIn.GetClientId()+'.BrowserEvents.Fired("  
                         + elementId + ","+ eventName + ","+returnValue+");'); };function c3eb(){var _item = " 
                         + objectType + "; PokeIn.AddEvent(_item, '" + eventName + "', document.__" 
                                            + simpleName + ");}"+"\nc3eb();\n");
        }
         
        /// <summary>
        /// Safes the parameter. (Deprecated)
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static string SafeParameter(string message)
        {
            return message;
        }
    }
}
