namespace Resources {
    public static class UDPDatabase {
        public enum PacketID {
            entityUpdate,
            hit,
            shoot,
            proc,
            chat,
            time,
            block,
            particle,
            sound,
            mission,
            interaction,
            join,
            dc
        }

        public enum Skill { 

        }

        public enum DamageType {
            block = 1,
            normal,
            miss,
            invisible,
            absorb,
            invisible2
        }


    }
}
