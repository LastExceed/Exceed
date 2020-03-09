package exceed.packetHandlers

import packets.ResidenceChunk
import exceed.Player

object ResidenceChunkHandler : PacketHandler<ResidenceChunk> {
	override suspend fun handlePacket(packet: ResidenceChunk, source: Player) {

	}
}