package Modules

import packets.*

object Pvp {
	fun enable(creatureUpdate: CreatureUpdate): CreatureUpdate {
		return creatureUpdate.copy(affiliation = Affiliation.Enemy)
	}
}