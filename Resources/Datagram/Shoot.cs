using Resources.Utilities;
using System;

namespace Resources.Datagram {
    public class Shoot {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }
        public LongVector Position {
            get {
                var t = new LongVector() {
                    x = BitConverter.ToInt64(data, 1),
                    y = BitConverter.ToInt64(data, 9),
                    z = BitConverter.ToInt64(data, 17)
                };
                return t;
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 1);
                BitConverter.GetBytes(value.y).CopyTo(data, 9);
                BitConverter.GetBytes(value.z).CopyTo(data, 17);
            }
        }
        public FloatVector Velocity {
            get {
                var t = new FloatVector() {
                    x = BitConverter.ToSingle(data, 25),
                    y = BitConverter.ToSingle(data, 29),
                    z = BitConverter.ToSingle(data, 33)
                };
                return t;
            }
            set {
                BitConverter.GetBytes(value.x).CopyTo(data, 25);
                BitConverter.GetBytes(value.y).CopyTo(data, 29);
                BitConverter.GetBytes(value.z).CopyTo(data, 33);
            }
        }
        public float Scale {
            get { return BitConverter.ToSingle(data, 37); }
            set { BitConverter.GetBytes(value).CopyTo(data, 37); }
        }
        public float Particles {
            get { return BitConverter.ToSingle(data, 41); }
            set { BitConverter.GetBytes(value).CopyTo(data, 41); }
        }
        public Database.Projectile Projectile {
            get { return (Database.Projectile)data[45]; }
            set { data[45] = (byte)value; }
        }

        public byte[] data;
         
        public Shoot() {
            data = new byte[46];
            DatagramID = Database.DatagramID.shoot;
        }

        public Shoot(byte[] data) {
            this.data = data;
        }
    }
}
