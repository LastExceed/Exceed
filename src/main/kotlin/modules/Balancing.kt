package modules

import Creature
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*

object Balancing {
	private const val GLOBAL_STUN_MODIFIER = 0.75
	const val GLOBAL_DAMAGE_REDUCTION_PERCENT = 0.5f
	private const val SHIELD_DEFENSE_PERCENT = 0.25f

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

	fun adjustDamage(hit: Hit, source: Creature, target: Creature): Hit {
		if (hit.damage < 0f) { //heal
			val healMultiplier =
				if (hit.target == source.id) {
					0.5f - 1f//self-heals are applied client side as well (bug), so we need to subtract the vanilla amount
				} else
					0.3f
			return hit.copy(damage = hit.damage * healMultiplier)
		}


		val weapon = source.equipment.rightWeapon
		val (weaponOffenseAdjustPercent, weaponStunBonus) = when (weapon.typeMajor) {
			Item.Type.Major.Weapon -> {
				when (weapon.typeMinor) {
					Item.Type.Minor.Weapon.Wand -> 0.0f to 0.25
					Item.Type.Minor.Weapon.Staff -> 0.1f to 0.0
					Item.Type.Minor.Weapon.Fist -> 0.2f to 0.25
					else -> 0.0f to 0.0
				}
			}
			else -> 0.0f to 0.0
		}

		val (classOffenseAdjustPercent, classStunBonus) = when (source.combatClassMajor) {
			CombatClassMajor.Warrior -> {
				when (source.combatClassMinor) {
					CombatClassMinor.Warrior.Berserker -> 0.2f to 0.5
					else -> 0.0f to 0.0
				}
			}
			CombatClassMajor.Ranger -> {
				0.1f to when (source.combatClassMinor) {
					CombatClassMinor.Ranger.Sniper -> 0.25
					else -> 0.0
				}
			}
			CombatClassMajor.Rogue -> {
				when (source.combatClassMinor) {
					CombatClassMinor.Rogue.Assassin -> 0.5f to 0.0
					else -> -0.1f to 0.0
				}
			}
			CombatClassMajor.Mage -> {
				when (source.combatClassMinor) {
					CombatClassMinor.Mage.Fire -> -0.2f to 0.0
					else -> 0.0f to 0.0
				}
			}
			else -> error("unreachable")
		}

		val gearDefensePercent = listOf(
			target.equipment.leftWeapon,
			target.equipment.rightWeapon
		).fold(0f) { accumulator, item ->
			accumulator + if (item.isShield()) SHIELD_DEFENSE_PERCENT else 0f
		}

		val totalDefensiveMultiplier = listOf(
			GLOBAL_DAMAGE_REDUCTION_PERCENT,
			gearDefensePercent
		).fold(1f) { accumulator, multiplicativeDefenseModifier ->
			accumulator * (1f - multiplicativeDefenseModifier)
		}

		val totalOffensiveMultiplier = listOf(
			weaponOffenseAdjustPercent,
			classOffenseAdjustPercent,
		).fold(1f) { accumulator, damageAdjustPercent ->
			accumulator * (1f + damageAdjustPercent)
		}

		val effectiveDamage = listOf(
			totalDefensiveMultiplier,
			totalOffensiveMultiplier
		).fold(hit.damage) { accumulator, damageMultiplier ->
			accumulator * damageMultiplier
		}

		val effectiveStunDuration = listOf(
			GLOBAL_STUN_MODIFIER,
			1.0 + classStunBonus,
			1.0 + weaponStunBonus
		).fold(hit.stuntime) { accumulator, modifier ->
			(accumulator * modifier).toInt()
		}

		return hit.copy(
			damage = effectiveDamage,
			stuntime = effectiveStunDuration
		)
	}

	val lastGroundTime = mutableMapOf<CreatureId, Pair<Long, Boolean>>()

	fun onCreatureUpdate(packet: CreatureUpdate, source: Player) {
		if (source.character.combatClassMajor != CombatClassMajor.Rogue) return

		if (source.character.flagsPhysics[PhysicsFlag.OnGround] ||
			source.character.flagsPhysics[PhysicsFlag.Swimming] ||
			source.character.flags[CreatureFlag.Gliding] ||
			source.character.flags[CreatureFlag.Climbing]
		) {
			lastGroundTime[source.character.id] = System.currentTimeMillis() to false
			return
		}

		val (lastGroundTime, warned) = lastGroundTime[source.character.id] ?: return
		val currentFlightDuration = System.currentTimeMillis() - lastGroundTime

		if (currentFlightDuration > 3000 && !warned) {
			source.send(
				WorldUpdate(
					statusEffects = listOf(
						StatusEffect(
							source = CreatureId(0),
							target = source.character.id,
							type = StatusEffect.Type.Unknown8,
							modifier = 1f,
							duration = 2000,
							creatureId3 = source.character.id
						)
					),
					soundEffects = listOf(
						SoundEffect(
							position = Utils.creatureToSoundPosition(source.character.position),
							SoundEffect.Sound.Magic01
						)
					)
				)
			)
			this.lastGroundTime[source.character.id] = lastGroundTime to true
		}

		if (currentFlightDuration > 5000) {
			source.send(
				WorldUpdate(
					hits = listOf(
						Hit(
							attacker = CreatureId(0),
							target = source.character.id,
							damage = 0f,
							critical = false,
							stuntime = 3000,
							position = source.character.position,
							direction = Vector3(0f, 0f, 0f),
							isYellow = false,
							type = Hit.Type.Default,
							flash = true
						)
					),
					soundEffects = listOf(
						SoundEffect(
							position = Utils.creatureToSoundPosition(source.character.position),
							SoundEffect.Sound.SpikeTrap
						)
					)
				)
			)
			source.notify("you have been punished for glitching in the air too long")
			this.lastGroundTime[source.character.id] = System.currentTimeMillis() to false
		}
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