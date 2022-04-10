package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import modules.*

object CreatureUpdateHandler : PacketHandler<CreatureUpdate> {
	override suspend fun handlePacket(packet: CreatureUpdate, source: Player) {
		AntiCheat.inspect(packet, source.character)?.let {
			source.kick(it)
		}
		//Neverland.onCreatureUpdate(source, packet)
		val filtered = TrafficReduction.onCreatureUpdate(packet, source)
		source.character.update(packet)
		if (filtered == null) return
		source.layer.broadcast(Pvp.makeAttackable(filtered), source)
	}
}