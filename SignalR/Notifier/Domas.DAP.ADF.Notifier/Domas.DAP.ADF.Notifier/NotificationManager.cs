using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.NotifierDeploy;

namespace Domas.DAP.ADF.Notifier
{
    public static class NotificationManager
    {
        public static void Send(Message message, string notificationType = "")
        {
            if (string.IsNullOrEmpty(notificationType) || notificationType.ToLower() == "email")
            {
                var notification = new EmailNotification(message);
                notification.Send();
            }
            else if (string.IsNullOrEmpty(notificationType) || notificationType.ToLower() == "message")
            {
                //need lock
                clist.Add(message);
            }
            else if (string.IsNullOrEmpty(notificationType) || notificationType.ToLower() == "socket")
            {
                //clist.Add(message);
            }
        }
        internal static bool CheckNewMessage(string usercode, out NotifierDeploy.MessageContainer list)
        {
            list = new MessageContainer();
            list.MessageCollection = clist.Where(c => c.UserCode.ToLower() == usercode).ToList();
            // and remove
            return (list.MessageCollection == null || list.MessageCollection.Count > 0);
        }
        internal static List<Message> clist = new List<Message>();
    }

}
