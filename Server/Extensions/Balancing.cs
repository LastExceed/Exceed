using Resources;
using Resources.Datagram;

namespace Server.Extensions {
    public static class Balancing {
        public static void Init() {
            Server.EntityAttacked += CutDmgInHalf;
        }

        private static void CutDmgInHalf(Attack datagram, Player player) {
            if (datagram.Damage < 0) {
                datagram.Damage *= 0.5f; //dmg
            }
            else {
                datagram.Damage *= 0.333333f; //heal
            }
        }
    }
}
