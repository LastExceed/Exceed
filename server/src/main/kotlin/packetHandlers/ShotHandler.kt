package exceed.packetHandlers

import exceed.Player
import packets.*

object ShotHandler : PacketHandler<Shot> {
	override suspend fun handlePacket(packet: Shot, source: Player) {
		val su = ServerUpdate()
		su.shots.add(packet)
		source.layer.broadcast(su, source)
	}
}