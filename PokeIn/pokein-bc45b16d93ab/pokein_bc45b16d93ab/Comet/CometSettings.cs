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
    public class CometSettings
    { 
        static int _listenerTimeout = 30000;
        /// <summary>
        /// Gets or sets the Listener Timeout. (Lifetime of every listener calls - Long pooling timeout)
        /// </summary>
        /// <value>The listener timeout.</value>
        public static int ListenerTimeout
        {
            set { _listenerTimeout = value; }
            get { return _listenerTimeout; }
        }
        static int _clientTimeout = 180000; //180 secs
        /// <summary>
        /// Gets or sets the client timeout. Assign 0 to disable timeout
        /// </summary>
        /// <value>The client timeout.</value>
        public static int ClientTimeout
        {
            set { _clientTimeout = value; }
            get { return _clientTimeout; }
        }
        static int _connectionLostTimeout = 5000; //5 secs 
        /// <summary>
        /// Gets or sets the connection lost timeout. (Maximum time between the two listener calls)
        /// </summary>
        /// <value>The connection lost timeout.</value>
        public static int ConnectionLostTimeout
        {
            set { _connectionLostTimeout = value; }
            get { return _connectionLostTimeout; }
        }
        static bool _logClientScripts;
        /// <summary>
        /// Gets or sets a value indicating whether [log client scripts].
        /// </summary>
        /// <value><c>true</c> if [log client scripts]; otherwise, <c>false</c>.</value>
        public static bool LogClientScripts
        {
            set { _logClientScripts = value; }
            get { return _logClientScripts; }
        }
    }
}
