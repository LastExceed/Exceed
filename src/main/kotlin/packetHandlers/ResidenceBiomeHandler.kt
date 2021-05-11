package packetHandlers

import me.lastexceed.cubeworldnetworking.packets.*
import Player

object ResidenceBiomeHandler : PacketHandler<ResidenceBiome> {
	override suspend fun handlePacket(packet: ResidenceBiome, source: Player) {

	}
}