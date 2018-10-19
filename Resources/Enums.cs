namespace Resources {
    public enum HotkeyID : int{
        CtrlSpace,
        SpecialMove2,
        TeleportToTown,
    }
    public enum SpecialMoveID {
        Taunt,
        CursedArrow,
        ArrowRain,
        Shrapnel,
        SmokeBomb,
        IceWave,
        Confusion,
        ShadowStep,
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
        HolePunch = 255, //for establishing connection
    }
    public enum ServerPacketID : byte {
        VersionCheck,
        Login,
        Logout,
        Register,
        BTFO,
        Kick,
    }
    public enum AuthResponse : byte {
        Success,
        UnknownUser,
        WrongPassword,
        Banned,
        Unverified,//bridge version not checked
        UserAlreadyLoggedIn,
        AccountAlreadyActive,
    }
    public enum RegisterResponse : byte {
        Success,
        EmailTaken,
        UsernameTaken,
        InvalidInput,
    }
    public enum BridgeStatus : byte {
        Offline,
        Connected,
        LoggedIn,
        Playing,
    }
    public enum RoleID : byte
    {
        Default,
        Vip,
        Mod,
        Admin
    }
    public enum PromoteResponse : byte
    {
        Success,
        InvalidTarget
    }
}
