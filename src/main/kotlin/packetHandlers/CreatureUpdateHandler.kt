package packetHandlers

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import kotlinx.coroutines.*
import modules.*

object CreatureUpdateHandler : PacketHandler<CreatureUpdate> {
	override suspend fun handlePacket(packet: CreatureUpdate, source: Player) {
		val ACmessage = AntiCheat.inspect(packet, source.character)
		if (ACmessage != null) {
			println(ACmessage + source.character.name)
			Server.notify(source, ACmessage)
			//delay(100)
			//source.kick(ACmessage)
		}
		//Neverland.onCreatureUpdate(source, packet)
		source.character.update(packet)
		source.layer.broadcast(Pvp.makeAttackable(packet), source)
	}
}