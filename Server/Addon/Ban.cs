namespace Server.Addon {
    public class Ban {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
        public string Reason { get; set; }

        public Ban(string name, string ip, string mac, string reason = "ban_message_default") {
            Name = name;
            Ip = ip;
            Mac = mac;
            Reason = reason;
        }
    }
}
