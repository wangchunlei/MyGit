/*
 * PokeIn ASP.NET Ajax Library - MSDOS Sample
 * 
 * PokeIn 2010
 * http://pokein.com
 */

using System;
using System.Diagnostics;
using PokeIn;
using PokeIn.Comet;

namespace PokeInMSDOS
{
    public class MSDOS:IDisposable
    {
        static MSDOS()
        {
            CometSettings.ClientTimeout = 0;
        }

        public string ClientId;
        public Process msdos;
        bool disposed;

        public MSDOS(string clientID)
        {
            ClientId = clientID;
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
                                       {
                                           WindowStyle = ProcessWindowStyle.Hidden,
                                           CreateNoWindow = true,
                                           RedirectStandardError = true,
                                           RedirectStandardInput = true,
                                           RedirectStandardOutput = true,
                                           UseShellExecute = false
                                       };

            msdos = new Process {StartInfo = psi, EnableRaisingEvents = true};
            msdos.OutputDataReceived += new DataReceivedEventHandler(MsdosOutputDataReceived);
            msdos.ErrorDataReceived += new DataReceivedEventHandler(MsdosOutputDataReceived);
            msdos.Exited += new EventHandler(MsdosExited);
            msdos.Start();
            msdos.BeginOutputReadLine();
        }

        void MsdosExited(object sender, EventArgs e)
        {
            CometWorker.SendToClient(ClientId, "ConsoleClosed();"); 
        } 

        public void Dispose()
        {
            try
            {
                disposed = true;
                msdos.StandardInput.WriteLine("exit");
                msdos.WaitForExit();
                msdos.Kill();
            }catch{}
        }

        bool addNext;
        private void MsdosOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && !disposed)
            {
                if (e.Data.EndsWith(">cd"))
                {
                    addNext = true;
                    return;
                }
                string command = e.Data;
                if(addNext) // cd called
                {
                    command += ">";
                    addNext = false;
                }
                
                command = JSON.Method("ConsoleUpdated", command);
                command = command.Replace("<", "&lt;");
                command = command.Replace(">", "&gt;");
                CometWorker.SendToClient(ClientId, command);
            }
        }

        public void Run(string command)
        {
            msdos.StandardInput.WriteLine(command);
            msdos.StandardInput.WriteLine("cd");
        }
    }
}
