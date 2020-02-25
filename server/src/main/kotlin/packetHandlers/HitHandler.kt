package exceed.packetHandlers

import Modules.Balancing
import Modules.ClientBugFixes
import packets.*
import exceed.Player

object HitHandler : PacketHandler<Hit> {
	override fun handlePacket(packet: Hit, source: Player) {
		if (ClientBugFixes.ignoreSelfHeal(source, packet)) {
			return
		}

		val su = ServerUpdate()
		su.hits.add(Balancing.adjustDamage(packet, source))
		source.layer.broadcast(su)
	}
}