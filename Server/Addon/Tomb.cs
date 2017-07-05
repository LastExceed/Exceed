using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class Tomb {
        public static EntityUpdate Show(Player player) {
            return new EntityUpdate() {
                guid = player.entityData.guid + 1000,
                position = player.entityData.position,
                hostility = (byte)Database.Hostility.neutral,
                entityType = -1,
                appearance = new Appearance() {
                    character_size = new Resources.Utilities.FloatVector() {
                        x = 1, y = 1, z = 1,
                    },
                    head_model = 2155,
                    head_size = 1
                },
                HP = 100,
                name = "tombstone\0\0\0\0\0\0\0"
            };
        }

        public static EntityUpdate Hide(Player player) {
            var tomb = new EntityUpdate() {
                guid = player.entityData.guid + 1000,
                hostility = (byte)Database.Hostility.neutral,
                entityType = -1,
                HP = 0
            };
            return tomb;
        }
    }
}
