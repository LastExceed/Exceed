package exceed

import Modules.Pvp
import kotlinx.coroutines.runBlocking
import packets.*

data class Layer(
	val creatures: MutableMap<Long, Creature> = mutableMapOf(),
	val players: MutableMap<Long, Player> = mutableMapOf()
) {
	fun broadcast(packet: Packet, toSkip: Player? = null) {
		runBlocking {
			for (player in players.values) {
				if (player == toSkip) continue
				player.send(packet)
			}
		}
	}

	fun addCreature(creature: Creature) {
		creatures[creature.id] = creature
		val creatureUpdate = creature.toCreatureUpdate()
		Pvp.enable(creatureUpdate)
		broadcast(creatureUpdate)
	}

	fun removeCreature(creature: Creature) {
		creatures.remove(creature.id)
		val creatureUpdate = CreatureUpdate(id = creature.id, HP = 0f, hostility = Hostility.Neutral)
		broadcast(creatureUpdate)
	}

	fun addPlayer(player: Player) {
		creatures.values.forEach {
			val creatureUpdate = it.copy().toCreatureUpdate()
			Pvp.enable(creatureUpdate)
			player.send(creatureUpdate)
		}
		addCreature(player.character)
		players[player.character.id] = player
	}

	fun removePlayer(player: Player) {
		players.remove(player.character.id)
		removeCreature(player.character)
	}
}