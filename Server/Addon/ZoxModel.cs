using System;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class ZoxModel {
        public string creator { get; set; }
        public byte height { get; set; }
        public byte width { get; set; }
        public byte depth { get; set; }
        public byte version { get; set; }
        public byte frames { get; set; }
        public uint[][] frame1 { get; set; }

        public void parse(ServerUpdate serverUpdate, int offsetX, int offsetY, int offsetZ) {
            for (uint x = 0; x < frame1.Length; x++) {
                byte[] colors = BitConverter.GetBytes(frame1[x][3]); //3=red 2=green 1=blue 0=alpha
                var blockDelta = new BlockDelta();
                blockDelta.posX = (int)(frame1[x][0] + offsetX);
                blockDelta.posY = (int)(frame1[x][2] + offsetY);
                blockDelta.posZ = (int)(frame1[x][1] + offsetZ);
                blockDelta.red = colors[3];
                blockDelta.green = colors[2];
                blockDelta.blue = colors[1];
                blockDelta.type = (byte)((colors[3] == 0 && colors[2] == 0 && colors[1] == 0xFF) ? 2 : 1);
                blockDelta.unknown = 0;
                serverUpdate.blockDeltas.Add(blockDelta);
            }
        }
    }
}
