using System;
using System.Text;

namespace Resources.Datagram {
    class Chat {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public ushort Sender {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1); }
        }
        public byte Length {
            get { return data[3]; }
            set { data[3] = value; }
        }
        public string Text {
            get { return Encoding.UTF8.GetString(data, 4, Length); }
            set { Encoding.UTF8.GetBytes(value).CopyTo(data, 4); }
        }


        public byte[] data;
        
        public Chat(string message) {
            data = new byte[4 + message.Length];
            DatagramID = Database.DatagramID.chat;
            Length = (byte)message.Length;
            Text = message;
        }

        public Chat(byte[] data) {
            this.data = data;
        }
    }
}
