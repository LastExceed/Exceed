using Resources.Utilities;
using System;
using System.Drawing;

namespace Resources.Datagram {
    public class Particle : Datagram {
        public LongVector Position {
            get => new LongVector() {
                x = BitConverter.ToInt64(data, 1),
                y = BitConverter.ToInt64(data, 9),
                z = BitConverter.ToInt64(data, 17)
            };
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 1);
                BitConverter.GetBytes(value.y).CopyTo(data, 9);
                BitConverter.GetBytes(value.z).CopyTo(data, 17);
            }
        }
        public FloatVector Velocity {
            get => new FloatVector() {
                x = BitConverter.ToSingle(data, 25),
                y = BitConverter.ToSingle(data, 29),
                z = BitConverter.ToSingle(data, 33)
            };
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 25);
                BitConverter.GetBytes(value.y).CopyTo(data, 29);
                BitConverter.GetBytes(value.z).CopyTo(data, 33);
            }
        }
        public Color Color {
            get => Color.FromArgb(data[37], data[38], data[39], data[40]);
            set {
                data[37] = value.A;
                data[38] = value.R;
                data[39] = value.G;
                data[40] = value.B;
            }
        }
        public float Size {
            get => BitConverter.ToSingle(data, 41);
            set => BitConverter.GetBytes(value).CopyTo(data, 41);
        }
        public ushort Count {
            get => BitConverter.ToUInt16(data, 45);
            set => BitConverter.GetBytes(value).CopyTo(data, 45);
        }
        public ParticleType Type {
            get => (ParticleType)data[47];
            set => data[47] = (byte)value;
        }
        public float Spread {
            get => BitConverter.ToSingle(data, 48);
            set => BitConverter.GetBytes(value).CopyTo(data, 48);
        }

        public Particle() {
            data = new byte[52];
        }

        public Particle(byte[] data) : base(data) { }
    }
}
