using System.Collections.Generic;
using System.IO;
using System.Text;
using Resources.Utilities;
using System.Threading;
using System;

namespace Resources.Packet {
    public class EntityUpdate {
        public const int packetID = 0;

        public ulong guid;
        public int bitfield1;
        public int bitfield2;

        public LongVector position = new LongVector();
        public FloatVector rotation = new FloatVector();
        public FloatVector velocity = new FloatVector();
        public FloatVector acceleration = new FloatVector();
        public FloatVector extraVel = new FloatVector();
        public float viewportPitch;
        public int physicsFlags;
        public byte hostility;
        public int entityType;
        public byte mode;
        public int modeTimer;
        public int combo;
        public int lastHitTime;
        public Part.Appearance appearance = new Part.Appearance();
        public short entityFlags;
        public int roll;
        public int stun;
        public int slow;
        public int ice;
        public int wind;
        public int showPatchTime;
        public byte entityClass;
        public byte specialization;
        public float charge;
        public FloatVector unused24 = new FloatVector();
        public FloatVector unused25 = new FloatVector();
        public FloatVector rayHit = new FloatVector();
        public float HP;
        public float MP;
        public float block;
        public Part.Multipliers multipliers = new Part.Multipliers();
        public byte unused31;
        public byte unused32;
        public int level;
        public int XP;
        public long parentOwner;
        public long unused36;
        public byte powerBase;
        public int unused38;
        public IntVector unused39 = new IntVector();
        public LongVector spawnPos = new LongVector();
        public IntVector unused41 = new IntVector();
        public byte unused42;
        public Part.Item consumable = new Part.Item();
        public Part.Item[] equipment = new Part.Item[13];
        public string name = "";
        public Part.SkillDistribution skillDistribution = new Part.SkillDistribution();
        public int manaCubes;

        public EntityUpdate() //constructor
        {
            for (int i = 0; i < 13; i++) {
                equipment[i] = new Part.Item();
            }
        }

        public EntityUpdate(BinaryReader reader) {
            int size = reader.ReadInt32();
            byte[] compressed = reader.ReadBytes(size);
            byte[] uncompressed = Zlib.Uncompress(compressed);

            MemoryStream stream = new MemoryStream(uncompressed);
            BinaryReader r = new BinaryReader(stream);
            guid = r.ReadUInt64();
            bitfield1 = r.ReadInt32();
            bitfield2 = r.ReadInt32();
            if (Tools.GetBit(bitfield1, 0)) //position
            {
                position.Read(r);
            }
            if (Tools.GetBit(bitfield1, 1)) //orientation
            {
                rotation.Read(r);
            }
            if (Tools.GetBit(bitfield1, 2)) //velocity
            {
                velocity.Read(r);
            }
            if (Tools.GetBit(bitfield1, 3)) //acceleration
            {
                acceleration.Read(r);
            }
            if (Tools.GetBit(bitfield1, 4)) //extra veloctiy
            {
                extraVel.Read(r);
            }
            if (Tools.GetBit(bitfield1, 5)) //viewport pitch
            {
                viewportPitch = r.ReadSingle();
            }
            if (Tools.GetBit(bitfield1, 6)) //physics flags
            {
                physicsFlags = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 7)) //hostile?
            {
                hostility = r.ReadByte();
            }
            if (Tools.GetBit(bitfield1, 8)) //entity type
            {
                entityType = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 9)) //current mode
            {
                mode = r.ReadByte();
            }
            if (Tools.GetBit(bitfield1, 10)) //mode start time
            {
                modeTimer = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 11)) //combo
            {
                combo = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 12)) //last hittime
            {
                lastHitTime = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 13)) //appearance data
            {
                appearance.Read(r);
            }
            if (Tools.GetBit(bitfield1, 14)) //entity flags
            {
                entityFlags = r.ReadInt16();
            }
            if (Tools.GetBit(bitfield1, 15)) //roll
            {
                roll = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 16)) //stun
            {
                stun = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 17)) //slowed?
            {
                slow = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 18)) //make blue time (ice)
            {
                ice = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 19)) //speed up time (wind)
            {
                wind = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 20)) //show patch time?
            {
                showPatchTime = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield1, 21)) //public class
            {
                entityClass = r.ReadByte();
            }
            if (Tools.GetBit(bitfield1, 22)) //subpublic class
            {
                specialization = r.ReadByte();
            }
            if (Tools.GetBit(bitfield1, 23)) //charge
            {
                charge = r.ReadSingle();
            }
            if (Tools.GetBit(bitfield1, 24)) //unused vector
            {
                unused24.Read(r);
            }
            if (Tools.GetBit(bitfield1, 25)) //unused vector
            {
                unused25.Read(r);
            }
            if (Tools.GetBit(bitfield1, 26)) //ray hit
            {
                rayHit.Read(r);
            }
            if (Tools.GetBit(bitfield1, 27)) //HP
            {
                HP = r.ReadSingle();
            }
            if (Tools.GetBit(bitfield1, 28)) //MP
            {
                MP = r.ReadSingle();
            }
            if (Tools.GetBit(bitfield1, 29)) //block power
            {
                block = r.ReadSingle();
            }
            if (Tools.GetBit(bitfield1, 30)) //multipliers
            {
                multipliers.Read(r);
            }
            if (Tools.GetBit(bitfield1, 31)) //unused
            {
                unused31 = r.ReadByte();
            }
            if (Tools.GetBit(bitfield2, 32 - 32)) //unused
            {
                unused32 = r.ReadByte();
            }
            if (Tools.GetBit(bitfield2, 33 - 32)) //level
            {
                level = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield2, 34 - 32)) //xp
            {
                XP = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield2, 35 - 32)) //parent owner?
            {
                parentOwner = r.ReadInt64();
            }
            if (Tools.GetBit(bitfield2, 36 - 32)) //unused *2
            {
                unused36 = r.ReadInt64();
            }
            if (Tools.GetBit(bitfield2, 37 - 32)) //power base
            {
                powerBase = r.ReadByte();
            }
            if (Tools.GetBit(bitfield2, 38 - 32)) //unused
            {
                unused38 = r.ReadInt32();
            }
            if (Tools.GetBit(bitfield2, 39 - 32)) //unused vector
            {
                unused39.Read(r);
            }
            if (Tools.GetBit(bitfield2, 40 - 32)) //spawn position
            {
                spawnPos.Read(r);
            }
            if (Tools.GetBit(bitfield2, 41 - 32)) //unused vector
            {
                unused41.Read(r);
            }
            if (Tools.GetBit(bitfield2, 42 - 32)) //unused
            {
                unused42 = r.ReadByte();
            }
            if (Tools.GetBit(bitfield2, 43 - 32)) //consumable
            {
                consumable = new Part.Item(reader);
            }
            if (Tools.GetBit(bitfield2, 44 - 32)) //equipment
            {
                for (int i = 0; i < 13; i++) {
                    equipment[i] = new Part.Item(r);
                }
            }
            if (Tools.GetBit(bitfield2, 45 - 32)) //name
            {
                name = new string(r.ReadChars(16));
                name = name.Substring(0, name.IndexOf("\0"));
            }
            if (Tools.GetBit(bitfield2, 46 - 32)) //skills (11*4)
            {
                skillDistribution.Read(r);
            }
            if (Tools.GetBit(bitfield2, 47 - 32)) //mama cubes 
            {
                manaCubes = r.ReadInt32();
            }
        }

        public void Merge(EntityUpdate playerEntityData) {
            if (Tools.GetBit(bitfield1, 0)) //position
            {
                playerEntityData.position = position;
            }
            if (Tools.GetBit(bitfield1, 1)) //orientation
            {
                playerEntityData.rotation = rotation;
            }
            if (Tools.GetBit(bitfield1, 2)) //velocity
            {
                playerEntityData.velocity = velocity;
            }
            if (Tools.GetBit(bitfield1, 3)) //acceleration
            {
                playerEntityData.acceleration = acceleration;
            }
            if (Tools.GetBit(bitfield1, 4)) //extra veloctiy
            {
                playerEntityData.extraVel = extraVel;
            }
            if (Tools.GetBit(bitfield1, 5)) //viewport pitch
            {
                playerEntityData.viewportPitch = viewportPitch;
            }
            if (Tools.GetBit(bitfield1, 6)) //physics flags
            {
                playerEntityData.physicsFlags = physicsFlags;
            }
            if (Tools.GetBit(bitfield1, 7)) //hostile?
            {
                playerEntityData.hostility = hostility;
            }
            if (Tools.GetBit(bitfield1, 8)) //entity type
            {
                playerEntityData.entityType = entityType;
            }
            if (Tools.GetBit(bitfield1, 9)) //current mode
            {
                playerEntityData.mode = mode;
            }
            if (Tools.GetBit(bitfield1, 10)) //mode start time
            {
                playerEntityData.modeTimer = modeTimer;
            }
            if (Tools.GetBit(bitfield1, 11)) //combo
            {
                playerEntityData.combo = combo;
            }
            if (Tools.GetBit(bitfield1, 12)) //last hittime
            {
                playerEntityData.lastHitTime = lastHitTime;
            }
            if (Tools.GetBit(bitfield1, 13)) //appearance data
            {
                playerEntityData.appearance = appearance;
            }
            if (Tools.GetBit(bitfield1, 14)) //entity flags
            {
                playerEntityData.entityFlags = entityFlags;
            }
            if (Tools.GetBit(bitfield1, 15)) //roll
            {
                playerEntityData.roll = roll;
            }
            if (Tools.GetBit(bitfield1, 16)) //stun
            {
                playerEntityData.stun = stun;
            }
            if (Tools.GetBit(bitfield1, 17)) //slowed?
            {
                playerEntityData.slow = slow;
            }
            if (Tools.GetBit(bitfield1, 18)) //make blue time (ice)
            {
                playerEntityData.ice = ice;
            }
            if (Tools.GetBit(bitfield1, 19)) //speed up time (wind)
            {
                playerEntityData.wind = wind;
            }
            if (Tools.GetBit(bitfield1, 20)) //show patch time?
            {
                playerEntityData.showPatchTime = showPatchTime;
            }
            if (Tools.GetBit(bitfield1, 21)) //public class
            {
                playerEntityData.entityClass = entityClass;
            }
            if (Tools.GetBit(bitfield1, 22)) //subpublic class
            {
                playerEntityData.specialization = specialization;
            }
            if (Tools.GetBit(bitfield1, 23)) //charge
            {
                playerEntityData.charge = charge;
            }
            if (Tools.GetBit(bitfield1, 24)) //unused vector
            {
                playerEntityData.unused24 = unused24;
            }
            if (Tools.GetBit(bitfield1, 25)) //unused vector
            {
                playerEntityData.unused25 = unused25;
            }
            if (Tools.GetBit(bitfield1, 26)) //ray hit
            {
                playerEntityData.rayHit = rayHit;
            }
            if (Tools.GetBit(bitfield1, 27)) //HP
            {
                playerEntityData.HP = HP;
            }
            if (Tools.GetBit(bitfield1, 28)) //MP
            {
                playerEntityData.MP = MP;
            }
            if (Tools.GetBit(bitfield1, 29)) //block power
            {
                playerEntityData.block = block;
            }
            if (Tools.GetBit(bitfield1, 30)) //multipliers
            {
                playerEntityData.multipliers = multipliers;
            }
            if (Tools.GetBit(bitfield1, 31)) //unused
            {
                playerEntityData.unused31 = unused31;
            }
            if (Tools.GetBit(bitfield2, 32 - 32)) //unused
            {
                playerEntityData.unused32 = unused32;
            }
            if (Tools.GetBit(bitfield2, 33 - 32)) //level
            {
                playerEntityData.level = level;
            }
            if (Tools.GetBit(bitfield2, 34 - 32)) //xp
            {
                playerEntityData.XP = XP;
            }
            if (Tools.GetBit(bitfield2, 35 - 32)) //parent owner?
            {
                playerEntityData.parentOwner = parentOwner;
            }
            if (Tools.GetBit(bitfield2, 36 - 32)) //unused *2
            {
                playerEntityData.unused36 = unused36;
            }
            if (Tools.GetBit(bitfield2, 37 - 32)) //power base
            {
                playerEntityData.powerBase = powerBase;
            }
            if (Tools.GetBit(bitfield2, 38 - 32)) //unused
            {
                playerEntityData.unused38 = unused38;
            }
            if (Tools.GetBit(bitfield2, 39 - 32)) //unused vector
            {
                playerEntityData.unused39 = unused39;
            }
            if (Tools.GetBit(bitfield2, 40 - 32)) //spawn position
            {
                playerEntityData.spawnPos = spawnPos;
            }
            if (Tools.GetBit(bitfield2, 41 - 32)) //unused vector
            {
                playerEntityData.unused41 = unused41;
            }
            if (Tools.GetBit(bitfield2, 42 - 32)) //unused
            {
                playerEntityData.unused42 = unused42;
            }
            if (Tools.GetBit(bitfield2, 43 - 32)) //consumable
            {
                playerEntityData.consumable = consumable;
            }
            if (Tools.GetBit(bitfield2, 44 - 32)) //equipment
            {
                playerEntityData.equipment = equipment;
            }
            if (Tools.GetBit(bitfield2, 45 - 32)) //name
            {
                playerEntityData.name = name;
            }
            if (Tools.GetBit(bitfield2, 46 - 32)) //skills (11*4)
            {
                playerEntityData.skillDistribution = skillDistribution;
            }
            if (Tools.GetBit(bitfield2, 47 - 32)) //mama cubes 
            {
                playerEntityData.manaCubes = manaCubes;
            }
        }

        public void Filter(EntityUpdate previous) {
            if (Tools.GetBit(bitfield1, 0)) //position
            {
                if (Math.Abs(position.x - previous.position.x) < 32768 &&
                    Math.Abs(position.y - previous.position.y) < 32768 &&
                    Math.Abs(position.z - previous.position.z) < 32768) {
                    bitfield1 &= ~(1 << 0);
                }
            }
            bitfield1 &= ~(1 << 1);
            if (Tools.GetBit(bitfield1, 2)) //velocity
            {
                if (Math.Abs(velocity.x) < 1 &&
                    Math.Abs(velocity.y) < 1 &&
                    Math.Abs(velocity.z) < 1) {
                    bitfield1 &= ~(1 << 2);
                }
            }
            if (Tools.GetBit(bitfield1, 4)) //extra veloctiy
            {
                if (Math.Abs(extraVel.x) < 1 &&
                    Math.Abs(extraVel.y) < 1 &&
                    Math.Abs(extraVel.z) < 1) {
                    bitfield1 &= ~(1 << 4);
                }
            }
            bitfield1 &= ~(1 << 5);
            if (Tools.GetBit(bitfield1, 10)) //mode start time
            {
                if (modeTimer != 0) {
                    bitfield1 &= ~(1 << 10);
                }
            }
            bitfield1 &= ~(1 << 12);
            if (Tools.GetBit(bitfield1, 15)) //roll
            {
                if (roll != 600) {
                    bitfield1 &= ~(1 << 15);
                }
            }
            if (Tools.GetBit(bitfield1, 16)) //stun
            {
                if (stun < previous.stun) {
                    bitfield1 &= ~(1 << 16);
                }
            }
            if (Tools.GetBit(bitfield1, 17)) //slowed?
            {
                if (slow < previous.slow) {
                    bitfield1 &= ~(1 << 17);
                }
            }
            if (Tools.GetBit(bitfield1, 18)) //make blue time (ice)
            {
                if (ice < previous.ice) {
                    bitfield1 &= ~(1 << 18);
                }
            }
            if (Tools.GetBit(bitfield1, 19)) //speed up time (wind)
            {
                if (wind < previous.wind) {
                    bitfield1 &= ~(1 << 19);
                }
            }
            bitfield1 &= ~(1 << 20);
            bitfield1 &= ~(1 << 24);
            bitfield1 &= ~(1 << 25);
            if (Tools.GetBit(bitfield1, 26)) //ray hit
            {
                if (previous.mode == 0 ||
                    (Math.Abs(rayHit.x - previous.rayHit.x) > 0.5 &&
                     Math.Abs(rayHit.y - previous.rayHit.y) > 0.5 &&
                     Math.Abs(rayHit.z - previous.rayHit.z) > 0.5)) {
                    bitfield1 &= ~(1 << 26);
                }
            }
            bitfield1 &= ~(1 << 28);
            bitfield1 &= ~(1 << 30);
            bitfield1 &= ~(1 << 31);
            bitfield2 &= ~(1 << 32 - 32);
            bitfield2 &= ~(1 << 34 - 32);
            //bitfield2 &= ~(1 << 35 - 32); //parent owner?
            bitfield2 &= ~(1 << 36 - 32);
            bitfield2 &= ~(1 << 37 - 32);
            bitfield2 &= ~(1 << 38 - 32);
            bitfield2 &= ~(1 << 39 - 32);
            bitfield2 &= ~(1 << 40 - 32);
            bitfield2 &= ~(1 << 41 - 32);
            bitfield2 &= ~(1 << 42 - 32);
            bitfield2 &= ~(1 << 46 - 32);
            bitfield2 &= ~(1 << 47 - 32);
        }

        public byte[] GetBytes() {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            writer.Write(guid);
            writer.Write(bitfield1);
            writer.Write(bitfield2);

            if (Tools.GetBit(bitfield1, 0)) //position
            {
                position.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 1)) //orientation
            {
                rotation.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 2)) //velocity
            {
                velocity.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 3)) //acceleration
            {
                acceleration.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 4)) //extra veloctiy
            {
                extraVel.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 5)) //viewport pitch
            {
                writer.Write(viewportPitch);
            }
            if (Tools.GetBit(bitfield1, 6)) //physics flags
            {
                writer.Write(physicsFlags);
            }
            if (Tools.GetBit(bitfield1, 7)) //hostile?
            {
                writer.Write(hostility);
            }
            if (Tools.GetBit(bitfield1, 8)) //entity type
            {
                writer.Write(entityType);
            }
            if (Tools.GetBit(bitfield1, 9)) //current mode
            {
                writer.Write(mode);
            }
            if (Tools.GetBit(bitfield1, 10)) //mode start time
            {
                writer.Write(modeTimer);
            }
            if (Tools.GetBit(bitfield1, 11)) //combo
            {
                writer.Write(combo);
            }
            if (Tools.GetBit(bitfield1, 12)) //last hittime
            {
                writer.Write(lastHitTime);
            }
            if (Tools.GetBit(bitfield1, 13)) //appearance data
            {
                appearance.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 14)) //entity flags
            {
                writer.Write(entityFlags);
            }
            if (Tools.GetBit(bitfield1, 15)) //roll
            {
                writer.Write(roll);
            }
            if (Tools.GetBit(bitfield1, 16)) //stun
            {
                writer.Write(stun);
            }
            if (Tools.GetBit(bitfield1, 17)) //slowed?
            {
                writer.Write(slow);
            }
            if (Tools.GetBit(bitfield1, 18)) //make blue time (ice)
            {
                writer.Write(ice);
            }
            if (Tools.GetBit(bitfield1, 19)) //speed up time (wind)
            {
                writer.Write(wind);
            }
            if (Tools.GetBit(bitfield1, 20)) //show patch time?
            {
                writer.Write(showPatchTime);
            }
            if (Tools.GetBit(bitfield1, 21)) //public class
            {
                writer.Write(entityClass);
            }
            if (Tools.GetBit(bitfield1, 22)) //subpublic class
            {
                writer.Write(specialization);
            }
            if (Tools.GetBit(bitfield1, 23)) //charge
            {
                writer.Write(charge);
            }
            if (Tools.GetBit(bitfield1, 24)) //unused vector
            {
                unused24.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 25)) //unused vector
            {
                unused25.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 26)) //ray hit
            {
                rayHit.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 27)) //HP
            {
                writer.Write(HP);
            }
            if (Tools.GetBit(bitfield1, 28)) //MP
            {
                writer.Write(MP);
            }
            if (Tools.GetBit(bitfield1, 29)) //block power
            {
                writer.Write(block);
            }
            if (Tools.GetBit(bitfield1, 30)) //multipliers
            {
                multipliers.Write(writer);
            }
            if (Tools.GetBit(bitfield1, 31)) //unused
            {
                writer.Write(unused31);
            }
            if (Tools.GetBit(bitfield2, 32 - 32)) //unused
            {
                writer.Write(unused32);
            }
            if (Tools.GetBit(bitfield2, 33 - 32)) //level
            {
                writer.Write(level);
            }
            if (Tools.GetBit(bitfield2, 34 - 32)) //xp
            {
                writer.Write(XP);
            }
            if (Tools.GetBit(bitfield2, 35 - 32)) //parent owner?
            {
                writer.Write(parentOwner);
            }
            if (Tools.GetBit(bitfield2, 36 - 32)) //unused *2
            {
                writer.Write(unused36);
            }
            if (Tools.GetBit(bitfield2, 37 - 32)) //power base
            {
                writer.Write(powerBase);
            }
            if (Tools.GetBit(bitfield2, 38 - 32)) //unused
            {
                writer.Write(unused38);
            }
            if (Tools.GetBit(bitfield2, 39 - 32)) //unused vector
            {
                unused39.Write(writer);
            }
            if (Tools.GetBit(bitfield2, 40 - 32)) //spawn position
            {
                spawnPos.Write(writer);
            }
            if (Tools.GetBit(bitfield2, 41 - 32)) //unused vector
            {
                unused41.Write(writer);
            }
            if (Tools.GetBit(bitfield2, 42 - 32)) //unused
            {
                writer.Write(unused42);
            }
            if (Tools.GetBit(bitfield2, 43 - 32)) //consumable
            {
                consumable.Write(writer);
            }
            if (Tools.GetBit(bitfield2, 44 - 32)) //equipment
            {
                foreach (Part.Item item in equipment) {
                    item.Write(writer);
                }
            }
            if (Tools.GetBit(bitfield2, 45 - 32)) //name
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(name);
                writer.Write(nameBytes);
                writer.Write(new byte[16 - nameBytes.Length]);
            }
            if (Tools.GetBit(bitfield2, 46 - 32)) //skills (11*4)
            {
                skillDistribution.Write(writer);
            }
            if (Tools.GetBit(bitfield2, 47 - 32)) //mama cubes 
            {
                writer.Write(manaCubes);
            }

            byte[] uncompressed = stream.ToArray();
            stream.Dispose();

            return Zlib.Compress(uncompressed);
        }

        public void Write(BinaryWriter writer, bool writePacketID) {
            if (writePacketID) {
                writer.Write(packetID);
            }
            var data = GetBytes();
            writer.Write(data.Length);
            writer.Write(data);
        }
        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            byte[] data = this.GetBytes();
            foreach (Player player in new List<Player>(players.Values)) {
                if (player.entityData.guid != toSkip) {
                    try {
                        player.writer.Write(data);
                    } catch (IOException) {

                    }
                }
            }
        }
    }
}