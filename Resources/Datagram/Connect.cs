using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Connect {
        public ushort Guid {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0);}
        }
        public int Mapseed {
            get { return BitConverter.ToInt32(data, 2);}
            set { BitConverter.GetBytes(value).CopyTo(data, 2);}
        }

        public byte[] data;

        public Connect() { }
        public Connect(byte[] data) {
            this.data = data;
        }
    }
}
