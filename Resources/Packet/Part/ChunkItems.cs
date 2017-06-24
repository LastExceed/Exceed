using System.Collections.Generic;
using System.IO;

namespace Resources.Packet.Part
{
    public class ChunkItems
    {
        public int chunkX;
        public int chunkY;
        public List<DroppedItem> droppedItems = new List<DroppedItem>();

        public void read(BinaryReader reader)
        {
            chunkX = reader.ReadInt32();
            chunkY = reader.ReadInt32();
            int m = reader.ReadInt32();
            for (int i = 0; i < m; i++)
            {
                DroppedItem droppedItem = new DroppedItem();
                droppedItem.read(reader);
                droppedItems.Add(droppedItem);
            }
        }

        public void write(BinaryWriter writer)
        {
            writer.Write(chunkX);
            writer.Write(chunkY);
            writer.Write(droppedItems.Count);
            foreach (DroppedItem droppedItem in droppedItems)
            {
                droppedItem.write(writer);
            }
        }
    }

    public class DroppedItem
    {
        public Item item;
        public long posX;
        public long posY;
        public long posZ;
        public float rotation;
        public float scale;
        public int unknownA; //bool?
        public int droptime;
        public int unknownB;
        public int unknownC;

        public void read(BinaryReader reader)
        {
            item = new Item(); item.read(reader);
            posX = reader.ReadInt64();
            posY = reader.ReadInt64();
            posZ = reader.ReadInt64();
            rotation = reader.ReadSingle();
            scale = reader.ReadSingle();
            unknownA = reader.ReadInt32();
            droptime = reader.ReadInt32();
            unknownB = reader.ReadInt32();
            unknownC = reader.ReadInt32();
        }

        public void write(BinaryWriter writer)
        {
            item.write(writer);
            writer.Write(posX);
            writer.Write(posY);
            writer.Write(posZ);
            writer.Write(rotation);
            writer.Write(scale);
            writer.Write(unknownA);
            writer.Write(droptime);
            writer.Write(unknownB);
            writer.Write(unknownC);
        }
    }
}
