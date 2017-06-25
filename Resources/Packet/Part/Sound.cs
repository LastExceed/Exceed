using Resources.Utilities;
using System.IO;

namespace Resources.Packet.Part {
    public class Sound {
        public IntVector position = new IntVector();
        public int soundID;
        public float pitch;
        public float volume;

        public void Read(BinaryReader reader) {
            position.Read(reader);
            soundID = reader.ReadInt32();
            pitch = reader.ReadSingle();
            volume = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer) {
            position.Write(writer);
            writer.Write(soundID);
            writer.Write(pitch);
            writer.Write(volume);
        }
    }
}
