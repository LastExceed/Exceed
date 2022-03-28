package packetHandlers

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import kotlinx.coroutines.*
import modules.*

object CreatureUpdateHandler : PacketHandler<CreatureUpdate> {
	override suspend fun handlePacket(packet: CreatureUpdate, source: Player) {
		AntiCheat.inspect(packet, source.character)?.let {
			println(it + source.character.name)
			Server.notify(source, it)
			//delay(100)
			//source.kick(ACmessage)
		}
		//Neverland.onCreatureUpdate(source, packet)
		source.character.update(packet)
		source.layer.broadcast(Pvp.makeAttackable(packet), source)
	}
}