using System.IO;

namespace Resources.Packet.Part {
    public class Multipliers {
        public float HP;
        public float attackSpeed;
        public float damage;
        public float armor;
        public float resi;

        public void Write(BinaryWriter writer) {
            writer.Write(HP);
            writer.Write(attackSpeed);
            writer.Write(damage);
            writer.Write(armor);
            writer.Write(resi);
        }

        public Multipliers() { }
        public Multipliers(BinaryReader reader) {
            HP = reader.ReadSingle();
            attackSpeed = reader.ReadSingle();
            damage = reader.ReadSingle();
            armor = reader.ReadSingle();
            resi = reader.ReadSingle();
        }
    }
}
