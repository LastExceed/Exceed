package exceed.packetHandlers

import packets.ChunkDiscovery
import exceed.Player

object ChunkDiscoveryHandler : PacketHandler<ChunkDiscovery> {
	override suspend fun handlePacket(packet: ChunkDiscovery, source: Player) {

	}
}