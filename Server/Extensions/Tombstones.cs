using Resources;
using Resources.Datagram;
using Resources.Packet;
using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Extensions {
    public static class Tombstones {
        public static void Init() {
            ServerCore.EntityUpdated += SpawnTomb;
        }

        private static void SpawnTomb(EntityUpdate entityUpdate, Player player) {
            if (entityUpdate.HP <= 0 && (player.entity.HP > 0 || player.entity.HP == null)) {
                var tombstone = new EntityUpdate() {
                    guid = ServerCore.AssignGuid(),
                    position = entityUpdate.position ?? player.entity.position,
                    hostility = Hostility.Neutral,
                    entityType = EntityType.None,
                    appearance = new EntityUpdate.Appearance() {
                        character_size = new FloatVector() {
                            x = 1,
                            y = 1,
                            z = 1,
                        },
                        head_model = 2155,
                        head_size = 1
                    },
                    HP = 100,
                    name = "tombstone"
                };
                player.tomb = (ushort)tombstone.guid;
                ServerCore.BroadcastUDP(tombstone.CreateDatagram());
            }
            else if (player.entity.HP <= 0 && entityUpdate.HP > 0 && player.tomb != null) {
                var rde = new RemoveDynamicEntity() {
                    Guid = (ushort)player.tomb,
                };
                ServerCore.BroadcastUDP(rde.data);
            }
        }
    }
}
