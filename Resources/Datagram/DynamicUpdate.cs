using Resources.Packet;

namespace Resources.Datagram {
    public static class DynamicUpdate {
        public static byte[] CreateDatagram(this EntityUpdate e) {
            byte[] data = e.Data;
            byte[] datagram = new byte[data.Length + 1];
            data.CopyTo(datagram, 1);
            return datagram;
        }
    }
}
