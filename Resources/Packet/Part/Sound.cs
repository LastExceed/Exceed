using System.IO;

namespace Resources.Packet.Part
{
    public class Sound
    {
        public int posX;
        public int posY;
        public int posZ;
        public int soundID;
        public float pitch;
        public float volume;

        public void read(BinaryReader reader)
        {
            posX = reader.ReadInt32();
            posY = reader.ReadInt32();
            posZ = reader.ReadInt32();
            soundID = reader.ReadInt32();
            pitch = reader.ReadSingle();
            volume = reader.ReadSingle();
        }

        public void write(BinaryWriter writer)
        {
            writer.Write(posX);
            writer.Write(posY);
            writer.Write(posZ);
            writer.Write(soundID);
            writer.Write(pitch);
            writer.Write(volume);
        }
    }
}
