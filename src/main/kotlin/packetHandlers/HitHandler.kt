package packetHandlers

import modules.Balancing
import modules.ClientBugFixes
import Player
import packets.*

object HitHandler : PacketHandler<Hit> {
	override suspend fun handlePacket(packet: Hit, source: Player) {
		if (ClientBugFixes.ignoreSelfHeal(source, packet)) {
			return
		}

		val su = ServerUpdate()
		su.hits.add(Balancing.adjustDamage(packet, source))
		source.layer.broadcast(su)
	}
}