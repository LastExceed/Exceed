package exceed.packetHandlers

import packets.ResidenceBiome
import exceed.Player

object ResidenceBiomeHandler : PacketHandler<ResidenceBiome> {
	override suspend fun handlePacket(packet: ResidenceBiome, source: Player) {

	}
}