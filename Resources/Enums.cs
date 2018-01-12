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
        specialMove, //temp
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
    public enum ProjectileType : byte {
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
        normal,
        fire,
        unknown2,
        noSpreadNoRotation,
        noGravity
    }
    public enum Mode : byte{
        idle,
        war_dual_wield001,
        war_dual_wield002,
        unknown3,//similar to daggers2 018
        unknown4,//similar to daggers1 019,
        carge_after_longsword,
        fists1,
        fists2,
        charge_during_shield ,
        shield2,
        shield1,
        charge_after_noweapon,
        unknown012,//(dual wield) slash?
        longsword2,
        longsword1,
        unknown015,//2hand weapon,
        unknown016,//similar to daggers after charge 017,
        charge_after_daggers,
        daggers2,
        daggers1,
        fists_after_kick,
        unknown021,//similar to fists after kick 020,
        shoot_arrow,//bow,crossbow
        charge_after_crossbow,
        charge_during_crossbow,
        charge_during_bow,
        boomerang,//also boomerang after charge
        charge_during_boomerang,
        beam_instant,
        unknown029,//dual wield, unable to attack
        staff_fire,
        charge_after_staff_fire,
        staff_water,
        charge_after_staff_water,
        after_healing_streamm,
        unknown035,//summoning?
        unknown036,//dual wield auto charge
        charge_after_bracelet,
        wand,
        bracelets2_fire,
        bracelets1_fire,
        bracelets2_water,
        bracelets1_water,
        charge_after_wand_C,
        wand_B,
        charge_after_wand_B,
        charge_after_wand,
        smash,
        after_intercept,
        after_teleport,
        shuriken,
        unknown051,
        unknown052,
        unknown053,
        after_smash,
        charge_after_bow,
        unknown056,//==============================
        greatweapon1,
        greatweapon3,
        charge_during_greatweapon,
        unknown060,//2hand
        charge_after_greatweapon,
        unknown062,//sword,
        charge_during_noweapon,
        charge_during_dual_wield,//?
        spinA,//2hand
        spinB,//2hand
        greatweapon2,
        boss_skill_knockdown1,
        boss_skill_knockdown22,
        boss_skill_spinkick,
        boss_skill_block,
        boss_skill_spin,
        boss_skill_cry,
        boss_skill_stomp,
        boss_skill_kick,
        boss_skill_knockdown3,
        boss_skill_knockdown4,
        boss_skill_knockdown5,
        stealth,
        drinking_potion,
        eat,
        pet_food,
        sitting,
        sleeping,
        unknown085,//talking to npc?,
        cyclone,
        fireexplosion_big,
        fireexplosion_after,
        lava,
        splash,
        earth_shatter,
        clone,
        spin_run,
        firebeam,//small
        fireray,//big
        after_shuriken,
        invalid097,
        unknown098,//blocking chaos
        invalid099,
        invalid100,
        superbulwalk,
        invalid102,
        supermanashield,
        charge_after_shield,
        teleport_to_city,
        riding,
        boat,
        boulder_toss,
        manacube,
        unknown110,//weird                                                                 
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
    public enum HotkeyID : int{
        ctrlSpace,
        specialmove2,
        teleport_to_town
    }
    public enum SpecialMoveID {
        taunt,
        cursedArrow,
        arrowRain,
        shrapnel,
        smokeBomb,
        iceWave,
        confusion,
        shadowStep
    }
}
