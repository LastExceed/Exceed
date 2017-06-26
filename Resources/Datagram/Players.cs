using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    public class Players {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
        }
        public byte Count {
            get { return data[1]; }
            set { data[1] = value; }
        }
        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 2); }
            set { BitConverter.GetBytes(value).CopyTo(data, 2);}
        }

        public byte[] data;

        public Players(List<Players> player) {
            //data = new byte[]
        }
        public Players(byte[] data) {
            this.data = data;
        }
    }
}
