package packetHandlers

import modules.Ultimates
import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

fun onCreatureAction(packet: CreatureAction, source: Player) {
	when (packet.type) {
		CreatureAction.Type.Bomb -> {
			source.notify("bombs are disabled")
		}
		CreatureAction.Type.Talk -> {
			source.notify("quests coming soon(tm)")
		}
		CreatureAction.Type.ObjectInteraction -> {
			source.notify("object interactions are disabled")
		}
		CreatureAction.Type.PickUp -> {
			source.notify("you shouldn't be able to do that")
		}
		CreatureAction.Type.Drop -> {
			println(packet)
			val pickup = Pickup(source.character.id, packet.item)
			source.send(WorldUpdate(pickups = listOf(pickup)))
		}
		CreatureAction.Type.CallPet -> {
			if (!source.character.flags[CreatureFlag.Sprinting]) {
				Ultimates.cast(source)
			} else {
				//Utils.notify(source, "pets are disabled")
			}
		}
		else -> println("unknown creature action type " + packet.type)
	}
}