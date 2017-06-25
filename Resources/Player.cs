using System.IO;
using System.Net.Sockets;

namespace Resources {
    public class Player {
        //public bool busy = false;
        public TcpClient tcp;
        public BinaryWriter writer;
        public BinaryReader reader;

        public Packet.EntityUpdate entityData = new Packet.EntityUpdate();

        public Player() //constructor
        {
            entityData.bitfield1 = -1;
            entityData.bitfield2 = -1;
        }
    }
}