using System;
using System.IO;

namespace Resources.Datagram {
    class Hit {
        public ushort Target {
            get { return BitConverter.ToUInt16(data, 0); }
            set { BitConverter.GetBytes(value).CopyTo(data, 0); }
        }
        public float Damage { 
            get { return BitConverter.ToSingle(data, 2); }
            set { BitConverter.GetBytes(value).CopyTo(data, 2); }
        }
        /// <summary>
        /// Stuntime in milliseconds
        /// </summary>
        public int StunTime {
            get { return BitConverter.ToInt32(data, 6); }
            set { BitConverter.GetBytes(value).CopyTo(data, 6); }
        }
        public UDPDatabase.Skill Skill {
            get { return (UDPDatabase.Skill)data[10]; }
            set { data[10] = (byte)value; }
        }
        public UDPDatabase.DamageType Type {
            get { return (UDPDatabase.DamageType)(data[11]); }
            set { data[11] = (byte)value; } }
        public bool ShowLight {
            get { return data[12].GetBit(0); }
            set { data[12].SetBit(value, 0); }
        }
        public bool Critical {
            get { return data[12].GetBit(1); }
            set { data[12].SetBit(value, 1); }
        }

        public byte[] data;

        public Hit() { }

        public Hit(byte[] data) {
            this.data = data;
        }
    }
}
