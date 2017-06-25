using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Proc {
        public ushort Target {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0); }
        }
        public Database.ProcType Type {
            get { return (Database.ProcType)(data[2]); }
            set { data[2] = (byte)value; }
        }
        public float Modifier {
            get { return BitConverter.ToSingle(data, 3); }
            set { BitConverter.GetBytes(value).CopyTo(data, 3); }
        }
        public int Duration {
            get { return BitConverter.ToInt32(data, 7); }
            set { BitConverter.GetBytes(value).CopyTo(data, 7); }
        }

        public byte[] data;

        public Proc() { }

        public Proc(byte[] data) {
            this.data = data;
        }
    }
}
