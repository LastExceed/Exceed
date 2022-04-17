package packetHandlers

import modules.Ultimates
import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.*

private val dropLists = mutableMapOf<Vector2<Int>, MutableList<Drop>>()

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
			val dropList = dropLists[packet.chunk]!!
			val drop = dropList.removeAt(packet.itemIndex)

			if (dropList.isEmpty()) dropLists.remove(packet.chunk)

			val worldUpdate = WorldUpdate(
				chunkLoots = listOf(ChunkLoot(packet.chunk, dropList)),
				soundEffects = listOf(
					SoundEffect(
						Utils.creatureToSoundPosition(source.character.position),
						SoundEffect.Sound.Pickup
					)
				)
			)
			source.layer.broadcast(worldUpdate, source)
			source.send(worldUpdate.copy(pickups = listOf(Pickup(source.character.id, drop.item))))
		}
		CreatureAction.Type.Drop -> {
			val chunk = source.character.zone
			val drop = Drop(
				packet.item,
				source.character.position.copy(z = source.character.position.z - 0x10000),
				source.character.rotation.z,
				scale = 0.1f
			)
			val dropList = dropLists.getOrPut(chunk) { mutableListOf() }
			val dropListCopy = dropList.toList()
			dropList.add(drop)

			source.layer.broadcast(
				WorldUpdate(
					chunkLoots = listOf(
						ChunkLoot(
							chunk,
							dropListCopy + drop.copy(droptime = 500)
						)
					),
					soundEffects = listOf(
						SoundEffect(
							Utils.creatureToSoundPosition(source.character.position),
							SoundEffect.Sound.Drop
						)
					)
				)
			)
			source.scope.launch {
				val positionSnapshot = source.character.position.copy()
				delay(500)
				source.send(
					WorldUpdate(
						soundEffects = listOf(
							SoundEffect(
								Utils.creatureToSoundPosition(positionSnapshot),
								SoundEffect.Sound.DropItem
							)
						)
					)
				)
			}
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