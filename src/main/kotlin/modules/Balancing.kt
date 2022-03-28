package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

object Balancing {
	private const val SHIELD_DEFENSE = 0.25f

	fun adjustDamage(hit: Hit, source: Player): Hit {
		var gearDefense = 0f

		val targetEquipment = source.layer.creatures[hit.target]!!.equipment

		if (targetEquipment.leftWeapon.isShield()) {
			gearDefense += SHIELD_DEFENSE
		}
		if (targetEquipment.rightWeapon.isShield()) {
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