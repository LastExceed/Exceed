package exceed

import Modules.Pvp
import packets.*

data class Layer(
	val creatures: MutableMap<Long, Creature> = mutableMapOf(),
	val players: MutableMap<Long, Player> = mutableMapOf()
) {
	suspend fun broadcast(packet: Packet, toSkip: Player? = null) {
		for (player in players.values) {
			if (player == toSkip) continue
			player.send(packet)
		}
	}

	suspend fun addCreature(creature: Creature) {
		creatures[creature.id] = creature
		val creatureUpdate = creature.toCreatureUpdate()
		Pvp.enable(creatureUpdate)
		broadcast(creatureUpdate)
	}

	suspend fun removeCreature(creature: Creature) {
		creatures.remove(creature.id)
		val creatureUpdate = CreatureUpdate(id = creature.id, health = 0f, affiliation = Affiliation.Neutral)
		broadcast(creatureUpdate)
	}

	suspend fun addPlayer(player: Player) {
		creatures.values.forEach {
			val creatureUpdate = it.copy().toCreatureUpdate()
			Pvp.enable(creatureUpdate)
			player.send(creatureUpdate)
		}
		addCreature(player.character)
		players[player.character.id] = player
	}

	suspend fun removePlayer(player: Player) {
		players.remove(player.character.id)
		removeCreature(player.character)
	}
}