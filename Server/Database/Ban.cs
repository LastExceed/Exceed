namespace Server.Database {
    public class Ban {
        public uint Id { get; set; }
        public int Ip { get; set; }
        public string Mac { get; set; }
        public string Reason { get; set; }

        public uint? UserId { get; set; }
        public virtual User User { get; set; }
    }
}