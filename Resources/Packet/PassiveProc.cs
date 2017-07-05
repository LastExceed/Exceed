using System.IO;

namespace Resources.Packet {
    public class PassiveProc {
        public const int packetID = 8;

        public ulong source;
        public ulong target;
        public byte type;
        //3pad
        public float modifier;
        public int duration;
        public int unknown;
        public long guid3;

        public PassiveProc() { }

        public PassiveProc(BinaryReader reader) {
            source = reader.ReadUInt64();
            target = reader.ReadUInt64();
            type = reader.ReadByte();
            reader.ReadBytes(3);//pad
            modifier = reader.ReadSingle();
            duration = reader.ReadInt32();
            unknown = reader.ReadInt32();
            guid3 = reader.ReadInt64();
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(source);
            writer.Write(target);
            writer.Write(type);
            writer.Write(new byte[3]);
            writer.Write(modifier);
            writer.Write(duration);
            writer.Write(unknown);
            writer.Write(guid3);
        }
    }
}
