namespace Server.Database {
    public class Login {
        public int Ip { get; set; }
        public string Mac { get; set; }

        public uint UserId { get; set; }
        public virtual User User { get; set; }
    }
}