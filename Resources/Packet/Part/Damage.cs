using System.IO;

namespace Resources.Packet.Part {
    public class Damage {
        public ulong target;
        public ulong attacker;
        public float damage;
        public int unknown;

        public Damage() { }

        public Damage(BinaryReader reader) {
            target = reader.ReadUInt64();
            attacker = reader.ReadUInt64();
            damage = reader.ReadSingle();
            unknown = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(target);
            writer.Write(attacker);
            writer.Write(damage);
            writer.Write(unknown);
        }
    }
}
