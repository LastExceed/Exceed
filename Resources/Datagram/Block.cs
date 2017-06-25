using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Block {
        public short Length {
            get { return BitConverter.ToInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0); }
        }
        //public compressed ???

        public byte[] data;
        public Block() { }

        public Block(byte[] data) {
            this.data = data;
        }
    }
}
