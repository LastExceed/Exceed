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
                    x = BitConverter.ToInt64(data, 3),
                    y = BitConverter.ToInt64(data, 7),
                    z = BitConverter.ToInt64(data, 11)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 3);
                BitConverter.GetBytes(value.y).CopyTo(data, 7);
                BitConverter.GetBytes(value.z).CopyTo(data, 11);
            }
        }
        public Database.StaticUpdateType Type {
            get { return (Database.StaticUpdateType)data[2]; }
            set { data[2] = (byte)value; }
        }
        public Database.StaticRotation Direction {
            get { return (Database.StaticRotation)data[15]; }
            set { data[15] = (byte)value; }
        }
        public FloatVector Size {
            get {
                return new FloatVector() {
                    x = BitConverter.ToSingle(data, 16),
                    y = BitConverter.ToSingle(data, 20),
                    z = BitConverter.ToSingle(data, 24)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 16);
                BitConverter.GetBytes(value.y).CopyTo(data, 20);
                BitConverter.GetBytes(value.z).CopyTo(data, 24);
            }
        }
        /// <summary>
        /// for closing animation
        /// </summary>
        public ushort Time {
            get { return BitConverter.ToUInt16(data, 28); }
            set { BitConverter.GetBytes(value).CopyTo(data, 28); }
        }
        /// <summary>
        /// guid of player who interacts with it
        /// </summary>
        public ushort User {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0);}
        }

        public byte[] data;

        public StaticUpdate() { }

        public StaticUpdate(byte[] data) {
            this.data = data;
        }
    }
}
