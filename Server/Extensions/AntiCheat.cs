using Resources;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Extensions {
    public static class AntiCheat {
        public static void Init() {
            Server.EntityUpdated += That;
        }

        private static void That(EntityUpdate entityUpdate, Player player) {
            string ACmessage = Inspect(entityUpdate, player.entity);
            if (ACmessage != null) {
                Server.Kick(player, ACmessage);
            }
        }

        private static string Inspect(EntityUpdate current, EntityUpdate previous) {
            if (current.guid != previous.guid) {
                return "guid";
            }
            if (current.hostility != null && current.hostility != 0) {
                return "hostility";
            }
            if (current.appearance != null) {
                if ((current.appearance.character_size.x != 0.8000000119f && current.appearance.character_size.x != 0.9600000381f && current.appearance.character_size.x != 1.039999962f) ||
                    (current.appearance.character_size.y != 0.8000000119f && current.appearance.character_size.y != 0.9600000381f && current.appearance.character_size.y != 1.039999962f) ||
                    (current.appearance.character_size.z != 1.799999952f && current.appearance.character_size.z != 2.160000086f && current.appearance.character_size.z != 2.339999914f) ||
                    (current.appearance.head_size != 0.8999999762f && current.appearance.head_size != 1.00999999f) ||
                    current.appearance.body_size != 1.0f ||
                    current.appearance.hand_size != 1.0f ||
                    current.appearance.foot_size != 0.9800000191f ||
                    current.appearance.shoulder2_size != 1.0f ||
                    current.appearance.weapon_size < 0.7f || current.appearance.weapon_size > 1.3f ||
                    current.appearance.tail_size != 0.8000000119f ||
                    (current.appearance.shoulder_size != 1.0f && current.appearance.shoulder_size != 1.200000048f) ||
                    current.appearance.wings_size != 1.0f) {
                    return "appearance";
                }
            }
            if (current.charge != null && current.charge > 1) { //(current.MP ?? previous.MP)) {
                return "MP charge";
            }
            if (current.HP != null && current.HP > 3333) {
                return "HP";
            }
            if (current.MP != null && current.MP > 1) {
                return "MP";
            }
            //if(current.multipliers != null) {
            //    if(current.multipliers.HP != 100 ||
            //        current.multipliers.attackSpeed != 1 ||
            //        current.multipliers.damage != 1 ||
            //        current.multipliers.armor != 1 ||
            //        current.multipliers.resi != 1) {
            //        return "multipliers";
            //    }
            //}
            if (current.level != null && current.level > 500) {
                return "level";
            }
            if (current.consumable != null) {
                if (current.consumable.type == ItemType.Food &&
                    current.consumable.subtype == 1 ||
                    current.consumable.level > 647 ||
                    current.consumable.rarity != 0) {
                    //return "consumable";
                }
            }
            if (current.equipment != null) {
                foreach (Item item in current.equipment) {
                    if (item.type != 0 &&
                       (item.level > 647 || (byte)item.rarity > 4)) {
                        return "equipment";
                    }
                }
            }
            if (current.skillDistribution != null) {
                if (current.skillDistribution.petmaster +
                    current.skillDistribution.petriding +
                    current.skillDistribution.sailing +
                    current.skillDistribution.climbing +
                    current.skillDistribution.hangGliding +
                    current.skillDistribution.swimming +
                    current.skillDistribution.ability1 +
                    current.skillDistribution.ability2 +
                    current.skillDistribution.ability3 +
                    current.skillDistribution.ability4 +
                    current.skillDistribution.ability5 > 500 * 2 - 2) {
                    return "skill distribution";
                }
            }
            return null;
        }
    }
}
