package modules

import Creature
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

object Balancing {
	private const val GLOBAL_DAMAGE_REDUCTION = 0.5f
	private const val SHIELD_DEFENSE = 0.25f

	fun warFrenzyBuff(packet: StatusEffect, source: Player) {
		source.layer.broadcast(
			WorldUpdate(
				statusEffects = listOf(
					//packet.copy(type = StatusEffect.Type.Camouflage),
					packet.copy(type = StatusEffect.Type.Swiftness)
				)
			)
		)
	}

	fun adjustDamage(hit: Hit, target: Creature): Hit {
		val gearDefense = listOf(
			target.equipment.leftWeapon,
			target.equipment.rightWeapon
		).fold(0f) { accumulator, item ->
			accumulator + if (item.isShield()) SHIELD_DEFENSE else 0f
		}

		val effectiveDamage = listOf(
			GLOBAL_DAMAGE_REDUCTION,
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

	private const val weaponLevelCap = 40
	suspend fun ensureLowLevelWeapon(creatureUpdate: CreatureUpdate, source: Player): Boolean {
		val equipment = creatureUpdate.equipment ?: return true

		setOf(
			equipment.leftWeapon,
			equipment.rightWeapon
		).filter { it.typeMajor != Item.Type.Major.None && it.level > weaponLevelCap }
			.let {
				if (it.isNotEmpty()) {
					source.send(
						WorldUpdate(
							pickups = it.map {
								Pickup(
									source.character.id,
									it.copy(level = weaponLevelCap.toShort())
								)
							}
						)
					)
					source.kick("weapons are limited to lvl$weaponLevelCap (for balance). you've been given downleveled copies of your weapons, please equip them and then re-join")
					return false
				}
			}
		return true
	}
}