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
 * Copyright © 2010 http://pokein.codeplex.com (info@pokein.com)
 */
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PokeInServerWatch
{
    public class Dummy
    {
        string ClientId;
        System.Threading.Thread ThTime;
        bool run;

        public Dummy(string clientId)
        {
            ClientId = clientId;
            run = false;
            ThTime = new System.Threading.Thread(new System.Threading.ThreadStart(updateTime));
        }

        ~Dummy()
        {
            run = false;
            ThTime.Abort();
        }
        
        private void updateTime()
        {
            while (run)
            {
                PokeIn.Comet.CometWorker.SendToClient(ClientId, "UpdateTime('"+DateTime.Now.ToLongTimeString()+"');");
                System.Threading.Thread.Sleep(1000);
            }
        }
        public void RunTimer()
        {
            run = true;
            ThTime.Start();
        }

    }
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public static void classDefiner(string clientId, ref Dictionary<string, object> classList)
        {
            classList.Add("Dummy", new Dummy(clientId));
        }
    }
}