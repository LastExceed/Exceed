package modules

import Creature
import com.github.lastexceed.cubeworldnetworking.packets.*

object Balancing {
	private const val SHIELD_DEFENSE = 0.25f

	fun adjustDamage(hit: Hit, target: Creature): Hit {
		var gearDefense = 0f

		if (target.equipment.leftWeapon.isShield()) {
			gearDefense += SHIELD_DEFENSE
		}
		if (target.equipment.rightWeapon.isShield()) {
			gearDefense += SHIELD_DEFENSE
		}

		return hit.copy(damage = hit.damage * (1 - gearDefense))
	}

	//add bleeding to make daggers viable
	//add burning to make fire mages viable
	//buff staffs cuz they're hard as fuck to aim

	private fun Item.isShield() =
		typeMajor == Item.Type.Major.Weapon && typeMinor == Item.Type.Minor.Weapon.Shield
}