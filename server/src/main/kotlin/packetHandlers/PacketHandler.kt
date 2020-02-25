package exceed.packetHandlers

import exceed.Player
import packets.Packet

interface PacketHandler<T : Packet> {
	fun handlePacket(packet: T, source: Player)
}