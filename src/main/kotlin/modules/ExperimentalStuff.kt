package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.delay

object ExperimentalStuff {
	suspend fun teleport(player: Player, destination: Vector3<Long>) {
		val serverEntity = CreatureUpdate(
			CreatureId(0),
			destination,
			affiliation = Affiliation.Pet,
			animation = Animation.Riding
		)
		player.send(serverEntity)
		delay(100)
		player.clearCreatures() //todo: deduplicate
		player.layer.creatures.forEach { player.send(it.value.toCreatureUpdate()) }//todo: what happens when you receive other creature updates in between?
	}
}

fun registerCommands() {
	ChatCommands.commands["skillpoint"] = Command(true) { caller, parameters ->
		val pickup = Pickup(
			interactor = caller.character.id,
			item = Item.void.copy(typeMajor = Item.Type.Major.ManaCube)
		)
		val miscellaneous = WorldUpdate(pickups = listOf(pickup, pickup, pickup, pickup))

		({ caller.layer.broadcast(miscellaneous) })
	}

	ChatCommands.commands["sfx"] = Command { caller, parameters ->
		val sound = SoundEffect(
			position = Utils.creatureToSoundPosition(caller.character.position),
			sound = SoundEffect.Sound.values[parameters.next().toInt()]
		)

		val worldUpdate = WorldUpdate(soundEffects = listOf(sound))

		({ caller.send(worldUpdate) })
	}

	ChatCommands.commands["army"] = Command { caller, parameters ->
		({
			(2302..2302).forEach {
				caller.send(
					caller.character.copy(
						id = CreatureId(2L),
						//position = caller.character.position.copy(z = caller.character.position.z + 0x100000),
						appearance = Appearance(
							unknownA = 0,
							unknownB = 0,
							hairColor = Vector3(255.toByte(),255.toByte(),255.toByte()),
							unknownC = 0,
							flags = FlagSet<AppearanceFlag>(BooleanArray(16)).apply { this[AppearanceFlag.LockedInPlace] = true },
							creatureSize = Vector3(10f,10f,10f),
							headModel = it.toShort(),
							hairModel = it.toShort(),
							handModel = it.toShort(),
							footModel = it.toShort(),
							bodyModel = it.toShort(),
							tailModel = it.toShort(),
							shoulder2Model = it.toShort(),
							wingModel = it.toShort(),
							headSize = 10f,
							bodySize = 10f,
							handSize = 10f,
							footSize = 10f,
							shoulder2Size = 10f,
							weaponSize = 10f,
							tailSize = 10f,
							shoulder1Size = 10f,
							wingSize = 10f,
							bodyRotation = 0f,
							handRotation = Vector3(0f, 0f,0f),
							feetRotation = 0f,
							wingRotation = 0f,
							tailRotation = 0f,
							bodyOffset = Vector3(0f, 0f, 0f),
							headOffset = Vector3(0f, 0f, 0f),
							handOffset = Vector3(0f, 0f, 0f),
							footOffset = Vector3(0f, 0f, 0f),
							tailOffset = Vector3(0f, 0f, 0f),
							wingOffset = Vector3(0f, 0f, 0f),
						),
						name = it.toString()
					).toCreatureUpdate()
				)
			}
		})
	}

	ChatCommands.commands["speed"] = Command(true) { caller, parameters ->
		val statusEffects =
			StatusEffect.Type.values.map {
				StatusEffect(
					caller.character.id,
					caller.character.id,
					it,
					modifier = 1f,
					duration = 10_000,
					creatureId3 = caller.character.id
				)
			}

		({ caller.send(WorldUpdate(statusEffects = statusEffects)) })
	}

	ChatCommands.commands["block"] = Command(true) { caller, parameters ->
		val we = WorldEdit(
			position = Vector3(
				(caller.character.position.x / Utils.SIZE_BLOCK).toInt(),
				(caller.character.position.y / Utils.SIZE_BLOCK).toInt(),
				(caller.character.position.z / Utils.SIZE_BLOCK).toInt()
			),
			color = Vector3(-1, 0, -1),
			blockType = WorldEdit.BlockType.Solid
		)

		({ caller.send(WorldUpdate(worldEdits = listOf(we))) })
	}

	ChatCommands.commands["oj"] = Command(true) { caller, parameters ->
		{ ModelImport.onJoin(caller) }
	}
}