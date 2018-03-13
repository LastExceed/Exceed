namespace Resources.Datagram {
    public abstract class Datagram {
        public byte[] data;
        public DatagramID DatagramID {
            get => (DatagramID)data[0];
            internal set => data[0] = (byte)value;
        }

        public Datagram() { }
        public Datagram(byte[] data) {
            this.data = data;
        }
    }
}
