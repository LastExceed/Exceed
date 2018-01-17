using Resources;
using Resources.Packet;
using Resources.Utilities;

namespace Server.Addon {
    class Tomb {
        public static EntityUpdate Show(EntityUpdate entityData) {
            return new EntityUpdate() {
                guid = entityData.guid + 1000,
                position = entityData.position,
                hostility = Hostility.neutral,
                //entityType = (EntityType)(-1),
                appearance = new EntityUpdate.Appearance() {
                    character_size = new FloatVector() {
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
                guid = player.guid + 1000,
                hostility = Hostility.neutral,
                entityType = (EntityType)(-1),
                HP = 0
            };
            return tomb;
        }
    }
}
