package packetHandlers

import modules.Ultimates
import packets.*
import Player

object PlayerActionHandler : PacketHandler<CreatureAction> {
	override suspend fun handlePacket(packet: CreatureAction, source: Player) {
		when (packet.type) {
			ActionType.Bomb -> {
				Utils.notify(source, "bombs are disabled")
			}
			ActionType.Talk -> {
				Utils.notify(source, "quests coming soon(tm)")
			}
			ActionType.ObjectInteraction -> {
				Utils.notify(source, "object interactions are disabled")
			}
			ActionType.PickUp -> {
				Utils.notify(source, "you shouldn't be able to do that")
			}
			ActionType.Drop -> {
				val serverUpdate = ServerUpdate()
				val pickup = Pickup(source.character.id, packet.item)
				serverUpdate.pickups.add(pickup)
				source.send(serverUpdate)
			}
			ActionType.CallPet -> {
				if (!source.character.flags[CreatureFlag.sprinting]) {
					Ultimates.cast(source)
				} else {
					//Utils.notify(source, "pets are disabled")
				}
			}
			else -> {
				println("unknown creature action type " + packet.type.value)
			}
		}
	}
}