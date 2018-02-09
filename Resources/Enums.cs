namespace Resources {
    public enum HotkeyID : int{
        CtrlSpace,
        SpecialMove2,
        TeleportToTown
    }
    public enum SpecialMoveID {
        Taunt,
        CursedArrow,
        ArrowRain,
        Shrapnel,
        SmokeBomb,
        IceWave,
        Confusion,
        ShadowStep
    }
    public enum DatagramID : byte {
        DynamicUpdate,
        Attack,
        Projectile,
        Proc,
        Chat,
        Time,
        Interaction,
        StaticUpdate,
        Block,
        Particle,
        RemoveDynamicEntity,
        SpecialMove, //temp
        HolePunch = 255 //for establishing connection
    }
    public enum AuthResponse : byte {
        Success,
        UnknownUser,
        WrongPassword
    }
    public enum BridgeStatus : byte {
        Offline,
        Connected,
        LoggedIn,
        Playing
    }
}
