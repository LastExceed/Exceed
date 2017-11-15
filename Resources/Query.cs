using System.Collections.Generic;
using System.IO;

namespace Resources {
    public class Query {
        public string name;
        public int slots;
        public Dictionary<ushort, string> players = new Dictionary<ushort, string>();

        public Query(string name, int slots) {
            this.name = name;
            this.slots = slots;
        }

        public Query(BinaryReader reader) {
            name = reader.ReadString();
            slots = reader.ReadInt32();
            int length = reader.ReadInt32();
            for(int i = 0; i < length; i++) {
                players.Add(reader.ReadUInt16(), reader.ReadString());
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(0);
            writer.Write(name);
            writer.Write(slots);
            writer.Write(players.Count);
            foreach(var player in players) {
                writer.Write(player.Key);
                writer.Write(player.Value);
            }
        }
    }
}
