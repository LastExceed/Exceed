package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.CreatureUpdate
import modules.*

object CreatureUpdateHandler : PacketHandler<CreatureUpdate> {
	override suspend fun handlePacket(packet: CreatureUpdate, source: Player) {
		AntiCheat.inspect(packet, source.character)?.let {
			source.kick(it)
		}
		//Neverland.onCreatureUpdate(source, packet)
		source.character.update(packet)

		if (TrafficReduction.shouldIgnore(packet)) return
		source.layer.broadcast(Pvp.makeAttackable(packet), source)
	}
}