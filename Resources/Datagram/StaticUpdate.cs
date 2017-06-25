using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class StaticUpdate {
        public ushort Id {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0);}
        }
        public LongVector Position {
            get {
                return new LongVector() {
                    x = BitConverter.ToInt64(data, 2),
                    y = BitConverter.ToInt64(data, 6),
                    z = BitConverter.ToInt64(data, 10)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 2);
                BitConverter.GetBytes(value.y).CopyTo(data, 6);
                BitConverter.GetBytes(value.z).CopyTo(data, 10);
            }
        }
        public FloatVector Size {
            get {
                return new FloatVector() {
                    x = BitConverter.ToSingle(data, 14),
                    y = BitConverter.ToSingle(data, 18),
                    z = BitConverter.ToSingle(data, 22)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 14);
                BitConverter.GetBytes(value.y).CopyTo(data, 18);
                BitConverter.GetBytes(value.z).CopyTo(data, 22);
            }
        }
        /// <summary>
        /// guid of player who interacts with it
        /// </summary>
        public ushort User {
            get { return BitConverter.ToUInt16(data, 26); }
            set { BitConverter.GetBytes(value).CopyTo(data, 26);}
        }
        /// <summary>
        /// for closing animation
        /// </summary>
        public ushort Time {
            get { return BitConverter.ToUInt16(data, 28); }
            set { BitConverter.GetBytes(value).CopyTo(data, 28); }
        }
        public Database.StaticUpdateType Type {
            get { return (Database.StaticUpdateType)data[30]; }
            set { data[30] = (byte)value; }
        }
        public Database.StaticRotation Direction {
            get { return (Database.StaticRotation)data[31]; }
            set { data[31] = (byte)value; }
        }
        public bool Closed {
            get { return data[32].GetBit(0); }
            set { data[32].SetBit(value, 0); }
        }
        
        public byte[] data;

        public StaticUpdate() { }

        public StaticUpdate(byte[] data) {
            this.data = data;
        }
    }
}
