namespace Resources {
    static public class Database {
        public const int mapseed = 8710; //hardcoded for now

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
            attack,
            shoot,
            proc,
            chat,
            time,
            interaction,
            staticUpdate,
            block,
            particle,
            connect,
            disconnect
        }
        public enum DamageType : byte {
            _placeholder,
            block,
            normal,
            miss,
            invisible,
            absorb,
            invisible2
        }
        public enum ProcType : byte {
            bulwalk = 1,
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
        public enum Hostility : byte {
            player,
            enemy,
            unknown2,
            NPC,
            unknown4,
            pet,
            neutral
        }
        public enum Equipment : byte {
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
        public enum StaticRotation : byte {
            south,
            east,
            north,
            west
        }
        public enum Projectile : byte {
            arrow = 1,
            boomerang,
            unknown,
            rock
        }
        public enum ActionType : byte {
            unknown1 = 1,
            talk,
            staticInteraction,
            unknown4,
            pickup,
            drop,
            unknown7,
            callPet
        }
        public enum StaticUpdateType : byte {
            //TODO
        }
        public enum ParticleType : byte {
            //TODO
        }
        public enum LoginResponse : byte {
            success,
            fail,
            banned
        }
    }
}
