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

		({ caller.send(miscellaneous) })
	}

	ChatCommands.commands["reload"] = Command { caller, parameters ->
		{
			caller.clearCreatures()
			caller.layer.creatures.forEach { caller.send(it.value.toCreatureUpdate()) }
		}
	}

	ChatCommands.commands["speed"] = Command(true) { caller, parameters ->
		val camo = StatusEffect(
			caller.character.id,
			caller.character.id,
			StatusEffect.Type.Camouflage,
			modifier = 99999f,
			duration = 10_000,
			creatureId3 = caller.character.id
		)
		val speed = StatusEffect(
			caller.character.id,
			caller.character.id,
			StatusEffect.Type.Swiftness,
			modifier = 99999f,
			duration = 10_000,
			creatureId3 = caller.character.id
		)

		({ caller.send(WorldUpdate(statusEffects = listOf(speed, camo))) })
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