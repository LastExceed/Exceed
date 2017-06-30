using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Packet {
    public class Login {
        public const int packetID = 255;

        public string name;

        public Login() {

        }
        public Login(BinaryReader reader) {
            byte length = reader.ReadByte();
            name = Encoding.UTF8.GetString(reader.ReadBytes(length));
            length = reader.ReadByte();
        }

        public void Write(BinaryWriter writer, bool writePacketID) {
            byte[] nBytes = Encoding.UTF8.GetBytes(name);
            byte[] pBytes = Encoding.UTF8.GetBytes(name);

            if (writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(packetID);
            writer.Write((byte)nBytes.Length);
            writer.Write(nBytes);
            writer.Write((byte)pBytes.Length);
            writer.Write(pBytes);
        }
    }
}
