using System.IO;

namespace Resources.Packet.Part
{
    public class Pickup
    {
        public ulong guid;
        public Item item;

        public void read(BinaryReader reader)
        {
            guid = reader.ReadUInt64();
            item = new Item(); item.read(reader);
        }

        public void write(BinaryWriter writer)
        {
            writer.Write(guid);
            item.write(writer);
        }
    }
}
