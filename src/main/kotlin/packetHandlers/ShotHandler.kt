package packetHandlers

import Player
import me.lastexceed.cubeworldnetworking.packets.*

object ShotHandler : PacketHandler<Shot> {
	override suspend fun handlePacket(packet: Shot, source: Player) {
		val su = ServerUpdate()
		su.shots.add(packet)
		source.layer.broadcast(su, source)
	}
}