using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Resources.Datagram {
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
        public Packet.Part.Appearance appearance;
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
        public Packet.Part.Multipliers multipliers;
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
        public Packet.Part.Item consumable;
        public Packet.Part.Item[] equipment;
        public string name;
        public Packet.Part.SkillDistribution skillDistribution;
        public int? manaCubes;

        public EntityUpdate() {

        }

        public EntityUpdate(byte[] data) {
            var c = new byte[BitConverter.ToInt32(data,1)];
            Array.Copy(data, 5, c, 0, c.Length);
            
            byte[] uncompressed = Zlib.Uncompress(c);

            MemoryStream stream = new MemoryStream(uncompressed);
            BinaryReader r = new BinaryReader(stream);
            guid = r.ReadUInt64();
            var bitfield = r.ReadInt64();

            //position
            if(Tools.GetBit(bitfield, 0)) {
                position = new LongVector(r);
            }
            //orientation
            if(Tools.GetBit(bitfield, 1)) {
                rotation = new FloatVector(r);
            }
            //velocity
            if(Tools.GetBit(bitfield, 2)) {
                velocity = new FloatVector(r);
            }
            //acceleration
            if(Tools.GetBit(bitfield, 3)) {
                acceleration = new FloatVector(r);
            }
            //extra veloctiy
            if(Tools.GetBit(bitfield, 4)) {
                extraVel = new FloatVector(r);
            }
            //viewport pitch
            if(Tools.GetBit(bitfield, 5)) {
                viewportPitch = r.ReadSingle();
            }
            //physics flags
            if(Tools.GetBit(bitfield, 6)) {
                physicsFlags = r.ReadInt32();
            }
            //hostile?
            if(Tools.GetBit(bitfield, 7)) {
                hostility = r.ReadByte();
            }
            //entity type
            if(Tools.GetBit(bitfield, 8)) {
                entityType = r.ReadInt32();
            }
            //current mode
            if(Tools.GetBit(bitfield, 9)) {
                mode = r.ReadByte();
            }
            //mode start time
            if(Tools.GetBit(bitfield, 10)) {
                modeTimer = r.ReadInt32();
            }
            //combo
            if(Tools.GetBit(bitfield, 11)) {
                combo = r.ReadInt32();
            }
            //last hittime
            if(Tools.GetBit(bitfield, 12)) {
                lastHitTime = r.ReadInt32();
            }
            //appearance data
            if(Tools.GetBit(bitfield, 13)) {
                appearance = new Packet.Part.Appearance(r);
            }
            //entity flags
            if(Tools.GetBit(bitfield, 14)) {
                entityFlags = r.ReadInt16();
            }
            //roll
            if(Tools.GetBit(bitfield, 15)) {
                roll = r.ReadInt32();
            }
            //stun
            if(Tools.GetBit(bitfield, 16)) {
                stun = r.ReadInt32();
            }
            //slowed?
            if(Tools.GetBit(bitfield, 17)) {
                slow = r.ReadInt32();
            }
            //make blue time (ice)
            if(Tools.GetBit(bitfield, 18)) {
                ice = r.ReadInt32();
            }
            //speed up time (wind)
            if(Tools.GetBit(bitfield, 19)) {
                wind = r.ReadInt32();
            }
            //show patch time?
            if(Tools.GetBit(bitfield, 20)) {
                showPatchTime = r.ReadInt32();
            }
            //public class
            if(Tools.GetBit(bitfield, 21)) {
                entityClass = r.ReadByte();
            }
            //subpublic class
            if(Tools.GetBit(bitfield, 22)) {
                specialization = r.ReadByte();
            }
            //charge
            if(Tools.GetBit(bitfield, 23)) {
                charge = r.ReadSingle();
            }
            //unused vector
            if(Tools.GetBit(bitfield, 24)) {
                unused24 = new FloatVector(r);
            }
            //unused vector
            if(Tools.GetBit(bitfield, 25)) {
                unused25 = new FloatVector(r);
            }
            //ray hit
            if(Tools.GetBit(bitfield, 26)) {
                rayHit = new FloatVector(r);
            }
            //HP
            if(Tools.GetBit(bitfield, 27)) {
                HP = r.ReadSingle();
            }
            //MP
            if(Tools.GetBit(bitfield, 28)) {
                MP = r.ReadSingle();
            }
            //block power
            if(Tools.GetBit(bitfield, 29)) {
                block = r.ReadSingle();
            }
            //multipliers
            if(Tools.GetBit(bitfield, 30)) {
                multipliers = new Packet.Part.Multipliers(r);
            }
            //unused
            if(Tools.GetBit(bitfield, 31)) {
                unused31 = r.ReadByte();
            }
            //unused
            if(Tools.GetBit(bitfield, 32)) {
                unused32 = r.ReadByte();
            }
            //level
            if(Tools.GetBit(bitfield, 33)) {
                level = r.ReadInt32();
            }
            //xp
            if(Tools.GetBit(bitfield, 34)) {
                XP = r.ReadInt32();
            }
            //parent owner?
            if(Tools.GetBit(bitfield, 35)) {
                parentOwner = r.ReadInt64();
            }
            //unused *2
            if(Tools.GetBit(bitfield, 36)) {
                unused36 = r.ReadInt64();
            }
            //power base
            if(Tools.GetBit(bitfield, 37)) {
                powerBase = r.ReadByte();
            }
            //unused
            if(Tools.GetBit(bitfield, 38)) {
                unused38 = r.ReadInt32();
            }
            //unused vector
            if(Tools.GetBit(bitfield, 39)) {
                unused39 = new IntVector(r);
            }
            //spawn position
            if(Tools.GetBit(bitfield, 40)) {
                spawnPos = new LongVector(r);
            }
            //unused vector
            if(Tools.GetBit(bitfield, 41)) {
                unused41 = new IntVector(r);
            }
            //unused
            if(Tools.GetBit(bitfield, 42)) {
                unused42 = r.ReadByte();
            }
            //consumable
            if(Tools.GetBit(bitfield, 43)) {
                consumable = new Packet.Part.Item(r);
            }
            //equipment
            if(Tools.GetBit(bitfield, 44)) {
                equipment = new Packet.Part.Item[13];
                for(int i = 0; i < 13; i++) {
                    equipment[i] = new Packet.Part.Item(r);
                }
            }
            //name
            if(Tools.GetBit(bitfield, 45)) {
                name = new string(r.ReadChars(16));
                name = name.Substring(0, name.IndexOf("\0"));
            }
            //skills (11*4)
            if(Tools.GetBit(bitfield, 46)) {
                skillDistribution = new Packet.Part.SkillDistribution(r);
            }
            //mama cubes 
            if(Tools.GetBit(bitfield, 47)) {
                manaCubes = r.ReadInt32();
            }
        }

        public byte[] GetData() {
            long bitfield = 0;

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            //position
            if(position != null) {
                position.Write(writer);
                Tools.SetBit(ref bitfield, true, 0);
            }
            //orientation
            if(rotation != null) {
                rotation.Write(writer);
                Tools.SetBit(ref bitfield, true, 1);
            }
            //velocity
            if(velocity != null) {
                velocity.Write(writer);
                Tools.SetBit(ref bitfield, true, 2);
            }
            //acceleration
            if(acceleration != null) {
                acceleration.Write(writer);
                Tools.SetBit(ref bitfield, true, 3);
            }
            //extra veloctiy
            if(extraVel != null) {
                extraVel.Write(writer);
                Tools.SetBit(ref bitfield, true, 4);
            }
            //viewport pitch
            if(viewportPitch != null) {
                writer.Write((float)viewportPitch);
                Tools.SetBit(ref bitfield, true, 5);
            }
            //physics flags
            if(physicsFlags != null) {
                writer.Write((int)physicsFlags);
                Tools.SetBit(ref bitfield, true, 6);
            }
            //hostile?
            if(hostility != null) {
                writer.Write((byte)hostility);
                Tools.SetBit(ref bitfield, true, 7);
            }
            //entity type
            if(entityType != null) {
                writer.Write((int)entityType);
                Tools.SetBit(ref bitfield, true, 8);
            }
            //current mode
            if(mode != null) {
                writer.Write((byte)mode);
                Tools.SetBit(ref bitfield, true, 9);
            }
            //mode start time
            if(modeTimer != null) {
                writer.Write((int)modeTimer);
                Tools.SetBit(ref bitfield, true, 10);
            }
            //combo
            if(combo != null) {
                writer.Write((int)combo);
                Tools.SetBit(ref bitfield, true, 11);
            }
            //last hittime
            if(lastHitTime != null) {
                writer.Write((int)lastHitTime);
                Tools.SetBit(ref bitfield, true, 12);
            }
            //appearance data
            if(appearance != null) {
                appearance.Write(writer);
                Tools.SetBit(ref bitfield, true, 13);
            }
            //entity flags
            if(entityFlags != null) {
                writer.Write((short)entityFlags);
                Tools.SetBit(ref bitfield, true, 14);
            }
            //roll
            if(roll != null) {
                writer.Write((int)roll);
                Tools.SetBit(ref bitfield, true, 15);
            }
            //stun
            if(stun != null) {
                writer.Write((int)stun);
                Tools.SetBit(ref bitfield, true, 16);
            }
            //slowed?
            if(slow != null) {
                writer.Write((int)slow);
                Tools.SetBit(ref bitfield, true, 17);
            }
            //make blue time (ice)
            if(ice != null) {
                writer.Write((int)ice);
                Tools.SetBit(ref bitfield, true, 18);
            }
            //speed up time (wind)
            if(wind != null) {
                writer.Write((int)wind);
                Tools.SetBit(ref bitfield, true, 19);
            }
            //show patch time?
            if(showPatchTime != null) {
                writer.Write((int)showPatchTime);
                Tools.SetBit(ref bitfield, true, 20);
            }
            //public class
            if(entityClass != null) {
                writer.Write((byte)entityClass);
                Tools.SetBit(ref bitfield, true, 21);
            }
            //subpublic class
            if(specialization != null) {
                writer.Write((byte)specialization);
                Tools.SetBit(ref bitfield, true, 22);
            }
            //charge
            if(charge != null) {
                writer.Write((float)charge);
                Tools.SetBit(ref bitfield, true, 23);
            }
            //unused vector
            if(unused24 != null) {
                unused24.Write(writer);
                Tools.SetBit(ref bitfield, true, 24);
            }
            //unused vector
            if(unused25 != null) {
                unused25.Write(writer);
                Tools.SetBit(ref bitfield, true, 25);
            }
            //ray hit
            if(rayHit != null) {
                rayHit.Write(writer);
                Tools.SetBit(ref bitfield, true, 26);
            }
            //HP
            if(HP != null) {
                writer.Write((float)HP);
                Tools.SetBit(ref bitfield, true, 27);
            }
            //MP
            if(MP != null) {
                writer.Write((float)MP);
                Tools.SetBit(ref bitfield, true, 28);
            }
            //block power
            if(block != null) {
                writer.Write((float)block);
                Tools.SetBit(ref bitfield, true, 29);
            }
            //multipliers
            if(multipliers != null) {
                multipliers.Write(writer);
                Tools.SetBit(ref bitfield, true, 30);
            }
            //unused
            if(unused31 != null) {
                writer.Write((byte)unused31);
                Tools.SetBit(ref bitfield, true, 31);
            }

            //unused
            if(unused32 != null) {
                writer.Write((byte)unused32);
                Tools.SetBit(ref bitfield, true, 32);
            }
            //level
            if(level != null) {
                writer.Write((int)level);
                Tools.SetBit(ref bitfield, true, 33);
            }
            //xp
            if(XP != null) {
                writer.Write((int)XP);
                Tools.SetBit(ref bitfield, true, 34);
            }
            //parent owner?
            if(parentOwner != null) {
                writer.Write((long)parentOwner);
                Tools.SetBit(ref bitfield, true, 35);
            }
            //unused *2
            if(unused36 != null) {
                writer.Write((long)unused36);
                Tools.SetBit(ref bitfield, true, 36);
            }
            //power base
            if(powerBase != null) {
                writer.Write((byte)powerBase);
                Tools.SetBit(ref bitfield, true, 37);
            }
            //unused
            if(unused38 != null) {
                writer.Write((int)unused38);
                Tools.SetBit(ref bitfield, true, 38);
            }
            //unused vector
            if(unused39 != null) {
                unused39.Write(writer);
                Tools.SetBit(ref bitfield, true, 39);
            }
            //spawn position
            if(spawnPos != null) {
                spawnPos.Write(writer);
                Tools.SetBit(ref bitfield, true, 40);
            }
            //unused vector
            if(unused41 != null) {
                unused41.Write(writer);
                Tools.SetBit(ref bitfield, true, 41);
            }
            //unused
            if(unused42 != null) {
                writer.Write((byte)unused42);
                Tools.SetBit(ref bitfield, true, 42);
            }
            //consumable
            if(consumable != null) {
                consumable.Write(writer);
                Tools.SetBit(ref bitfield, true, 43);
            }
            //equipment
            if(equipment != null) {
                foreach(Packet.Part.Item item in equipment) {
                    item.Write(writer);
                }
                Tools.SetBit(ref bitfield, true, 44);
            }
            //name
            if(name != null) {
                byte[] nameBytes = Encoding.UTF8.GetBytes(name);
                writer.Write(nameBytes);
                writer.Write(new byte[16 - nameBytes.Length]);
                Tools.SetBit(ref bitfield, true, 45);
            }
            //skills (11*4)
            if(skillDistribution != null) {
                skillDistribution.Write(writer);
                Tools.SetBit(ref bitfield, true, 46);
            }
            //mama cubes
            if(manaCubes != null) {
                writer.Write((int)manaCubes);
                Tools.SetBit(ref bitfield, true, 47);
            }

            var asd = stream.ToArray();

            stream = new MemoryStream();
            writer = new BinaryWriter(stream);

            writer.Write(guid);
            writer.Write(bitfield);
            writer.Write(asd);

            byte[] compressed = Zlib.Compress(stream.ToArray());
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            writer.Write(0);
            writer.Write(compressed.Length);
            writer.Write(compressed);
            byte[] data = stream.ToArray();
            stream.Dispose();
            return data;
        }
    }
}
