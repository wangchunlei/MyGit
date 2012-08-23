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
using System;
using System.Collections.Generic;

namespace PokeIn.Comet
{
    public class ClientCodeStatus 
    {
        public ClientCodeStatus(CometWorker worker)
        {
            Events = new Dictionary<string, BrowserHelper.ClientElementEventReceived>();
            LastSend = DateTime.Now;
            LastListen = DateTime.Now;
            Worker = worker;
        }
        
        public DateTime LastSend, LastListen;
        public CometWorker Worker;
        public Dictionary<string, BrowserHelper.ClientElementEventReceived> Events; 
    }
}
