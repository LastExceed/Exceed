package packets.utils

import packets.*
import utils.*
import kotlin.math.*

object Utils {
	fun creatureToSoundPosition(creaturePosition: Vector3<Long>): Vector3<Float> {
		return Vector3(
			(creaturePosition.x / 0x10000).toFloat(),
			(creaturePosition.y / 0x10000).toFloat(),
			(creaturePosition.z / 0x10000).toFloat()
		)
	}

	//not sure what this is exactly, but its used for lots of things
	fun levelScalingFactor(level: Float) = 0.05f * (level - 1f) + 1f
	fun levelScalingFactor(level: Int) = levelScalingFactor(level.toFloat())

	fun computePower(level: Int) = (101f - 100f / levelScalingFactor(level)).toInt()

	fun computeMaxExperience(level: Int) = (1050f - 1000f / levelScalingFactor(level)).toInt()

//	fun computeBaseHP(creature: Creature): Float {
//		val level_health = 2f.pow((1f - (1f / Utils.levelScalingFactor(lvl))) * 3f)
//
//		var health = level_health * hpMult * if(affiliation == Affiliation.Player) 2f else 2f.pow(power_base * 0.25f)
//
//		when(combatClass) {
//			CombatClassMajor.Warrior -> {
//				health *= 1.3f
//				if (combatClassMinor == CombatClassMinor.Warrior.Guardian) health *= 1.25f
//			}
//			CombatClassMajor.Ranger -> health *= 1.1f
//			CombatClassMajor.Rogue -> health *= 1.2f
//		}
//
//		return health
//	}

	const val SIZE_BLOCK = 0x10000L
	const val SIZE_CHUNK = SIZE_BLOCK * 0x100L
	const val SIZE_BIOME = SIZE_CHUNK * 64L
	const val SIZE_WORLD = SIZE_BIOME * 1024L
}