namespace Resources {
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
        disconnect,
        petCall, //temp
        dummy = 255 //for establishing connection
    }
    public enum AuthResponse : byte {
        success,
        unknownUser,
        wrongPassword
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
        _placeholder,
        unknown1,
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
    public enum ItemType : byte {
        empty,
        food,
        formula,
        weapon,
        chest,
        gloves,
        boots,
        shoulder,
        amulet,
        ring,
        spirit,
        nugget,
        coin,
        platinumCoin,
        leftovers,
        beak,
        painting,
        vase,
        candle,
        pet,
        petFood,
        quest,
        unknown,
        special,
        lamp
    }
    public enum WeaponSubtype : byte {
        sword,
        axe,
        mace,
        dagger,
        fist,
        longsword,
        bow,
        crossbow,
        boomerand,
        arrow,
        staff,
        wand,
        bracelet,
        shield,
        quiver,
        greatsword,
        greataxe,
        greatmace,
        pitchfork,
        pickaxe,
        torch,
    }
    public enum ItemRarity : byte {
        normal,
        uncommon,
        rare,
        epic,
        legendary,
        mythic,
    }

    public enum EntityClass {
        None,
        Warrior,
        Ranger,
        Mage,
        Rogue
    }

    public enum BanEntry : byte{
        name,
        ip,
        mac,
        reason
    }
}
