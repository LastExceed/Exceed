using System.IO;
using Resources.Utilities;

namespace Resources.Packet.Part {
    public class StaticEntity {
        public int chunkX;
        public int chunkY;
        public int id;
        public int paddingA;
        public int type;
        public int paddingB;
        public LongVector position;
        public int rotation;
        public FloatVector size;
        public int closed; //bool?
        public int time; //for open/close doors ect
        public int unknown;
        public int paddingC;
        public ulong guid; //of player who interacts with it

        public StaticEntity() { }

        public StaticEntity(BinaryReader reader) {
            chunkX = reader.ReadInt32();
            chunkY = reader.ReadInt32();
            id = reader.ReadInt32();
            paddingA = reader.ReadInt32();
            type = reader.ReadInt32();
            paddingB = reader.ReadInt32();
            position = new LongVector(reader);
            rotation = reader.ReadInt32();
            size = new FloatVector(reader);
            closed = reader.ReadInt32();
            time = reader.ReadInt32();
            unknown = reader.ReadInt32();
            paddingC = reader.ReadInt32();
            guid = reader.ReadUInt64();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(chunkX);
            writer.Write(chunkY);
            writer.Write(id);
            writer.Write(paddingA);
            writer.Write(type);
            writer.Write(paddingB);
            position.Write(writer);
            writer.Write(rotation);
            size.Write(writer);
            writer.Write(closed);
            writer.Write(time);
            writer.Write(unknown);
            writer.Write(paddingC);
            writer.Write(guid);
        }
    }
}
