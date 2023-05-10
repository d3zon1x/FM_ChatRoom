using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpansionForCAndS
{
    public enum MessageType { Private, Public, Exception, Disconnect, Update, SetNick};
    public class MessageInfo
    {
        public MessageType Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
        public DateTime TimeSend { get; set; }
        public string _time => TimeSend.ToShortTimeString();

        public MessageInfo(MessageType type, string from = ".", string to = ".", string message = ".")
        {
            Type = type;
            From = from;
            To = to;
            Message = message;
            TimeSend = DateTime.Now;
        }

        public override string ToString()
        {
            return $"type:{this.Type}, {this.From} -> {this.To}, Message : {this.Message}, Time:{this.TimeSend.ToLongTimeString()}";
        }
    }
}
