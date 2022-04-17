import modules.Pvp
import com.github.lastexceed.cubeworldnetworking.packets.*

data class Layer(
	val creatures: MutableMap<CreatureId, Creature> = mutableMapOf(),
	val players: MutableMap<CreatureId, Player> = mutableMapOf()
) {
	fun broadcast(packet: Packet, toSkip: Player? = null) {
		for (player in players.values.toSet()) {
			if (player == toSkip) continue
			player.send(packet)
		}
	}

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
		addCreature(player.character)
		players[player.character.id] = player
	}

	fun removePlayer(player: Player) {
		players.remove(player.character.id)
		removeCreature(player.character)
	}

	fun announce(message: String) {
		players.values.toSet().forEach {
			it.notify(message)
		}
		println(message)
	}
}