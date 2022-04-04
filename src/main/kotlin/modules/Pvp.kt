package modules

import com.github.lastexceed.cubeworldnetworking.packets.*

object Pvp {
	fun makeAttackable(creatureUpdate: CreatureUpdate) =
		creatureUpdate.copy(
			flags = creatureUpdate.flags?.apply {//todo: copy
				set(CreatureFlag.FriendlyFire, true)
			},
			//multipliers = Multipliers(100f, 1f, 1f, 1f, 1f),
			//affiliation = Affiliation.Enemy
		)
}