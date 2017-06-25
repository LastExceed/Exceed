using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Shoot {
        public LongVector Position {
            get {
                var t = new LongVector() {
                    x = BitConverter.ToInt64(data, 0),
                    y = BitConverter.ToInt64(data, 4),
                    z = BitConverter.ToInt64(data, 8)
                };
                return t;
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 0);
                BitConverter.GetBytes(value.y).CopyTo(data, 4);
                BitConverter.GetBytes(value.z).CopyTo(data, 8);
            }
        }
        public FloatVector Velocity {
            get {
                var t = new FloatVector() {
                    x = BitConverter.ToSingle(data, 12),
                    y = BitConverter.ToSingle(data, 16),
                    z = BitConverter.ToSingle(data, 20)
                };
                return t;
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 12);
                BitConverter.GetBytes(value.y).CopyTo(data, 16);
                BitConverter.GetBytes(value.z).CopyTo(data, 20);
            }
        }
        public float Scale {
            get { return BitConverter.ToSingle(data, 24); }
            set { BitConverter.GetBytes(value).CopyTo(data, 24); }
        }
        public float Particles {
            get { return BitConverter.ToSingle(data, 28); }
            set { BitConverter.GetBytes(value).CopyTo(data, 28); }
        }
        public Database.Projectile Projectile {
            get { return (Database.Projectile)data[29]; }
            set { data[29] = (byte)value; }
        }

        public byte[] data;

        public Shoot() { }

        public Shoot(byte[] data) {
            this.data = data;
        }
    }
}
