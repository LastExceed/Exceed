package packetHandlers

import Creature
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.Utils
import modules.*
import modules.TrafficReduction.filter
import modules.TrafficReduction.isEmpty

object CreatureUpdateHandler : PacketHandler<CreatureUpdate> {
	val sentDatas = mutableMapOf<CreatureId, Creature>()

	override suspend fun handlePacket(packet: CreatureUpdate, source: Player) {
		AntiCheat.inspect(packet, source.character)?.let {
			source.kick(it)
		}
		//Neverland.onCreatureUpdate(source, packet)
		val sentData = sentDatas[source.character.id] ?: source.character.copy().also { sentDatas[source.character.id] = it }
		val filtered = packet.filter(source.character, sentData)
		source.character.update(packet)

		if (!filtered.isEmpty()) {
			source.layer.broadcast(Pvp.makeAttackable(filtered), source)
			sentData.update(filtered)
		}
	}
}