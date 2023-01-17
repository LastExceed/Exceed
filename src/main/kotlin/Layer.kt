import com.andreapivetta.kolor.*
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.*
import modules.*

data class Layer(
	val creatures: MutableMap<CreatureId, Creature> = mutableMapOf(),
	val players: MutableMap<CreatureId, Player> = mutableMapOf(),
	val dropLists: MutableMap<Vector2<Int>, MutableList<Drop>> = mutableMapOf()
) {
	fun broadcast(packet: Packet, toSkip: Player? = null) =
		(players.values - toSkip)
			.forEach { it!!.send(packet) }

	fun addCreature(creature: Creature) {
		creatures[creature.id] = creature
		val creatureUpdate = creature.toCreatureUpdate()
		broadcast(Pvp.makeAttackable(creatureUpdate))
	}

	fun removeCreature(creature: Creature) {
		creatures.remove(creature.id)
		val creatureUpdate = CreatureUpdate(id = creature.id, health = 0f, affiliation = Affiliation.Neutral)
		broadcast(creatureUpdate)
	}

	fun addPlayer(player: Player) {
		creatures.values.toSet().forEach {
			val creatureUpdate = it.copy().toCreatureUpdate()
			player.send(Pvp.makeAttackable(creatureUpdate))
		}
		player.send(
			WorldUpdate(
				chunkLoots = dropLists.map {
					ChunkLoot(
						chunk = it.key,
						drops = it.value
					)
				}
			)
		)
		addCreature(player.character)
		players[player.character.id] = player
	}

	fun removePlayer(player: Player) {
		players.remove(player.character.id)
		removeCreature(player.character)
	}

	fun announce(message: String, skipDiscord: Boolean = false) {
		players.values.toSet().forEach {
			it.notify(message)
		}
		println(Kolor.foreground(message, Color.LIGHT_GRAY))
		if (!skipDiscord) {
			GlobalScope.launch { DiscordBot.post(message) }
		}
	}

	fun addGroundItem(item: Item, position: Vector3<Long>, rotation: Float) {
		val chunk = Vector2(
			(position.x / Utils.SIZE_ZONE).toInt(),
			(position.y / Utils.SIZE_ZONE).toInt()
		)
		val drop = Drop(
			item,
			position,
			rotation,
			scale = 0.1f
		)
		val dropList = dropLists.getOrPut(chunk) { mutableListOf() }
		val dropListCopy = dropList.toList()
		dropList.add(drop)

		broadcast(
			WorldUpdate(
				chunkLoots = listOf(
					ChunkLoot(
						chunk,
						dropListCopy + drop.copy(droptime = 500)
					)
				),
				soundEffects = listOf(
					SoundEffect(
						Utils.creatureToSoundPosition(position),
						SoundEffect.Sound.Drop
					)
				)
			)
		)

		GlobalScope.launch {//todo: layer scope
			delay(500)
			broadcast(
				WorldUpdate(
					soundEffects = listOf(
						SoundEffect(
							Utils.creatureToSoundPosition(position),
							SoundEffect.Sound.DropItem
						)
					)
				)
			)
		}
	}

	fun removeGroundItem(dropList: MutableList<Drop>, drop: Drop) {
		assert(dropList.remove(drop))

		val chunk = dropLists.toList().first { it.second === dropList }.first

		if (dropList.isEmpty()) dropLists.remove(chunk)

		broadcast(
			WorldUpdate(
				chunkLoots = listOf(ChunkLoot(chunk, dropList))
			)
		)
	}

	fun findPlayerByNameOrId(input: String) =
		players.values.find {
			it.character.name.lowercase().contains(input.lowercase())
				|| it.character.id.value == input.toLongOrNull()
		}
}