package modules

import Creature
import com.github.lastexceed.cubeworldnetworking.packets.*

object Balancing {
	private const val SHIELD_DEFENSE = 0.25f

	fun adjustDamage(hit: Hit, target: Creature): Hit {
		val gearDefense = listOf(
			target.equipment.leftWeapon,
			target.equipment.rightWeapon
		).fold(0f) { accumulator, item ->
			accumulator + if (item.isShield()) SHIELD_DEFENSE else 0f
		}

		val effectiveDamage = listOf(
			gearDefense
		).fold(hit.damage) { accumulator, multiplicativeDefenseModifier ->
			accumulator * (1f - multiplicativeDefenseModifier)
		}

		return hit.copy(damage = effectiveDamage)
	}

	//add bleeding to make daggers viable
	//add burning to make fire mages viable
	//buff staffs cuz they're hard as fuck to aim

	private fun Item.isShield() =
		typeMajor == Item.Type.Major.Weapon && typeMinor == Item.Type.Minor.Weapon.Shield
}