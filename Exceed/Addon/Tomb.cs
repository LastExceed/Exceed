using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon
{
    class Tomb
    {
        public static EntityUpdate show(Player player)
        {
            var tomb = new EntityUpdate();
            tomb.guid = player.entityData.guid + 1000;
            tomb.bitfield1 = 0b00001000_00000000_00100001_10000001;
            tomb.bitfield2 = 0b00000000_00000000_00100000_00000000;
            tomb.position = player.entityData.position;
            tomb.hostility = (byte)Database.Hostility.neutral;
            tomb.entityType = -1;
            tomb.appearance = new Appearance();
            tomb.appearance.character_size.x = 1;
            tomb.appearance.character_size.y = 1;
            tomb.appearance.character_size.z = 1;
            tomb.appearance.head_model = 2155;
            tomb.appearance.head_size = 1;
            tomb.HP = 100;
            tomb.name = "tombstone\0\0\0\0\0\0\0";
            return tomb;
        }

        public static EntityUpdate hide(Player player)
        {
            var tomb = new EntityUpdate();
            tomb.guid = player.entityData.guid + 1000;
            tomb.bitfield1 = 0b00001000_00000000_00000001_10000000;
            tomb.hostility = (byte)Database.Hostility.neutral;
            tomb.entityType = -1;
            tomb.HP = 0;
            return tomb;
        }
    }
}
