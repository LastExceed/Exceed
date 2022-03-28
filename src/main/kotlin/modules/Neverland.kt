package exceed.Modules

import Layer
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

object Neverland {
	private val ghostLayer = Layer()

	suspend fun onCreatureUpdate(source: Player, creatureUpdate: CreatureUpdate) {
		if (source.layer == ghostLayer ||
			source.character.health > 0f ||
			creatureUpdate.health == null ||
			creatureUpdate.health!! <= 0f //TODO: why isnt this smartcasted ?
		) return

		source.moveTo(ghostLayer)

		val statusEffect = StatusEffect(
			source.character.id,
			source.character.id,
			StatusEffect.Type.Camouflage,
			0,
			0,
			99999f,
			Int.MAX_VALUE,
			0,
			source.character.id
		)
		source.send(ServerUpdate(statusEffects = listOf(statusEffect)))
		source.send(DayTime(0, -10_100_000))

		val tombstone = CreatureUpdate(
			id = CreatureIdPool.claim(),
			position = source.character.position,
			health = 1f,
			affiliation = Affiliation.Neutral
		)

		source.send(tombstone)
	}
}