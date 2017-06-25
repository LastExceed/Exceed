using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Disconnect {
        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0);}
        }

        public byte[] data;

        public Disconnect() { }
        public Disconnect(byte[] data) {
            this.data = data;
        }
    }
}
