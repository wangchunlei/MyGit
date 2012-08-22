namespace NotifyClients
{
    using System;
    using System.Threading;

    using PokeIn;
    using PokeIn.Comet;

    public static class Notifier
    {
        static Thread internalThread = null;

        static Notifier()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        }

        public static void Start()
        {
            if (internalThread == null)
            {
                internalThread = new Thread(UpdateClients);
                internalThread.Start();
            }
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            if (internalThread != null)
            {
                try
                {
                    internalThread.Abort();
                }
                catch
                {
                }
            }
        }

        static void UpdateClients()
        {
            while (true)
            {
                if (CometWorker.ActiveClientCount > 0)
                {
                    CometWorker.SendToAll(JSON.Method("UpdateTime", DateTime.Now));
                }
                Thread.Sleep(500);
            }
        }
    }
}