using Resources.Datagram;
using Resources.Packet;
using System;

namespace Bridge {
    public delegate void EntityUpdateSentEventHandler(EntityUpdate entityUpdate);
    public delegate void EntityActionSentEventHandler(EntityAction entityAction);
    public delegate void HitSentHandler(Hit hit);
    public delegate void PassiveProcSentEventHandler(PassiveProc passiveProc);
    public delegate void ShotSentHandler(Shoot shoot);
    public delegate void ChatMessageSentEventHandler(ChatMessage chatMessage);
    public delegate void ChunkDiscoveredEventHandler(Chunk chunk);
    public delegate void SectorDiscoveredEventHandler(Sector sector);
    public delegate void VersionSentEventHandler(ProtocolVersion protocolVersion);

    public delegate void EntityUpdateReceivedEventHandler(EntityUpdate entityUpdate);
    public delegate void AttackReceivedEventHandler(Attack attack);
    public delegate void ProjectileReceivedEventHandler(Projectile projectile);
    public delegate void PassiveProcReceivedEventHandler(Proc proc);
    public delegate void ChatMessageReceivedEventHandler(long sender, string chat);
    public delegate void InGameTimeReceivedEventHandler(InGameTime inGameTime);
    public delegate void InteractionReceivedEventHandler(Interaction interaction);
    public delegate void StaticUpdateReceivedEventHandler(StaticUpdate staticUpdate);
    public delegate void ParticleReceivedEventHandler(Particle particle);
    public delegate void DynamicEntityRemovedEventHandler(RemoveDynamicEntity removeDynamicEntity);
    public delegate void SpecialMoveReceiveddEventHandler(SpecialMove specialMove);

    public delegate void ClientConnectedEventHandler();
    public delegate void ClientDisconnectedEventHandler();

    public static partial class BridgeCore {
        public static event EntityUpdateSentEventHandler EntityUpdateSent;
        public static event EntityActionSentEventHandler EntityActionSent;
        public static event HitSentHandler HitSent;
        public static event PassiveProcSentEventHandler PassiveProcSent;
        public static event ShotSentHandler ShotSent;
        public static event ChatMessageSentEventHandler ChatMessageSent;
        public static event ChunkDiscoveredEventHandler ChunkDiscovered;
        public static event SectorDiscoveredEventHandler SectorDiscovered;
        public static event VersionSentEventHandler VersionSent;

        public static event EntityUpdateReceivedEventHandler EntityUpdateReceived;
        public static event AttackReceivedEventHandler AttackReceived;
        public static event ProjectileReceivedEventHandler ProjectileReceived;
        public static event PassiveProcReceivedEventHandler PassiveProcReceived;
        public static event ChatMessageReceivedEventHandler ChatMessageReceived;
        public static event InGameTimeReceivedEventHandler InGameTimeReceived;
        public static event InteractionReceivedEventHandler InteractionReceived;
        public static event StaticUpdateReceivedEventHandler StaticUpdateReceived;
        public static event ParticleReceivedEventHandler ParticleReceived;
        public static event DynamicEntityRemovedEventHandler DynamicEntityRemoved;
        public static event SpecialMoveReceiveddEventHandler SpecialMoveReceived;

        public static event ClientConnectedEventHandler ClientConnected;
        public static event ClientDisconnectedEventHandler ClientDisconnected;
    }
}
