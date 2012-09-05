using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Domas.DAP.ADF.Notifier.Models;
using Domas.DAP.ADF.Notifier.Services;
using Domas.DAP.ADF.NotifierDeploy;
using SignalR.Hubs;
using SignalR;

namespace Domas.DAP.ADF.Notifier
{
    public class NotifierServer : Hub, IDisconnect, IConnected
    {
        private static System.Timers.Timer aTimer;
        private static LogManager.ILogger logger = LogManager.LogManager.GetLogger("NotifierServer");
        private static INotifierRepository _repository = new MemoryRepository();
        static NotifierServer()
        {
            aTimer = new Timer(60000);
            aTimer.Elapsed += new ElapsedEventHandler(aTimer_Elapsed);
            aTimer.Enabled = true;
            aTimer.Start();
        }

        static void aTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeOutMessage = _repository.Messages.Where(m => DateTime.Now.CompareTo(m.StartTime.AddDays(1)) > 0);
            foreach (var message in timeOutMessage)
            {
                _repository.Remove(message);
            }
        }
        public static void Send(NotifierDeploy.NotifierDTO message, string userAccount, bool isReSend = false)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotifierServer>();
            var loginId = Domas.DAP.ADF.Context.ContextFactory.GetCurrentContext().LoginID;

            if (!isReSend)
            {
                var messageId = Guid.NewGuid().ToString();
                message.MessageId = messageId;
                message.SendToId = userAccount;
                _repository.Add(new ClientMessage()
                {
                    MessageId = messageId,
                    MessageStatus = MessageStatus.Sended,
                    MessageData = message,
                    StartTime = DateTime.Now
                });
            }
            context.Clients[userAccount].addMessage(message);



            logger.Debug(string.Format("客户端:{0}，连接号:{1}发送消息成功", loginId, userAccount));

        }
        public void ServerCallback(string messageId)
        {
            var loginId = Domas.DAP.ADF.Context.ContextFactory.GetCurrentContext().LoginID;
            logger.Debug(string.Format("客户端:{0}，任务号:{1}回调成功", loginId, messageId));
            var message = _repository.Messages.FirstOrDefault(m => m.MessageId == messageId);
            if (message != null)
            {
                message.MessageStatus = MessageStatus.Received;
                _repository.Remove(message);
                logger.Debug(string.Format("客户端:{0}，任务号:{1}删除成功", loginId, messageId));
            }
        }
        public Task Disconnect()
        {
            Clients.leave(Context.ConnectionId, DateTime.Now.ToString());

            logger.Debug(string.Format("客户端:{0}退出连接成功", Context.ConnectionId));
            return null;
        }

        public Task Connect()
        {
            var loginId = Domas.DAP.ADF.Context.ContextFactory.GetCurrentContext().LoginID;

            Clients.joined(Context.ConnectionId, DateTime.Now.ToString());

            var messages = _repository.Messages.Where(m => m.MessageData.SendToId == loginId && m.MessageStatus == MessageStatus.Sended);
            foreach (var message in messages)
            {
                Send(message.MessageData, loginId, true);
            }
            logger.Debug(string.Format("客户端:{0}连接成功，连接号:{1}", loginId, Context.ConnectionId));

            return null;
        }

        public Task Reconnect(IEnumerable<string> groups)
        {
            var loginId = Domas.DAP.ADF.Context.ContextFactory.GetCurrentContext().LoginID;
            Clients.rejoined(Context.ConnectionId, DateTime.Now.ToString());
            logger.Debug(string.Format("客户端:{0}重新连接成功，连接号:{1}", loginId, Context.ConnectionId));
            return null;
        }
    }
}
