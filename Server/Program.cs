namespace Server {
    class Program {
        static void Main(string[] args) {
            ServerTCP server = new ServerTCP();
            server.Listen();
        }
    }
}
