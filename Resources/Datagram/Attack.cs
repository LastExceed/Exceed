using Resources.Utilities;
using System;

namespace Resources.Datagram {
    public class Attack : Datagram {
        public ushort Target {
            get => BitConverter.ToUInt16(data, 1);
            set => BitConverter.GetBytes(value).CopyTo(data, 1);
        }
        public float Damage {
            get => BitConverter.ToSingle(data, 3);
            set => BitConverter.GetBytes(value).CopyTo(data, 3);
        }
        public FloatVector Direction {
            get => new FloatVector() {
                x = BitConverter.ToSingle(data, 7),
                y = BitConverter.ToSingle(data, 11),
                z = BitConverter.ToSingle(data, 15)
            };
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 7);
                BitConverter.GetBytes(value.y).CopyTo(data, 11);
                BitConverter.GetBytes(value.z).CopyTo(data, 15);
            }
        }
        public int Stuntime {
            get => BitConverter.ToInt32(data, 19);
            set => BitConverter.GetBytes(value).CopyTo(data, 19);
        }
        public byte Skill {
            get => data[23];
            set => data[23] = value;
        }
        public DamageType Type {
            get => (DamageType)(data[24]);
            set => data[24] = (byte)value;
        }
        public bool ShowLight {
            get => data[25].GetBit(0);
            set => Tools.SetBit(ref data[25], value, 0);
        }
        public bool Critical {
            get => data[25].GetBit(1);
            set => Tools.SetBit(ref data[25], value, 1);
        }

        public Attack() {
            data = new byte[26];
            DatagramID = DatagramID.Attack;
        }

        public Attack(byte[] data) : base(data) { }
    }
}
