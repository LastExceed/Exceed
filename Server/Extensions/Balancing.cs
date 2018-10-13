using Resources;
using Resources.Datagram;

namespace Server.Extensions {
    public static class Balancing {
        public static void Init() {
            Server.EntityAttacked += CutDmgInHalf;
        }

        private static void CutDmgInHalf(Attack attack, Player player) {
            if (attack.Damage > 0) {
                attack.Damage *= 0.5f; //dmg
            }
            else {
                attack.Damage *= 0.333333f; //heal
            }
            if (attack.Target == player.entity.guid) {//players can't damage themselves. this prevents double self heals since selfheal is already applied locally
                attack.Damage = 0;
            }
        }
    }
}
