package exceed.Modules

import exceed.*
import packets.*

object Neverland {
	private val ghostLayer = Layer()

	fun onCreatureUpdate(source: Player, creatureUpdate: CreatureUpdate) {
		if (source.layer == ghostLayer ||
			source.character.HP > 0f ||
			creatureUpdate.HP == null ||
			creatureUpdate.HP!! > 0f
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
	}
}