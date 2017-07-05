using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Packet.Part {
    /// <summary>
    /// placeholder name (packet 4 struct 8)
    /// </summary>
    public class P48 {
        public ulong unknownA;
        public List<SubP48> subP48s = new List<SubP48>();

        public P48(BinaryReader reader) {
            unknownA = reader.ReadUInt64();
            int count = reader.ReadInt32();
            for(int i = 0; i < count; i++) {
                subP48s.Add(new SubP48(reader));
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(unknownA);
            writer.Write(subP48s.Count);
            foreach(SubP48 subP48 in subP48s) {
                subP48.Write(writer);
            }
        }
    }

    public class SubP48 {
        public ulong unknownB;
        public ulong unknownC;

        public SubP48(BinaryReader reader) {
            unknownB = reader.ReadUInt64();
            unknownC = reader.ReadUInt64();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(unknownB);
            writer.Write(unknownC);
        }
    }
}
