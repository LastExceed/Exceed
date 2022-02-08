package packetHandlers

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

object ResidenceChunkHandler : PacketHandler<ResidenceChunk> {
	override suspend fun handlePacket(packet: ResidenceChunk, source: Player) {

	}
}