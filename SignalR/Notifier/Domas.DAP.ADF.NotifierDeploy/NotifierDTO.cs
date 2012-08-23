using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.DAP.ADF.NotifierDeploy
{
    public class NotifierDTO
    {
        public string ID { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }

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

        public string Data { get; set; }

        public string URI { get; set; }
    }
}
