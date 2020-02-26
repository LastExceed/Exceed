package exceed.packetHandlers

import packets.SectorDiscovery
import packets.*
import exceed.Player

object SectorDiscoveryHandler : PacketHandler<SectorDiscovery> {
	override suspend fun handlePacket(packet: SectorDiscovery, source: Player) {

	}
}