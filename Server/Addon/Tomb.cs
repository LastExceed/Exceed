using Resources;
using Resources.Packet;

namespace Server.Addon {
    class Tomb {
        public static EntityUpdate Show(Player player) {
            return new EntityUpdate() {
                guid = player.entityData.guid + 1000,
                position = player.entityData.position,
                hostility = (byte)Hostility.neutral,
                entityType = -1,
                appearance = new EntityUpdate.Appearance() {
                    character_size = new Resources.Utilities.FloatVector() {
                        x = 1, y = 1, z = 1,
                    },
                    head_model = 2155,
                    head_size = 1
                },
                HP = 100,
                name = "tombstone"
            };
        }

        public static EntityUpdate Hide(Player player) {
            var tomb = new EntityUpdate() {
                guid = player.entityData.guid + 1000,
                hostility = (byte)Hostility.neutral,
                entityType = -1,
                HP = 0
            };
            return tomb;
        }
    }
}
