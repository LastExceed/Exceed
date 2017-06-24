using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Resources.Packet
{
    public class ServerUpdate
    {
        public const int packetID = 4;

        public List<Part.BlockDelta> blockDeltas = new List<Part.BlockDelta>();
        public List<Packet.Hit> hits = new List<Packet.Hit>();
        public List<Part.Particle> particles = new List<Part.Particle>();
        public List<Part.Sound> sounds = new List<Part.Sound>();
        public List<Packet.Shoot> shoots = new List<Packet.Shoot>();
        public List<Part.StaticEntity> statics = new List<Part.StaticEntity>();
        public List<Part.ChunkItems> chunkItems = new List<Part.ChunkItems>();
        public List<Part.P48> p48s = new List<Part.P48>();
        public List<Part.Pickup> pickups = new List<Part.Pickup>();
        public List<Part.Kill> kills = new List<Part.Kill>();
        public List<Part.Damage> damages = new List<Part.Damage>();
        public List<Packet.PassiveProc> passiveProcs = new List<Packet.PassiveProc>();
        public List<Part.Mission> missions = new List<Part.Mission>();

        public void read(BinaryReader reader)
        {
            //todo
        }

        public byte[] getBytes()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(blockDeltas.Count);
            foreach (Part.BlockDelta blockDelta in blockDeltas)
            {
                blockDelta.write(writer);
            }

            writer.Write(hits.Count);
            foreach (Hit hit in hits)
            {
                hit.write(writer);
            }

            writer.Write(particles.Count);
            foreach (Part.Particle particle in particles)
            {
                particle.write(writer);
            }

            writer.Write(sounds.Count);
            foreach (Part.Sound sound in sounds)
            {
                sound.write(writer);
            }

            writer.Write(shoots.Count);
            foreach (Shoot shoot in shoots)
            {
                shoot.write(writer);
            }

            writer.Write(statics.Count);
            foreach (Part.StaticEntity staticEntity in statics)
            {
                staticEntity.write(writer);
            }

            writer.Write(chunkItems.Count);
            foreach (Part.ChunkItems chunkItem in chunkItems)
            {
                chunkItem.write(writer);
            }

            writer.Write(p48s.Count);
            foreach (Part.P48 p48 in p48s)
            {
                p48.write(writer);
            }

            writer.Write(pickups.Count);
            foreach (Part.Pickup pickup in pickups)
            {
                pickup.write(writer);
            }

            writer.Write(kills.Count);
            foreach (Part.Kill kill in kills)
            {
                kill.write(writer);
            }

            writer.Write(damages.Count);
            foreach (Part.Damage damage in damages)
            {
                damage.write(writer);
            }

            writer.Write(passiveProcs.Count); //npc rClick ??? todo
            foreach (PassiveProc passiveProc in passiveProcs)
            {
                passiveProc.write(writer);
            }

            writer.Write(missions.Count);
            foreach (Part.Mission mission in missions)
            {
                mission.write(writer);
            }


            byte[] data = Zlib.Compress(stream.ToArray());
            stream.Dispose();
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            writer.Write(packetID);
            writer.Write(data.Length);
            writer.Write(data);
            data = stream.ToArray();
            stream.Dispose();
            return data;
        }

        public void send(Player player)
        {
            byte[] data = this.getBytes();
            //SpinWait.SpinUntil(() => !player.busy);
            //player.busy = true;
            player.writer.Write(data);
            //player.busy = false;
        }
        public void send(Dictionary<ulong, Player> players, ulong toSkip)
        {
            byte[] data = this.getBytes();
            foreach (Player player in new List<Player>(players.Values))
            {
                if (player.entityData.guid != toSkip)
                {
                    try
                    {
                        player.writer.Write(data);
                    }
                    catch (IOException)
                    {
                        
                    }
                }
            }
        }
    }
}
