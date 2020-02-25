package Modules

import packets.*

object Pvp {
	fun enable(creatureUpdate: CreatureUpdate): CreatureUpdate {
		return creatureUpdate.copy(hostility = Hostility.Enemy)
	}
}