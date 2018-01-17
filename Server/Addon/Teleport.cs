using System;
using Resources;
using Resources.Packet;
using Resources.Utilities;

namespace Server.Addon {
    class Teleport {
        public void TeleportPlayer(Player player, long x, long y, long z) {
            var staticEntity = new ServerUpdate.StaticEntity {
                chunkX = BitConverter.ToInt32(BitConverter.GetBytes(x), 3),
                chunkY = BitConverter.ToInt32(BitConverter.GetBytes(y), 3),
                id = 0,
                type = (StaticEntityType)18,
                position = new LongVector() {
                    x = x,
                    y = y,
                    z = z,
                },
                rotation = 2,
                time = 1000,
                guid = player.entityData.guid
            };

            var serverUpdate = new ServerUpdate();
            serverUpdate.statics.Add(staticEntity);
            serverUpdate.Write(player.writer);
            staticEntity.guid = 0;
            serverUpdate.Write(player.writer);
        }
    }
}
