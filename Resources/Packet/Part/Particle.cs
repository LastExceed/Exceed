using Resources.Utilities;
using System.IO;

namespace Resources.Packet.Part {
    public class Particle {
        public LongVector position;
        public FloatVector velocity;
        public FloatVector color;
        public float alpha;
        public float size;
        public int count;
        public int type;
        public float spread;
        public int unknown;

        public Particle() { }

        public Particle(BinaryReader reader) {
            position = new LongVector(reader);
            velocity = new FloatVector(reader);
            color = new FloatVector(reader);
            alpha = reader.ReadSingle();
            size = reader.ReadSingle();
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
            writer.Write(size);
            writer.Write(count);
            writer.Write(type);
            writer.Write(spread);
            writer.Write(unknown);
        }
    }
}
