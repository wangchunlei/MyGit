using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.DAP.ADF.NotifierDeploy
{
    public class NotifierDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string SendToId { get; set; }

        public string SendFromId { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expiration { get; set; }

        public string Tag { get; set; }

        public object Data { get; set; }

        public string URI { get; set; }
    }
}
