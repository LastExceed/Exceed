using Resources.Utilities;
using System;

namespace Resources.Datagram {
    class StaticUpdate {
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
        public FloatVector Size {
            get {
                return new FloatVector() {
                    x = BitConverter.ToSingle(data, 15),
                    y = BitConverter.ToSingle(data, 19),
                    z = BitConverter.ToSingle(data, 23)
                };
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 15);
                BitConverter.GetBytes(value.y).CopyTo(data, 19);
                BitConverter.GetBytes(value.z).CopyTo(data, 23);
            }
        }
        /// <summary>
        /// guid of player who interacts with it
        /// </summary>
        public ushort User {
            get { return BitConverter.ToUInt16(data, 27); }
            set { BitConverter.GetBytes(value).CopyTo(data, 27);}
        }
        /// <summary>
        /// for closing animation
        /// </summary>
        public ushort Time {
            get { return BitConverter.ToUInt16(data, 29); }
            set { BitConverter.GetBytes(value).CopyTo(data, 29); }
        }
        public Database.StaticUpdateType Type {
            get { return (Database.StaticUpdateType)data[31]; }
            set { data[31] = (byte)value; }
        }
        public Database.StaticRotation Direction {
            get { return (Database.StaticRotation)data[32]; }
            set { data[32] = (byte)value; }
        }
        public bool Closed {
            get { return data[33].GetBit(0); }
            set { data[33].SetBit(value, 0); }
        }
        
        public byte[] data;

        public StaticUpdate() {
            data = new byte[33];
            DatagramID = Database.DatagramID.staticUpdate;
        }

        public StaticUpdate(byte[] data) {
            this.data = data;
        }
    }
}
