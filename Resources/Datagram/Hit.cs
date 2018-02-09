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
        public int Stuntime {
            get => BitConverter.ToInt32(data, 7);
            set => BitConverter.GetBytes(value).CopyTo(data, 7);
        }
        public byte Skill {
            get => data[11];
            set => data[11] = value;
        }
        public DamageType Type {
            get => (DamageType)(data[12]);
            set => data[12] = (byte)value;
        }
        public bool ShowLight {
            get => data[13].GetBit(0);
            set => Tools.SetBit(ref data[13], value, 0);
        }
        public bool Critical {
            get => data[13].GetBit(1);
            set => Tools.SetBit(ref data[13], value, 1);
        }

        public Attack() {
            data = new byte[14];
            DatagramID = DatagramID.Attack;
        }

        public Attack(byte[] data) : base(data) { }
    }
}
