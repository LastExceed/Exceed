using System.IO;

namespace Resources.Packet.Part {
    public class SkillDistribution {
        public int petmaster;
        public int petriding;
        public int sailing;
        public int climbing;
        public int hangGliding;
        public int swimming;
        public int ability1;
        public int ability2;
        public int ability3;
        public int ability4;
        public int ability5;

        public void Write(BinaryWriter writer) {
            writer.Write(petmaster);
            writer.Write(petriding);
            writer.Write(sailing);
            writer.Write(climbing);
            writer.Write(hangGliding);
            writer.Write(swimming);
            writer.Write(ability1);
            writer.Write(ability2);
            writer.Write(ability3);
            writer.Write(ability4);
            writer.Write(ability5);
        }

        public SkillDistribution() { }
        public SkillDistribution(BinaryReader reader) {
            petmaster = reader.ReadInt32();
            petriding = reader.ReadInt32();
            sailing = reader.ReadInt32();
            climbing = reader.ReadInt32();
            hangGliding = reader.ReadInt32();
            swimming = reader.ReadInt32();
            ability1 = reader.ReadInt32();
            ability2 = reader.ReadInt32();
            ability3 = reader.ReadInt32();
            ability4 = reader.ReadInt32();
            ability5 = reader.ReadInt32();
        }
    }
}
