using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.NotifierDeploy;

namespace Domas.DAP.ADF.Notifier.Models
{
    public class ClientMessage
    {
        public string MessageId { get; set; }
        public MessageStatus MessageStatus { get; set; }
        public NotifierDTO MessageData { get; set; }
        public DateTime StartTime { get; set; }
    }
    public enum MessageStatus
    {
        NotSend = 1,
        Sended = 2,
        Received = 3
    }
}
