using Resources;
using Resources.Packet;

namespace Server.Addon {
    class AntiCheat {
        public static string Inspect(EntityUpdate toInspect) {
            if(toInspect.hostility != null && toInspect.hostility != 0) {
                return "hostility";
            }
            if(toInspect.appearance != null) {
                if((toInspect.appearance.character_size.x != 0.8000000119f && toInspect.appearance.character_size.x != 0.9600000381f && toInspect.appearance.character_size.x != 1.039999962f) ||
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
            if(toInspect.charge != null && toInspect.charge > 1) {
                return "MP charge";
            }
            if(toInspect.HP != null && toInspect.HP > 3333) {
                return "HP";
            }
            if(toInspect.MP != null && toInspect.MP > 1) {
                return "MP";
            }
            if(toInspect.multipliers != null) {
                if(toInspect.multipliers.HP != 100 ||
                    toInspect.multipliers.attackSpeed != 1 ||
                    toInspect.multipliers.damage != 1 ||
                    toInspect.multipliers.armor != 1 ||
                    toInspect.multipliers.resi != 1) {
                    return "multipliers";
                }
            }
            if(toInspect.level != null && toInspect.level > 500) {
                return "level";
            }
            if(toInspect.consumable != null) {
                if(toInspect.consumable.type == ItemType.Food &&
                    toInspect.consumable.subtype == 1 ||
                    toInspect.consumable.level > 647 ||
                    toInspect.consumable.rarity != 0) {
                    //return "consumable";
                }
            }
            if(toInspect.equipment != null) {
                foreach(Item item in toInspect.equipment) {
                    if(item.type != 0 &&
                       (item.level > 647 || (byte)item.rarity > 4)) {
                        return "equipment";
                    }
                }
            }
            if(toInspect.skillDistribution != null) {
                if(toInspect.skillDistribution.petmaster +
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
            return null;
        }
    }
}
//block
