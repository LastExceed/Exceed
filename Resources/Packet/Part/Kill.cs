using System.IO;

namespace Resources.Packet.Part {
    public class Kill {
        public long killer;
        public long victim;
        public int unknown;
        public int xp;

        public Kill() { }

        public Kill(BinaryReader reader) {
            killer = reader.ReadInt64();
            victim = reader.ReadInt64();
            unknown = reader.ReadInt32();
            xp = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(killer);
            writer.Write(victim);
            writer.Write(unknown);
            writer.Write(xp);
        }
    }
}
