using System.IO;

namespace Resources.Packet.Part {
    public class Mission {
        public int sectionX = 4096;
        public int sectionY = 4096;
        public int unknownA = 1;
        public int unknownB = 1;
        public int unknownC = 1;
        public int id = 2;
        public int unknownD = 1;
        public int monsterID = 150;
        public int level = 150;
        public byte unknownE = 1;
        public byte state = 2; //0=ready 1=progressing 2=finished
        //2padding
        public float unknownF = 100;
        public float unknownG = 100;
        public int chunkX = 32768;
        public int chunkY = 32768;

        public void Read(BinaryReader reader) {
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
            reader.ReadBytes(2);
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
            writer.Write((short)0);//padding
            writer.Write(unknownF);
            writer.Write(unknownG);
            writer.Write(chunkX);
            writer.Write(chunkY);
        }
    }
}