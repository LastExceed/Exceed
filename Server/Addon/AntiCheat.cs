using Resources;
using Resources.Packet;
using Resources.Packet.Part;

namespace Server.Addon {
    class AntiCheat {
        public static string Inspect(EntityUpdate toInspect) {
            if (Tools.GetBit(toInspect.bitfield1, 7)) //hostile?
            {
                if (toInspect.hostility != 0) {
                    return "hostility";
                }
            }
            if (Tools.GetBit(toInspect.bitfield1, 13)) {
                if ((toInspect.appearance.character_size.x != 0.8000000119f && toInspect.appearance.character_size.x != 0.9600000381f && toInspect.appearance.character_size.x != 1.039999962f) ||
                    (toInspect.appearance.character_size.y != 0.8000000119f && toInspect.appearance.character_size.y != 0.9600000381f && toInspect.appearance.character_size.y != 1.039999962f) ||
                    (toInspect.appearance.character_size.z != 1.799999952f && toInspect.appearance.character_size.z != 2.160000086f && toInspect.appearance.character_size.z != 2.339999914f) ||
                    (toInspect.appearance.head_size != 0.8999999762f && toInspect.appearance.head_size != 1.00999999f) ||
                    toInspect.appearance.body_size != 1.0f ||
                    toInspect.appearance.hand_size != 1.0f ||
                    toInspect.appearance.foot_size != 0.9800000191f ||
                    toInspect.appearance.shoulder2_size != 1.0f ||
                    toInspect.appearance.weapon_size < 0.7f || toInspect.appearance.weapon_size > 1.3f ||
                    toInspect.appearance.tail_size != 0.8000000119f ||
                    (toInspect.appearance.shoulder_size != 1.0f && toInspect.appearance.shoulder_size != 1.200000048f) ||
                    toInspect.appearance.wings_size != 1.0f) {
                    return "appearance";
                }
            }
            if (Tools.GetBit(toInspect.bitfield1, 23)) //charge?
            {
                if (toInspect.charge > 1) {
                    return "MP charge";
                }
            }
            if (Tools.GetBit(toInspect.bitfield1, 27)) //HP?
            {
                if (toInspect.HP > 3333) {
                    return "HP";
                }
            }
            if (Tools.GetBit(toInspect.bitfield1, 28)) //MP?
            {
                if (toInspect.MP > 1) {
                    return "MP";
                }
            }
            if (Tools.GetBit(toInspect.bitfield1, 30)) //multipliers?
            {
                if (toInspect.multipliers.HP != 100 ||
                    toInspect.multipliers.attackSpeed != 1 ||
                    toInspect.multipliers.damge != 1 ||
                    toInspect.multipliers.armor != 1 ||
                    toInspect.multipliers.resi != 1) {
                    return "multipliers";
                }
            }
            if (Tools.GetBit(toInspect.bitfield2, 33 - 32)) //level
            {
                if (toInspect.level > 500) {
                    return "level";
                }
            }
            if (Tools.GetBit(toInspect.bitfield2, 43 - 32)) //consumable
            {
                if (toInspect.consumable.type == 1 &&
                    toInspect.consumable.subtype == 1 ||
                    toInspect.consumable.level > 647 ||
                    toInspect.consumable.rarity != 0) {
                    //return "consumable";
                }
            }
            if (Tools.GetBit(toInspect.bitfield2, 44 - 32)) //equip
            {
                foreach (Item item in toInspect.equipment) {
                    if (item.type != 0 && (item.level > 647 ||
                        item.rarity > 4)) {
                        return "equipment";
                    }
                }
            }
            if (Tools.GetBit(toInspect.bitfield2, 46 - 32)) //skills
            {
                if (toInspect.skillDistribution.petmaster +
                    toInspect.skillDistribution.petriding +
                    toInspect.skillDistribution.sailing +
                    toInspect.skillDistribution.climbing +
                    toInspect.skillDistribution.hangGliding +
                    toInspect.skillDistribution.swimming +
                    toInspect.skillDistribution.ability1 +
                    toInspect.skillDistribution.ability2 +
                    toInspect.skillDistribution.ability3 +
                    toInspect.skillDistribution.ability4 +
                    toInspect.skillDistribution.ability5 > 500 * 2 - 2) {
                    return "skill distribution";
                }
            }
            return "ok";
        }
    }
}
//block
