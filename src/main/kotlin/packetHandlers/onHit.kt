package packetHandlers

import modules.Balancing
import modules.ClientBugFixes
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

fun onHit(packet: Hit, source: Player) {
	if (ClientBugFixes.ignoreSelfHeal(source, packet)) {
		return
	}
	val target = source.layer.creatures[packet.target] ?: return //in case target disconnected in this moment
	source.layer.broadcast(WorldUpdate(hits = listOf(Balancing.adjustDamage(packet, target))))
}