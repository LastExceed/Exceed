using System.Collections.Generic;
using System.IO;

namespace Resources.Packet.Part {
    public class Item {
        public byte type;
        public byte subtype;
        //2 pad
        public int modifier;
        public int unknown;
        public byte rarity;
        public byte material;
        public byte adapted; //bool?
        //1pad
        public short level;
        //2 pad
        List<Spirit> spirits = new List<Spirit>();
        public int spiritCounter;

        public void read(BinaryReader reader) {
            type = reader.ReadByte();
            subtype = reader.ReadByte();
            reader.ReadBytes(2);
            modifier = reader.ReadInt32();
            unknown = reader.ReadInt32();
            rarity = reader.ReadByte();
            material = reader.ReadByte();
            adapted = reader.ReadByte();
            reader.ReadBytes(1);
            level = reader.ReadInt16();
            reader.ReadBytes(2);

            for (int i = 0; i < 32; i++) {
                Spirit spirit = new Spirit();
                spirit.x = reader.ReadByte();
                spirit.y = reader.ReadByte();
                spirit.z = reader.ReadByte();
                spirit.type = reader.ReadByte();
                spirit.level = reader.ReadInt16();
                reader.ReadBytes(2);
                spirits.Add(spirit);
            }
            spiritCounter = reader.ReadInt32();
        }

        public void write(BinaryWriter writer) {
            writer.Write(type);
            writer.Write(subtype);
            writer.Write((short)0);
            writer.Write(modifier);
            writer.Write(unknown);
            writer.Write(rarity);
            writer.Write(material);
            writer.Write(adapted);
            writer.Write((byte)0);
            writer.Write(level);
            writer.Write((short)0);
            foreach (Spirit spirit in spirits) {
                writer.Write(spirit.x);
                writer.Write(spirit.y);
                writer.Write(spirit.z);
                writer.Write(spirit.type);
                writer.Write(spirit.level);
                writer.Write((short)0);
            }
            writer.Write(spiritCounter);
        }
    }

    public class Spirit {
        public byte x;
        public byte y;
        public byte z;
        public byte type;
        public short level;
        //2 pad

        public void read(BinaryReader reader) {
            x = reader.ReadByte();
            y = reader.ReadByte();
            z = reader.ReadByte();
            type = reader.ReadByte();
            level = reader.ReadInt16();
            reader.ReadBytes(2);
        }

        public void write(BinaryWriter writer) {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
            writer.Write(type);
            writer.Write(level);
            writer.Write((short)0);
        }
    }
}
