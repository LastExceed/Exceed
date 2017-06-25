using System.IO;

namespace Resources.Packet.Part {
    public class Particle {
        public long posX;
        public long posY;
        public long posZ;
        public float velX;
        public float velY;
        public float velZ;
        public float red;
        public float green;
        public float blue;
        public float alpha;
        public float scale;
        public int count;
        public int type;
        public float spread;
        public int unknown;

        public void Read(BinaryReader reader) {
            posX = reader.ReadInt64();
            posY = reader.ReadInt64();
            posZ = reader.ReadInt64();
            velX = reader.ReadSingle();
            velY = reader.ReadSingle();
            velZ = reader.ReadSingle();
            red = reader.ReadSingle();
            green = reader.ReadSingle();
            blue = reader.ReadSingle();
            alpha = reader.ReadSingle();
            scale = reader.ReadSingle();
            count = reader.ReadInt32();
            type = reader.ReadInt32();
            spread = reader.ReadSingle();
            unknown = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(posX);
            writer.Write(posY);
            writer.Write(posZ);
            writer.Write(velX);
            writer.Write(velY);
            writer.Write(velZ);
            writer.Write(red);
            writer.Write(green);
            writer.Write(blue);
            writer.Write(alpha);
            writer.Write(scale);
            writer.Write(count);
            writer.Write(type);
            writer.Write(spread);
            writer.Write(unknown);
        }
    }
}
