package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

interface PacketHandler<T : Packet> {
	suspend fun handlePacket(packet: T, source: Player)
}