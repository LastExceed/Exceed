using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Datagram {
    class Particle {
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
        public Color Color {
            get { return Color.FromArgb(data[24], data[25], data[26],data[27]); }
            set {
                data[24] = value.A;
                data[25] = value.R;
                data[26] = value.G;
                data[27] = value.B;
            }
        }
        public float Size {
            get { return BitConverter.ToSingle(data, 28); }
            set { BitConverter.GetBytes(value).CopyTo(data, 28); }
        }
        public ushort Count {
            get { return BitConverter.ToUInt16(data, 32); }
            set { BitConverter.GetBytes(value).CopyTo(data, 32); }
        }
        public Database.ParticleType Type {
            get { return (Database.ParticleType)data[34]; }
            set { data[34] = (byte)value; }
        }
        public float Spread {
            get { return BitConverter.ToSingle(data, 35); }
            set { BitConverter.GetBytes(value).CopyTo(data, 35); }
        }

        public byte[] data;

        public Particle() { }

        public Particle(byte[] data) {
            this.data = data;
        }
    }
}
