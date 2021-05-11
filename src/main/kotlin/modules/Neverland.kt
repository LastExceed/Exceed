package exceed.Modules

import Layer
import Player
import me.lastexceed.cubeworldnetworking.packets.*

object Neverland {
	private val ghostLayer = Layer()

	suspend fun onCreatureUpdate(source: Player, creatureUpdate: CreatureUpdate) {
		if (source.layer == ghostLayer ||
			source.character.health > 0f ||
			creatureUpdate.health == null ||
			creatureUpdate.health!! <= 0f //TODO: why isnt this smartcasted ?
		) return

		source.moveTo(ghostLayer)

		val buff = Buff(
			source.character.id,
			source.character.id,
			BuffType.Camouflage,
			0,
			0,
			99999f,
			Int.MAX_VALUE,
			0,
			source.character.id
		)
		val su = ServerUpdate()
		su.buffs.add(buff)
		source.send(su)

		val time = DayTime(0, -10_100_000)
		source.send(time)

		val tombstone = CreatureUpdate(
			id = CreatureIdPool.claim(),
			position = source.character.position,
			health = 1f,
			affiliation = Affiliation.Neutral
		)

		source.send(tombstone)
	}
}