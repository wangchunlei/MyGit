using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Domas.DAP.ADF.NotifierDeploy
{
    [Serializable]
    public class MessageContainer
    {
        [XmlArray("MessageCollection")]
        public Message[] MessageArray
        {
            get
            {
                return MessageCollection == null ? null : MessageCollection.ToArray();
            }
            set
            {
                MessageCollection = new List<Message>(value);
            }
        }

        [XmlIgnore]
        public List<Message> MessageCollection { get; set; }

        public DateTime PostTime { get; set; }

        public string Serialize()
        {
            return Domas.DAP.ADF.Scheme.Scheme.Serialize<MessageContainer>(this);
        }

        public static MessageContainer DeSerialize(string xml)
        {
            return Domas.DAP.ADF.Scheme.Scheme.Deserialize<MessageContainer>(xml);
        }
    }
}
