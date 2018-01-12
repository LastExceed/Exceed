using Resources.Utilities;
using System;

namespace Resources.Datagram {
    public class Projectile : Datagram {
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
        public float Scale {
            get => BitConverter.ToSingle(data, 37);
            set => BitConverter.GetBytes(value).CopyTo(data, 37);
        }
        public float Particles {
            get => BitConverter.ToSingle(data, 41);
            set => BitConverter.GetBytes(value).CopyTo(data, 41);
        }
        public ProjectileType Type {
            get => (ProjectileType)data[45];
            set => data[45] = (byte)value;
        }
         
        public Projectile() {
            data = new byte[46];
            DatagramID = DatagramID.shoot;
        }

        public Projectile(byte[] data) : base(data) { }
    }
}
