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
    internal class BrowserEvents
    {
        string _clientId;
        public BrowserEvents(string clientId)
        {
            _clientId = clientId;
        }
        public void Fired(string elementId, string eventName, string returnValue)
        {
            string simpleName = elementId + "_" + eventName;

            bool hasClient;
            lock (CometWorker.ClientStatus)
            {
                hasClient = CometWorker.ClientStatus.ContainsKey(_clientId);
            }
            if (!hasClient) return;
            lock (CometWorker.ClientStatus[_clientId])
            {
                if (CometWorker.ClientStatus[_clientId].Events.ContainsKey(simpleName))
                {
                    CometWorker.ClientStatus[_clientId].Events[simpleName](_clientId, elementId, eventName, returnValue);
                }
            }
        }
    }
}
