import modules.Pvp
import com.github.lastexceed.cubeworldnetworking.packets.*

data class Layer(
	val creatures: MutableMap<CreatureId, Creature> = mutableMapOf(),
	val players: MutableMap<CreatureId, Player> = mutableMapOf()
) {
	suspend fun broadcast(packet: Packet, toSkip: Player? = null) {
		for (player in players.values.toList()) { //TODO: concurrent modification apparently
			if (player == toSkip) continue
			player.send(packet)
		}
	}

	suspend fun addCreature(creature: Creature) {
		creatures[creature.id] = creature
		val creatureUpdate = creature.toCreatureUpdate()
		broadcast(Pvp.makeAttackable(creatureUpdate))
	}

	suspend fun removeCreature(creature: Creature) {
		creatures.remove(creature.id)
		val creatureUpdate = CreatureUpdate(id = creature.id, health = 0f, affiliation = Affiliation.Neutral)
		broadcast(creatureUpdate)
	}

	suspend fun addPlayer(player: Player) {
		creatures.values.forEach {
			val creatureUpdate = it.copy().toCreatureUpdate()
			player.send(Pvp.makeAttackable(creatureUpdate))
		}
		addCreature(player.character)
		players[player.character.id] = player
	}

	suspend fun removePlayer(player: Player) {
		players.remove(player.character.id)
		removeCreature(player.character)
	}

	suspend fun announce(message: String) {
		players.values.forEach {
			it.notify(message)
		}
		println(message)
	}
}