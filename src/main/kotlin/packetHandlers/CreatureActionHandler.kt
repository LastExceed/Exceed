package packetHandlers

import modules.Ultimates
import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

object CreatureActionHandler : PacketHandler<CreatureAction> {
	override suspend fun handlePacket(packet: CreatureAction, source: Player) {
		when (packet.type) {
			CreatureAction.Type.Bomb -> {
				Server.notify(source, "bombs are disabled")
			}
			CreatureAction.Type.Talk -> {
				Server.notify(source, "quests coming soon(tm)")
			}
			CreatureAction.Type.ObjectInteraction -> {
				Server.notify(source, "object interactions are disabled")
			}
			CreatureAction.Type.PickUp -> {
				Server.notify(source, "you shouldn't be able to do that")
			}
			CreatureAction.Type.Drop -> {
				val pickup = Pickup(source.character.id, packet.item)
				val serverUpdate = ServerUpdate(
					pickups = listOf(pickup)
				)
				source.send(serverUpdate)
			}
			CreatureAction.Type.CallPet -> {
				if (!source.character.flags[CreatureFlag.Sprinting]) {
					Ultimates.cast(source)
				} else {
					//Utils.notify(source, "pets are disabled")
				}
			}
			else -> {
				println("unknown creature action type " + packet.type)
			}
		}
	}
}