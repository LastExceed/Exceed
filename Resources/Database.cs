namespace Resources {
    static public class Database {
        public enum PacketID {
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

        public enum DamageType {
            _placeholder,
            block,
            normal,
            miss,
            invisible,
            absorb,
            invisible2
        }

        public enum Passives {
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

        public enum Hostility {
            player,
            enemy,
            unknown2,
            NPC,
            unknown4,
            pet,
            neutral
        }

        public enum Equipment {
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

        public enum StaticRotation {
            south,
            east,
            north,
            west
        }
        public enum Projectile {

        }
        public enum ProcType {

        }
        public enum StaticUpdateType {

        }
    }
}
