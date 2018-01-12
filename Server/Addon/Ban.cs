namespace Server.Addon {
    public class Ban {
        public string Name;
        public string Ip;
        public string Mac;
        public string Reason;

        public Ban(string name, string ip, string mac, string reason = "ban_message_default") {
            Name = name;
            Ip = ip;
            Mac = mac;
            Reason = reason;
        }
    }
}
