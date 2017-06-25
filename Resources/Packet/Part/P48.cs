using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Packet.Part {
    public class P48 //placeholder name (packet 4 struct 8)
    {
        public ulong unknownA;
        public List<SubP48> subP48s = new List<SubP48>();

        public void Read(BinaryReader reader) {
            unknownA = reader.ReadUInt64();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++) {
                SubP48 subP48 = new SubP48();
                subP48.read(reader);
                subP48s.Add(subP48);
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(unknownA);
            writer.Write(subP48s.Count);
            foreach (SubP48 subP48 in subP48s) {
                subP48.write(writer);
            }
        }
    }

    public class SubP48 {
        public ulong unknownB;
        public ulong unknownC;

        public void read(BinaryReader reader) {
            unknownB = reader.ReadUInt64();
            unknownC = reader.ReadUInt64();
        }

        public void write(BinaryWriter writer) {
            writer.Write(unknownB);
            writer.Write(unknownC);
        }
    }
}
