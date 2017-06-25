namespace Resources {
    static public class Database {
        public enum PacketID : int {
            entityUpdate,
            multiEntityUpdate,
            entityUpdatesFinished,
            unknown3,
            serverUpdate,
            time,
            entityAction,
            hit,
            passiveProc,
            shoot,
            chat,
            chunk,
            sector,
            unknown13,
            unknown14,
            mapseed,
            joinPacket,
            version,
            serverFull
        }

        public enum DatagramID : byte {
            entityUpdate,
            hit,
            shoot,
            proc,
            chat,
            time,
            interaction,
            staticUpdate,
            block,
            particle,
            join,
            disconnect,
            players = 255
        }

        public enum DamageTypes {
            _placeholder,
            block,
            normal,
            miss,
            invisible,
            absorb,
            invisible2
        }

        public enum Passives : byte{
            _placeholder,
            bulwalk,
            warFrenzy,
            camouflage,
            poison,
            unknownA,
            manashield,
            unknownB,
            unknownC,
            fireSpark,
            intuition, //scout
            elusivenes, //ninja
            swiftness
        }

        public enum Hostility : byte{
            player,
            enemy,
            unknown2,
            NPC,
            unknown4,
            pet,
            neutral
        }

        public enum Equipment : byte{
            unknown,
            neck,
            chest,
            feet,
            hands,
            shoulder,
            leftWeapon,
            rightWeapon,
            leftRing,
            rightRing,
            lamp,
            special,
            pet
        }

        public enum StaticRotation : byte{
            south,
            east,
            north,
            west
        }
 
        public enum Projectile {
            arrow = 1,
            boomerang,
            unknown,
            rock
        }
        public enum ProcType {

        }
        public enum StaticUpdateType {

        }
    }
}
