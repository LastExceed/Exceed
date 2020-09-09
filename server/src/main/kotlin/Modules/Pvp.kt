package Modules

import packets.*

object Pvp {
	fun makeAttackable(creatureUpdate: CreatureUpdate): CreatureUpdate {
		val editedFlags = creatureUpdate.flags //todo: copy
		if (editedFlags != null) {
			editedFlags[CreatureFlag.friendlyFire] = true
		}
		return creatureUpdate.copy(flags = editedFlags, multipliers = Multipliers(1f, 1f, 1f, 10f, 10f))
		//return creatureUpdate.copy(affiliation = Affiliation.Enemy)
	}
}