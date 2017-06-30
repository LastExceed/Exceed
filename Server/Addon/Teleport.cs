using System;
using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class Teleport {
        public void TeleportPlayer(Player player, long x, long y, long z) {
            var staticEntity = new StaticEntity();
            byte[] buffer = BitConverter.GetBytes(x);
            staticEntity.chunkX = BitConverter.ToInt32(buffer, 3);
            buffer = BitConverter.GetBytes(y);
            staticEntity.chunkY = BitConverter.ToInt32(buffer, 3);
            staticEntity.id = 0;
            staticEntity.type = 18;
            staticEntity.position.x = x;
            staticEntity.position.y = y;
            staticEntity.position.z = z;
            staticEntity.rotation = 2;
            staticEntity.size.x = 0;
            staticEntity.size.y = 0;
            staticEntity.size.z = 0;
            staticEntity.closed = 0;
            staticEntity.time = 1000;
            staticEntity.unknown = 0;
            staticEntity.guid = player.entityData.guid;

            var serverUpdate10 = new ServerUpdate();
            serverUpdate10.statics.Add(staticEntity);
            serverUpdate10.Write(player.writer, true);
            staticEntity.guid = 0;
            serverUpdate10.Write(player.writer, true);
        }
    }
}
