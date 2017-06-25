using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class Tomb {
        public static EntityUpdate Show(Player player) {
            var tomb = new EntityUpdate() {
                guid = player.entityData.guid + 1000,
                bitfield1 = 0b00001000_00000000_00100001_10000001,
                bitfield2 = 0b00000000_00000000_00100000_00000000,
                position = player.entityData.position,
                hostility = (byte)Database.Hostility.neutral,
                entityType = -1,
                appearance = new Appearance()
            };
            tomb.appearance.character_size.x = 1;
            tomb.appearance.character_size.y = 1;
            tomb.appearance.character_size.z = 1;
            tomb.appearance.head_model = 2155;
            tomb.appearance.head_size = 1;
            tomb.HP = 100;
            tomb.name = "tombstone\0\0\0\0\0\0\0";
            return tomb;
        }

        public static EntityUpdate Hide(Player player) {
            var tomb = new EntityUpdate() {
                guid = player.entityData.guid + 1000,
                bitfield1 = 0b00001000_00000000_00000001_10000000,
                hostility = (byte)Database.Hostility.neutral,
                entityType = -1,
                HP = 0
            };
            return tomb;
        }
    }
}
