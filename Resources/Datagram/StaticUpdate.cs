using Resources.Utilities;
using System;

namespace Resources.Datagram {
    public class StaticUpdate {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public ushort Id {
            get { return BitConverter.ToUInt16(data, 1); }
            set { BitConverter.GetBytes(value).CopyTo(data, 1);}
        }
        public LongVector Position {
            get {
                return new LongVector() {
                    x = BitConverter.ToInt64(data, 3),
                    y = BitConverter.ToInt64(data, 11),
                    z = BitConverter.ToInt64(data, 19)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 3);
                BitConverter.GetBytes(value.y).CopyTo(data, 11);
                BitConverter.GetBytes(value.z).CopyTo(data, 19);
            }
        }
        public FloatVector Size {
            get {
                return new FloatVector() {
                    x = BitConverter.ToSingle(data, 27),
                    y = BitConverter.ToSingle(data, 31),
                    z = BitConverter.ToSingle(data, 35)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 27);
                BitConverter.GetBytes(value.y).CopyTo(data, 31);
                BitConverter.GetBytes(value.z).CopyTo(data, 35);
            }
        }
        /// <summary>
        /// guid of player who interacts with it
        /// </summary>
        public ushort User {
            get { return BitConverter.ToUInt16(data, 39); }
            set { BitConverter.GetBytes(value).CopyTo(data, 39);}
        }
        /// <summary>
        /// for closing animation
        /// </summary>
        public ushort Time {
            get { return BitConverter.ToUInt16(data, 41); }
            set { BitConverter.GetBytes(value).CopyTo(data, 41); }
        }
        public Database.StaticUpdateType Type {
            get { return (Database.StaticUpdateType)data[43]; }
            set { data[43] = (byte)value; }
        }
        public Database.StaticRotation Direction {
            get { return (Database.StaticRotation)data[44]; }
            set { data[44] = (byte)value; }
        }
        public bool Closed {
            get { return data[45].GetBit(0); }
            set { Tools.SetBit(ref data[45], value, 0); }
        }
        
        public byte[] data;

        public StaticUpdate() {
            data = new byte[45];
            DatagramID = Database.DatagramID.staticUpdate;
        }

        public StaticUpdate(byte[] data) {
            this.data = data;
        }
    }
}
