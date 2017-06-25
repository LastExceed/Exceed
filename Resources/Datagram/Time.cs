using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class InGameTime {
        public int Time {
            get { return BitConverter.ToInt32(data, 6); }
            set { BitConverter.GetBytes(value).CopyTo(data, 6); }
        }

        public byte[] data;

        public InGameTime() { }

        public InGameTime(byte[] data) {
            this.data = data;
        }
    }
}
