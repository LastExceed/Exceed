using System.IO;

namespace Resources.Packet.Part {
    public class Multipliers {
        public float HP;
        public float attackSpeed;
        public float damge;
        public float armor;
        public float resi;

        public void read(BinaryReader reader) {
            HP = reader.ReadSingle();
            attackSpeed = reader.ReadSingle();
            damge = reader.ReadSingle();
            armor = reader.ReadSingle();
            resi = reader.ReadSingle();
        }

        public void write(BinaryWriter writer) {
            writer.Write(HP);
            writer.Write(attackSpeed);
            writer.Write(damge);
            writer.Write(armor);
            writer.Write(resi);
        }
    }
}
