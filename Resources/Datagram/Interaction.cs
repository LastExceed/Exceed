using System;

namespace Resources.Datagram {
    public class Interaction {
        public Database.DatagramID DatagramID {
            get { return (Database.DatagramID)data[0]; }
            private set { data[0] = (byte)value; }
        }

        //TODO

        public byte[] data;

        public Interaction() {
            //data = new byte[???];
            DatagramID = Database.DatagramID.interaction;
        }
        public Interaction(byte[] data) {
            this.data = data;
        }
    }
}
