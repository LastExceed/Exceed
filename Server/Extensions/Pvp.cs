using Resources;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Extensions {
    public static class Pvp {
        public static void Init() {
            Server.EntityUpdated += EnableFriendlyFireFlag;
        }

        private static void EnableFriendlyFireFlag(EntityUpdate entityUpdate, Player player) {
            entityUpdate.entityFlags |= 1 << 5;
        }
    }
}
