package packetHandlers

import modules.Ultimates
import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import com.github.lastexceed.cubeworldnetworking.utils.*

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
			val dropList = source.layer.dropLists[packet.chunk] ?: return //when item was dropped in single player
			val drop = dropList.getOrNull(packet.itemIndex) ?: return

			source.layer.removeGroundItem(dropList, drop)

			source.send(
				WorldUpdate(
					pickups = listOf(Pickup(source.character.id, drop.item)),
					soundEffects = listOf(
						SoundEffect(
							Utils.creatureToSoundPosition(source.character.position),
							SoundEffect.Sound.Pickup
						)
					)
				)
			)
		}
		CreatureAction.Type.Drop -> {
			source.layer.addGroundItem(
				packet.item,
				source.character.position.copy(z = source.character.position.z - (source.character.appearance.creatureSize.z / 2f * Utils.SIZE_BLOCK).toLong()),
				source.character.rotation.z
			)
		}
		CreatureAction.Type.CallPet -> {
			if (!source.character.flags[CreatureFlag.Sprinting]) {
				Ultimates.cast(source)
			} else {
				//Utils.notify(source, "pets are disabled")
			}
		}
		else -> error("unknown creature action type " + packet.type)
	}
}