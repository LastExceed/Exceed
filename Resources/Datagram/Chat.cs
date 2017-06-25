using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Chat {
        public ushort Sender {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0); }
        }
        public byte Length {
            get { return data[2]; }
            set { data[2] = value; }
        }
        public string Text {
            get { return Encoding.UTF8.GetString(data, 3, Length); }
            set { Encoding.UTF8.GetBytes(value).CopyTo(data, 3); }
        }


        public byte[] data;

        public Chat() { }

        public Chat(byte[] data) {
            this.data = data;
        }
    }
}
