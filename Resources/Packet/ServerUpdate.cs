using System.Collections.Generic;
using System.IO;
using Resources.Packet.Part;

namespace Resources.Packet {
    public class ServerUpdate {
        public const int packetID = 4;

        public List<BlockDelta> blockDeltas = new List<BlockDelta>();
        public List<Hit> hits = new List<Hit>();
        public List<Particle> particles = new List<Particle>();
        public List<Sound> sounds = new List<Sound>();
        public List<Shoot> shoots = new List<Shoot>();
        public List<StaticEntity> statics = new List<StaticEntity>();
        public List<ChunkItems> chunkItems = new List<ChunkItems>();
        public List<P48> p48s = new List<P48>();
        public List<Pickup> pickups = new List<Pickup>();
        public List<Kill> kills = new List<Kill>();
        public List<Damage> damages = new List<Damage>();
        public List<PassiveProc> passiveProcs = new List<PassiveProc>();
        public List<Mission> missions = new List<Mission>();

        public void Read(BinaryReader reader) {
            //todo
        }

        public byte[] GetBytes() {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(blockDeltas.Count);
            foreach(BlockDelta blockDelta in blockDeltas) {
                blockDelta.Write(writer);
            }

            writer.Write(hits.Count);
            foreach(Hit hit in hits) {
                hit.Write(writer, false);
            }

            writer.Write(particles.Count);
            foreach(Particle particle in particles) {
                particle.Write(writer);
            }

            writer.Write(sounds.Count);
            foreach(Sound sound in sounds) {
                sound.Write(writer);
            }

            writer.Write(shoots.Count);
            foreach(Shoot shoot in shoots) {
                shoot.Write(writer, false);
            }

            writer.Write(statics.Count);
            foreach(StaticEntity staticEntity in statics) {
                staticEntity.Write(writer);
            }

            writer.Write(chunkItems.Count);
            foreach(ChunkItems chunkItem in chunkItems) {
                chunkItem.Write(writer);
            }

            writer.Write(p48s.Count);
            foreach(P48 p48 in p48s) {
                p48.Write(writer);
            }

            writer.Write(pickups.Count);
            foreach(Pickup pickup in pickups) {
                pickup.Write(writer);
            }

            writer.Write(kills.Count);
            foreach(Kill kill in kills) {
                kill.Write(writer);
            }

            writer.Write(damages.Count);
            foreach(Damage damage in damages) {
                damage.Write(writer);
            }

            writer.Write(passiveProcs.Count); //npc rClick ??? todo
            foreach(PassiveProc passiveProc in passiveProcs) {
                passiveProc.Write(writer, false);
            }

            writer.Write(missions.Count);
            foreach(Mission mission in missions) {
                mission.Write(writer);
            }

            return Zlib.Compress(stream.ToArray());
        }

        public void Write(BinaryWriter writer, bool writePacketID = true) {
            byte[] data = GetBytes();

            if(writePacketID) {
                writer.Write(packetID);
            }
            writer.Write(data.Length);
            writer.Write(data);
        }

        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            byte[] data = GetBytes();
            foreach(Player player in new List<Player>(players.Values)) {
                if(player.entityData.guid != toSkip) {
                    try {
                        player.writer.Write(packetID);
                        player.writer.Write(data.Length);
                        player.writer.Write(data);
                    }
                    catch (IOException) { }
                }
            }
        }
    }
}
