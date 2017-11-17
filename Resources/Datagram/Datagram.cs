namespace Resources.Datagram {
    public abstract class Datagram {
        public DatagramID DatagramID {
            get => (DatagramID)data[0];
            internal set => data[0] = (byte)value;
        }

        public byte[] data;
        
        public Datagram(byte[] data) {
            this.data = data;
        }

        public Datagram() { }
    }
}
