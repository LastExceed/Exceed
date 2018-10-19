using System;
using Resources;
using Resources.Utilities;
using Resources.Packet;

namespace Resources {
    class ZoxModel {
        public string Creator { get; set; }
        public byte Height { get; set; }
        public byte Width { get; set; }
        public byte Depth { get; set; }
        public byte Version { get; set; }
        public byte Frames { get; set; }
        public uint[][] Frame1 { get; set; }

        public void Parse(ServerUpdate serverUpdate, int offsetX, int offsetY, int offsetZ) {
            for (uint x = 0; x < Frame1.Length; x++) {
                byte[] colors = BitConverter.GetBytes(Frame1[x][3]); //3=red 2=green 1=blue 0=alpha
                var blockDelta = new ServerUpdate.BlockDelta() {
                    position = new IntVector() {
                        x = (int)(Frame1[x][0] + offsetX),
                        y = (int)(Frame1[x][2] + offsetY),
                        z = (int)(Frame1[x][1] + offsetZ)
                    },
                    color = new ByteVector() {
                        x = colors[3],
                        y = colors[2],
                        z = colors[1]
                    },
                    type = (colors[3] == 0 && colors[2] == 0 && colors[1] == 0xFF) ? BlockType.Liquid : BlockType.Solid,
                    unknown = 0
                };
                serverUpdate.blockDeltas.Add(blockDelta);
            }
        }
    }
}
