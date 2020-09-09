package packetHandlers

import packets.ResidenceBiome
import Player

object ResidenceBiomeHandler : PacketHandler<ResidenceBiome> {
	override suspend fun handlePacket(packet: ResidenceBiome, source: Player) {

	}
}