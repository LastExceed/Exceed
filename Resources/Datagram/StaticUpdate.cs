using Resources.Utilities;
using System;

namespace Resources.Datagram {
    public class StaticUpdate : Datagram {
        public ushort Id {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public LongVector Position {
            get => new LongVector() {
                x = BitConverter.ToInt64(data, 3),
                y = BitConverter.ToInt64(data, 11),
                z = BitConverter.ToInt64(data, 19)
            };
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 3);
                BitConverter.GetBytes(value.y).CopyTo(data, 11);
                BitConverter.GetBytes(value.z).CopyTo(data, 19);
            }
        }
        public FloatVector Size {
            get => new FloatVector() {
                x = BitConverter.ToSingle(data, 27),
                y = BitConverter.ToSingle(data, 31),
                z = BitConverter.ToSingle(data, 35)
            };
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
            get => BitConverter.ToUInt16(data, 39);
            set => BitConverter.GetBytes(value).CopyTo(data, 39);
        }
        /// <summary>
        /// for closing animation
        /// </summary>
        public ushort Time {
            get => BitConverter.ToUInt16(data, 41);
            set => BitConverter.GetBytes(value).CopyTo(data, 41);
        }
        public StaticUpdateType Type {
            get => (StaticUpdateType)data[43];
            set => data[43] = (byte)value;
        }
        public StaticRotation Direction {
            get => (StaticRotation)data[44];
            set => data[44] = (byte)value;
        }
        public bool Closed {
            get => data[45].GetBit(0);
            set => Tools.SetBit(ref data[45], value, 0);
        }

        public StaticUpdate() {
            data = new byte[45];
            DatagramID = DatagramID.staticUpdate;
        }

        public StaticUpdate(byte[] data) : base(data) { }
    }
}
