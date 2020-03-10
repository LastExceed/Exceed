package exceed.packetHandlers

import Modules.Ultimates
import packets.*
import exceed.*

object PlayerActionHandler : PacketHandler<CreatureAction> {
	override suspend fun handlePacket(packet: CreatureAction, source: Player) {
		when (packet.type) {
			ActionType.Bomb -> {
				GetRidOfThis.notify(source, "bombs are disabled")
			}
			ActionType.Talk -> {
				GetRidOfThis.notify(source, "quests coming soon(tm)")
			}
			ActionType.ObjectInteraction -> {
				GetRidOfThis.notify(source, "object interactions are disabled")
			}
			ActionType.PickUp -> {
				GetRidOfThis.notify(source, "you shouldn't be able to do that")
			}
			ActionType.Drop -> {
				val serverUpdate = ServerUpdate()
				val pickup = Pickup(source.character.id, packet.item)
				serverUpdate.pickups.add(pickup)
				source.send(serverUpdate)
			}
			ActionType.CallPet -> {
				if (!source.character.flags[CreatureFlagsIndex.sprinting.value]) {
					Ultimates.cast(source)
				} else {
					GetRidOfThis.notify(source, "pets are disabled")
				}
			}
			else -> {
				println("unknown creature action type " + packet.type.value)
			}
		}
	}
}