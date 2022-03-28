package packetHandlers

import modules.Balancing
import modules.ClientBugFixes
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

object HitHandler : PacketHandler<Hit> {
	override suspend fun handlePacket(packet: Hit, source: Player) {
		if (ClientBugFixes.ignoreSelfHeal(source, packet)) {
			return
		}

		val damageAdjustedHit = Balancing.adjustDamage(packet, source)

		source.layer.broadcast(
			ServerUpdate(hits = listOf(damageAdjustedHit))
		)
	}
}