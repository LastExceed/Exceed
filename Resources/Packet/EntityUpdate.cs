using System.Collections.Generic;
using System.IO;
using System.Text;
using Resources.Utilities;
using System.Threading;
using System;

namespace Resources.Packet {
    public class EntityUpdate {
        public ulong guid;

        public LongVector position;
        public FloatVector rotation;
        public FloatVector velocity;
        public FloatVector acceleration;
        public FloatVector extraVel;
        public float? viewportPitch;
        public int? physicsFlags;
        public byte? hostility;
        public int? entityType;
        public byte? mode;
        public int? modeTimer;
        public int? combo;
        public int? lastHitTime;
        public Part.Appearance appearance;
        public short? entityFlags;
        public int? roll;
        public int? stun;
        public int? slow;
        public int? ice;
        public int? wind;
        public int? showPatchTime;
        public byte? entityClass;
        public byte? specialization;
        public float? charge;
        public FloatVector unused24;
        public FloatVector unused25;
        public FloatVector rayHit;
        public float? HP;
        public float? MP;
        public float? block;
        public Part.Multipliers multipliers;
        public byte? unused31;
        public byte? unused32;
        public int? level;
        public int? XP;
        public long? parentOwner;
        public long? unused36;
        public byte? powerBase;
        public int? unused38;
        public IntVector unused39;
        public LongVector spawnPos;
        public IntVector unused41;
        public byte? unused42;
        public Part.Item consumable;
        public Part.Item[] equipment;
        public string name;
        public Part.SkillDistribution skillDistribution;
        public int? manaCubes;
        public byte[] Data {
            get {
                var stream = new MemoryStream();
                var writer = new BinaryWriter(stream);
                
                byte[] compressed = GetBytes();

                writer.Write((int)Database.PacketID.entityUpdate);
                writer.Write(compressed.Length);
                writer.Write(compressed);
                return stream.ToArray();
            }
        }
        public bool IsEmpty {
        get {
                return !(position != null ||
                   rotation != null ||
                   velocity != null ||
                   acceleration != null ||
                   extraVel != null ||
                   viewportPitch != null ||
                   physicsFlags != null ||
                   hostility != null ||
                   entityType != null ||
                   mode != null ||
                   modeTimer != null ||
                   combo != null ||
                   lastHitTime != null ||
                   appearance != null ||
                   entityFlags != null ||
                   roll != null ||
                   stun != null ||
                   slow != null ||
                   ice != null ||
                   wind != null ||
                   showPatchTime != null ||
                   entityClass != null ||
                   specialization != null ||
                   charge != null ||
                   unused24 != null ||
                   unused25 != null ||
                   rayHit != null ||
                   HP != null ||
                   MP != null ||
                   block != null ||
                   multipliers != null ||
                   unused31 != null ||
                   unused32 != null ||
                   level != null ||
                   XP != null ||
                   parentOwner != null ||
                   unused36 != null ||
                   powerBase != null ||
                   unused38 != null ||
                   unused39 != null ||
                   spawnPos != null ||
                   unused41 != null ||
                   unused42 != null ||
                   consumable != null ||
                   equipment != null ||
                   name != null ||
                   skillDistribution != null ||
                   manaCubes != null);
            }
        }

        public EntityUpdate() {
            /*equipment = new Part.Item[13];
            for(int i = 0; i < 13; i++) {
                equipment[i] = new Part.Item();
            }*/
        }
        public EntityUpdate(BinaryReader reader) {
            byte[] uncompressed = Zlib.Decompress(reader.ReadBytes(reader.ReadInt32()));

            MemoryStream stream = new MemoryStream(uncompressed);
            BinaryReader r = new BinaryReader(stream);
            guid = r.ReadUInt64();
            var bitfield = r.ReadInt64();

            if(Tools.GetBit(bitfield, 0)) {
                position = new LongVector(r);
            }
            if(Tools.GetBit(bitfield, 1)) {
                rotation = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 2)) {
                velocity = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 3)) {
                acceleration = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 4)) {
                extraVel = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 5)) {
                viewportPitch = r.ReadSingle();
            }
            if(Tools.GetBit(bitfield, 6)) {
                physicsFlags = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 7)) {
                hostility = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 8)) {
                entityType = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 9)) {
                mode = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 10)) {
                modeTimer = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 11)) {
                combo = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 12)) {
                lastHitTime = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 13)) {
                appearance = new Part.Appearance(r);
            }
            if(Tools.GetBit(bitfield, 14)) {
                entityFlags = r.ReadInt16();
            }
            if(Tools.GetBit(bitfield, 15)) {
                roll = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 16)) {
                stun = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 17)) {
                slow = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 18)) {
                ice = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 19)) {
                wind = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 20)) {
                showPatchTime = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 21)) {
                entityClass = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 22)) {
                specialization = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 23)) {
                charge = r.ReadSingle();
            }
            if(Tools.GetBit(bitfield, 24)) {
                unused24 = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 25)) {
                unused25 = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 26)) {
                rayHit = new FloatVector(r);
            }
            if(Tools.GetBit(bitfield, 27)) {
                HP = r.ReadSingle();
            }
            if(Tools.GetBit(bitfield, 28)) {
                MP = r.ReadSingle();
            }
            if(Tools.GetBit(bitfield, 29)) {
                block = r.ReadSingle();
            }
            if(Tools.GetBit(bitfield, 30)) {
                multipliers = new Part.Multipliers(r);
            }
            if(Tools.GetBit(bitfield, 31)) {
                unused31 = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 32)) {
                unused32 = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 33)) {
                level = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 34)) {
                XP = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 35)) {
                parentOwner = r.ReadInt64();
            }
            if(Tools.GetBit(bitfield, 36)) {
                unused36 = r.ReadInt64();
            }
            if(Tools.GetBit(bitfield, 37)) {
                powerBase = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 38)) {
                unused38 = r.ReadInt32();
            }
            if(Tools.GetBit(bitfield, 39)) {
                unused39 = new IntVector(r);
            }
            if(Tools.GetBit(bitfield, 40)) {
                spawnPos = new LongVector(r);
            }
            if(Tools.GetBit(bitfield, 41)) {
                unused41 = new IntVector(r);
            }
            if(Tools.GetBit(bitfield, 42)) {
                unused42 = r.ReadByte();
            }
            if(Tools.GetBit(bitfield, 43)) {
                consumable = new Part.Item(r);
            }
            if(Tools.GetBit(bitfield, 44)) {
                equipment = new Part.Item[13];
                for(int i = 0; i < 13; i++) {
                    equipment[i] = new Part.Item(r);
                }
            }
            if(Tools.GetBit(bitfield, 45)) {
                name = new string(r.ReadChars(16));
                name = name.Substring(0, name.IndexOf("\0"));
            }
            if(Tools.GetBit(bitfield, 46)) {
                skillDistribution = new Part.SkillDistribution(r);
            }
            if(Tools.GetBit(bitfield, 47)) {
                manaCubes = r.ReadInt32();
            }
        }

        public EntityUpdate(byte[] data) : this(Convert(data)) { }
        private static BinaryReader Convert(byte[] data) {
            var reader = new BinaryReader(new MemoryStream(data));
            reader.ReadInt32();
            return reader;
        }

        public void Merge(EntityUpdate playerEntityData) {
            if(position != null) {
                playerEntityData.position = position;
            }
            if(rotation != null) {
                playerEntityData.rotation = rotation;
            }
            if(velocity != null) {
                playerEntityData.velocity = velocity;
            }
            if(acceleration != null) {
                playerEntityData.acceleration = acceleration;
            }
            if(extraVel != null) {
                playerEntityData.extraVel = extraVel;
            }
            if(viewportPitch != null) {
                playerEntityData.viewportPitch = viewportPitch;
            }
            if(physicsFlags != null) {
                playerEntityData.physicsFlags = physicsFlags;
            }
            if(hostility != null) {
                playerEntityData.hostility = hostility;
            }
            if(entityType != null) {
                playerEntityData.entityType = entityType;
            }
            if(mode != null) {
                playerEntityData.mode = mode;
            }
            if(modeTimer != null) {
                playerEntityData.modeTimer = modeTimer;
            }
            if(combo != null) {
                playerEntityData.combo = combo;
            }
            if(lastHitTime != null) {
                playerEntityData.lastHitTime = lastHitTime;
            }
            if(appearance != null) {
                playerEntityData.appearance = appearance;
            }
            if(entityFlags != null) {
                playerEntityData.entityFlags = entityFlags;
            }
            if(roll != null) {
                playerEntityData.roll = roll;
            }
            if(stun != null) {
                playerEntityData.stun = stun;
            }
            if(slow != null) {
                playerEntityData.slow = slow;
            }
            if(ice != null) {
                playerEntityData.ice = ice;
            }
            if(wind != null) {
                playerEntityData.wind = wind;
            }
            if(showPatchTime != null) {
                playerEntityData.showPatchTime = showPatchTime;
            }
            if(entityClass != null) {
                playerEntityData.entityClass = entityClass;
            }
            if(specialization != null) {
                playerEntityData.specialization = specialization;
            }
            if(charge != null) {
                playerEntityData.charge = charge;
            }
            if(unused24 != null) {
                playerEntityData.unused24 = unused24;
            }
            if(unused25 != null) {
                playerEntityData.unused25 = unused25;
            }
            if(rayHit != null) {
                playerEntityData.rayHit = rayHit;
            }
            if(HP != null) {
                playerEntityData.HP = HP;
            }
            if(MP != null) {
                playerEntityData.MP = MP;
            }
            if(block != null) {
                playerEntityData.block = block;
            }
            if(multipliers != null) {
                playerEntityData.multipliers = multipliers;
            }
            if(unused31 != null) {
                playerEntityData.unused31 = unused31;
            }
            if(unused32 != null) {
                playerEntityData.unused32 = unused32;
            }
            if(level != null) {
                playerEntityData.level = level;
            }
            if(XP != null) {
                playerEntityData.XP = XP;
            }
            if(parentOwner != null) {
                playerEntityData.parentOwner = parentOwner;
            }
            if(unused36 != null) {
                playerEntityData.unused36 = unused36;
            }
            if(powerBase != null) {
                playerEntityData.powerBase = powerBase;
            }
            if(unused38 != null) {
                playerEntityData.unused38 = unused38;
            }
            if(unused39 != null) {
                playerEntityData.unused39 = unused39;
            }
            if(spawnPos != null) {
                playerEntityData.spawnPos = spawnPos;
            }
            if(unused41 != null) {
                playerEntityData.unused41 = unused41;
            }
            if(unused42 != null) {
                playerEntityData.unused42 = unused42;
            }
            if(consumable != null) {
                playerEntityData.consumable = consumable;
            }
            if(equipment != null) {
                playerEntityData.equipment = equipment;
            }
            if(name != null) {
                playerEntityData.name = name;
            }
            if(skillDistribution != null) {
                playerEntityData.skillDistribution = skillDistribution;
            }
            if(manaCubes != null) {
                playerEntityData.manaCubes = manaCubes;
            }
        }
        public void Filter(EntityUpdate previous) {
            if(position != null) {
                if(Math.Abs(position.x - previous.position.x) < 32768 &&
                    Math.Abs(position.y - previous.position.y) < 32768 &&
                    Math.Abs(position.z - previous.position.z) < 32768) {
                    position = null;
                }
            }
            rotation = null;
            if(velocity != null) {
                if(Math.Abs(velocity.x) < 1 &&
                    Math.Abs(velocity.y) < 1 &&
                    Math.Abs(velocity.z) < 1) {
                    velocity = null;
                }
            }
            if(extraVel != null) {
                if(Math.Abs(extraVel.x) < 1 &&
                    Math.Abs(extraVel.y) < 1 &&
                    Math.Abs(extraVel.z) < 1) {
                    extraVel = null;
                }
            }
            viewportPitch = null;
            physicsFlags = null;
            if(modeTimer != null && modeTimer != 0) {
                modeTimer = null;
            }
            lastHitTime = null;
            if(roll != null && roll != 600) {
                roll = null;
            }
            if(stun != null && stun < previous.stun) {
                stun = null;
            }
            if(slow != null && slow < previous.slow) {
                slow = null;
            }
            if(ice != null && ice < previous.ice) {
                ice = null;
            }
            if(wind != null && wind < previous.wind) {
                wind = null;
            }
            showPatchTime = null;
            unused24 = null;
            unused25 = null;
            if(rayHit != null) {
                if(previous.mode == 0 ||
                    (Math.Abs(rayHit.x - previous.rayHit.x) > 0.5 &&
                     Math.Abs(rayHit.y - previous.rayHit.y) > 0.5 &&
                     Math.Abs(rayHit.z - previous.rayHit.z) > 0.5)) {
                    rayHit = null;
                }
            }
            MP = null;
            multipliers = null;
            unused31 = null;
            unused32 = null;
            XP = null;
            unused36 = null;
            powerBase = null;
            unused38 = null;
            unused39 = null;
            spawnPos = null;
            unused41 = null;
            unused42 = null;
            skillDistribution = null;
            manaCubes = null;
        }

        private byte[] GetBytes() {
            long bitfield = 0;

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            
            if(position != null) {
                position.Write(writer);
                Tools.SetBit(ref bitfield, true, 0);
            }
            if(rotation != null) {
                rotation.Write(writer);
                Tools.SetBit(ref bitfield, true, 1);
            }
            if(velocity != null) {
                velocity.Write(writer);
                Tools.SetBit(ref bitfield, true, 2);
            }
            if(acceleration != null) {
                acceleration.Write(writer);
                Tools.SetBit(ref bitfield, true, 3);
            }
            if(extraVel != null) {
                extraVel.Write(writer);
                Tools.SetBit(ref bitfield, true, 4);
            }
            if(viewportPitch != null) {
                writer.Write((float)viewportPitch);
                Tools.SetBit(ref bitfield, true, 5);
            }
            if(physicsFlags != null) {
                writer.Write((int)physicsFlags);
                Tools.SetBit(ref bitfield, true, 6);
            }
            if(hostility != null) {
                writer.Write((byte)hostility);
                Tools.SetBit(ref bitfield, true, 7);
            }
            if(entityType != null) {
                writer.Write((int)entityType);
                Tools.SetBit(ref bitfield, true, 8);
            }
            if(mode != null) {
                writer.Write((byte)mode);
                Tools.SetBit(ref bitfield, true, 9);
            }
            if(modeTimer != null) {
                writer.Write((int)modeTimer);
                Tools.SetBit(ref bitfield, true, 10);
            }
            if(combo != null) {
                writer.Write((int)combo);
                Tools.SetBit(ref bitfield, true, 11);
            }
            if(lastHitTime != null) {
                writer.Write((int)lastHitTime);
                Tools.SetBit(ref bitfield, true, 12);
            }
            if(appearance != null) {
                appearance.Write(writer);
                Tools.SetBit(ref bitfield, true, 13);
            }
            if(entityFlags != null) {
                writer.Write((short)entityFlags);
                Tools.SetBit(ref bitfield, true, 14);
            }
            if(roll != null) {
                writer.Write((int)roll);
                Tools.SetBit(ref bitfield, true, 15);
            }
            if(stun != null) {
                writer.Write((int)stun);
                Tools.SetBit(ref bitfield, true, 16);
            }
            if(slow != null) {
                writer.Write((int)slow);
                Tools.SetBit(ref bitfield, true, 17);
            }
            if(ice != null) {
                writer.Write((int)ice);
                Tools.SetBit(ref bitfield, true, 18);
            }
            if(wind != null) {
                writer.Write((int)wind);
                Tools.SetBit(ref bitfield, true, 19);
            }
            if(showPatchTime != null) {
                writer.Write((int)showPatchTime);
                Tools.SetBit(ref bitfield, true, 20);
            }
            if(entityClass != null) {
                writer.Write((byte)entityClass);
                Tools.SetBit(ref bitfield, true, 21);
            }
            if(specialization != null) {
                writer.Write((byte)specialization);
                Tools.SetBit(ref bitfield, true, 22);
            }
            if(charge != null) {
                writer.Write((float)charge);
                Tools.SetBit(ref bitfield, true, 23);
            }
            if(unused24 != null) {
                unused24.Write(writer);
                Tools.SetBit(ref bitfield, true, 24);
            }
            if(unused25 != null) {
                unused25.Write(writer);
                Tools.SetBit(ref bitfield, true, 25);
            }
            if(rayHit != null) {
                rayHit.Write(writer);
                Tools.SetBit(ref bitfield, true, 26);
            }
            if(HP != null) {
                writer.Write((float)HP);
                Tools.SetBit(ref bitfield, true, 27);
            }
            if(MP != null) {
                writer.Write((float)MP);
                Tools.SetBit(ref bitfield, true, 28);
            }
            if(block != null) {
                writer.Write((float)block);
                Tools.SetBit(ref bitfield, true, 29);
            }
            if(multipliers != null) {
                multipliers.Write(writer);
                Tools.SetBit(ref bitfield, true, 30);
            }
            if(unused31 != null) {
                writer.Write((byte)unused31);
                Tools.SetBit(ref bitfield, true, 31);
            }
            if(unused32 != null) {
                writer.Write((byte)unused32);
                Tools.SetBit(ref bitfield, true, 32);
            }
            if(level != null) {
                writer.Write((int)level);
                Tools.SetBit(ref bitfield, true, 33);
            }
            if(XP != null) {
                writer.Write((int)XP);
                Tools.SetBit(ref bitfield, true, 34);
            }
            if(parentOwner != null) {
                writer.Write((long)parentOwner);
                Tools.SetBit(ref bitfield, true, 35);
            }
            if(unused36 != null) {
                writer.Write((long)unused36);
                Tools.SetBit(ref bitfield, true, 36);
            }
            if(powerBase != null) {
                writer.Write((byte)powerBase);
                Tools.SetBit(ref bitfield, true, 37);
            }
            if(unused38 != null) {
                writer.Write((int)unused38);
                Tools.SetBit(ref bitfield, true, 38);
            }
            if(unused39 != null) {
                unused39.Write(writer);
                Tools.SetBit(ref bitfield, true, 39);
            }
            if(spawnPos != null) {
                spawnPos.Write(writer);
                Tools.SetBit(ref bitfield, true, 40);
            }
            if(unused41 != null) {
                unused41.Write(writer);
                Tools.SetBit(ref bitfield, true, 41);
            }
            if(unused42 != null) {
                writer.Write((byte)unused42);
                Tools.SetBit(ref bitfield, true, 42);
            }
            if(consumable != null) {
                consumable.Write(writer);
                Tools.SetBit(ref bitfield, true, 43);
            }
            if(equipment != null) {
                foreach(var item in equipment) {
                    item.Write(writer);
                }
                Tools.SetBit(ref bitfield, true, 44);
            }
            if(name != null) {
                byte[] nameBytes = Encoding.ASCII.GetBytes(name);
                writer.Write(nameBytes);
                writer.Write(new byte[16 - nameBytes.Length]);
                Tools.SetBit(ref bitfield, true, 45);
            }
            if(skillDistribution != null) {
                skillDistribution.Write(writer);
                Tools.SetBit(ref bitfield, true, 46);
            }
            if(manaCubes != null) {
                writer.Write((int)manaCubes);
                Tools.SetBit(ref bitfield, true, 47);
            }

            var data = stream.ToArray();

            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            writer.Write(guid);
            writer.Write(bitfield);
            writer.Write(data);

            return Zlib.Compress(stream.ToArray());
        }
        public void Write(BinaryWriter writer) {
            var data = Data;
            writer.Write(data);
        }
        public void Broadcast(Dictionary<ulong, Player> players, ulong toSkip) {
            byte[] data = this.Data;
            foreach(Player player in new List<Player>(players.Values)) {
                if(player.entityData.guid != toSkip) {
                    SpinWait.SpinUntil(() => player.available);
                    player.available = false;
                    try
                    {
                        player.writer.Write(data);
                    } catch { }
                    player.available = true;
                }
            }
        }
    }
}