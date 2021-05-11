package packetHandlers

import Player
import me.lastexceed.cubeworldnetworking.packets.*

interface PacketHandler<T : Packet> {
	suspend fun handlePacket(packet: T, source: Player)
}