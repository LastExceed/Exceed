package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

object ShotHandler : PacketHandler<Shot> {
	override suspend fun handlePacket(packet: Shot, source: Player) {
		source.layer.broadcast(
			ServerUpdate(shots = listOf(packet)),
			source
		)
	}
}