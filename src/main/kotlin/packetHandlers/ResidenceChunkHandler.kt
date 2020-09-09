package packetHandlers

import packets.ResidenceChunk
import Player

object ResidenceChunkHandler : PacketHandler<ResidenceChunk> {
	override suspend fun handlePacket(packet: ResidenceChunk, source: Player) {

	}
}