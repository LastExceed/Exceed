using Resources;
using Resources.Datagram;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server {
    public delegate void EntityUpdatedEventHandler(EntityUpdate entityUpdate, Player source);
    public delegate void EntityAttackedEventHandler(Attack datagram, Player source);
    public delegate void ProjectileCreatedEventHandler(Projectile datagram, Player source);
    public delegate void PassiveProccedEventHandler(Proc datagram, Player source);
    public delegate void ChatMessageReceivedEventHandler(string message, Player source);
    public delegate void EntityInteractedEventHandler(Interaction datagram, Player source);
    public delegate void EntityRemovedEventHandler(RemoveDynamicEntity datagram, Player source);
    public delegate void SpecialMoveUsedEventHandler(SpecialMove datagram, Player source);

    public static partial class Server {
        public static event EntityUpdatedEventHandler EntityUpdated;
        public static event EntityAttackedEventHandler EntityAttacked;
        public static event ProjectileCreatedEventHandler ProjectileCreated;
        public static event PassiveProccedEventHandler PassiveProcced;
        public static event ChatMessageReceivedEventHandler ChatMessageReceived;
        public static event EntityInteractedEventHandler EntityInteracted;
        public static event EntityRemovedEventHandler EntityRemoved;
        public static event SpecialMoveUsedEventHandler SpecialMoveUsed;
    }
}
