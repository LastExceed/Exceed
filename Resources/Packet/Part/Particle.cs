using Resources.Utilities;
using System.IO;

namespace Resources.Packet.Part {
    public class Particle {
        public LongVector position;
        public FloatVector velocity;
        public FloatVector color = new FloatVector();
        public float alpha;
        public float scale;
        public int count;
        public int type;
        public float spread;
        public int unknown;

        public void Read(BinaryReader reader) {
            position.Read(reader);
            velocity.Read(reader);
            color.Read(reader);
            alpha = reader.ReadSingle();
            scale = reader.ReadSingle();
            count = reader.ReadInt32();
            type = reader.ReadInt32();
            spread = reader.ReadSingle();
            unknown = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            position.Write(writer);
            velocity.Write(writer);
            color.Write(writer);
            writer.Write(alpha);
            writer.Write(scale);
            writer.Write(count);
            writer.Write(type);
            writer.Write(spread);
            writer.Write(unknown);
        }
    }
}
