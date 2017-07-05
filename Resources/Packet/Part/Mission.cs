using System.IO;

namespace Resources.Packet.Part {
    public class Mission {
        public int sectionX;
        public int sectionY;
        public int unknownA;
        public int unknownB;
        public int unknownC;
        public int id;
        public int unknownD;
        public int monsterID;
        public int level;
        public byte unknownE;
        public byte state; //0=ready 1=progressing 2=finished
        public short padding;
        public float unknownF;
        public float unknownG;
        public int chunkX;
        public int chunkY;

        public Mission() { }

        public Mission(BinaryReader reader) {
            sectionX = reader.ReadInt32();
            sectionY = reader.ReadInt32();
            unknownA = reader.ReadInt32();
            unknownB = reader.ReadInt32();
            unknownC = reader.ReadInt32();
            id = reader.ReadInt32();
            unknownD = reader.ReadInt32();
            monsterID = reader.ReadInt32();
            level = reader.ReadInt32();
            unknownE = reader.ReadByte();
            state = reader.ReadByte();
            padding = reader.ReadInt16();
            unknownF = reader.ReadSingle();
            unknownG = reader.ReadSingle();
            chunkX = reader.ReadInt32();
            chunkY = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(sectionX);
            writer.Write(sectionY);
            writer.Write(unknownA);
            writer.Write(unknownB);
            writer.Write(unknownC);
            writer.Write(id);
            writer.Write(unknownD);
            writer.Write(monsterID);
            writer.Write(level);
            writer.Write(unknownE);
            writer.Write(state);
            writer.Write(padding);
            writer.Write(unknownF);
            writer.Write(unknownG);
            writer.Write(chunkX);
            writer.Write(chunkY);
        }
    }
}