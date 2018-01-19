namespace Resources {
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
    public enum DatagramID : byte {
        dynamicUpdate,
        attack,
        shoot,
        proc,
        chat,
        time,
        interaction,
        staticUpdate,
        block,
        particle,
        RemoveDynamicEntity,
        specialMove, //temp
        dummy = 255 //for establishing connection
    }
    public enum AuthResponse : byte {
        success,
        unknownUser,
        wrongPassword
    }
}
