using Resources.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Resources.Datagram {
    public class DynamicUpdate : Packet.EntityUpdate {
        public DynamicUpdate() { }

        public DynamicUpdate(byte[] data) : base(Convert(data)) { }

        private static BinaryReader Convert(byte[] data) {
            var reader = new BinaryReader(new MemoryStream(data));
            reader.ReadInt32();
            return reader;
        }

        public byte[] GetData() {
            long bitfield = 0;

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.Write(guid);

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
