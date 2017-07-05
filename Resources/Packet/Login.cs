using System.IO;
using System.Text;

namespace Resources.Packet {
    public class Login {
        public const int packetID = 255;

        public string name;

        public Login() { }

        public Login(BinaryReader reader) {
            name = Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadByte()));
            reader.ReadByte();
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            byte[] nBytes = Encoding.UTF8.GetBytes(name);
            byte[] pBytes = Encoding.UTF8.GetBytes(name);

            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write((byte)nBytes.Length);
            writer.Write(nBytes);
            writer.Write((byte)pBytes.Length);
            writer.Write(pBytes);
        }
    }
}
