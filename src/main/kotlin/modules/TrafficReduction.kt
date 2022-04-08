package modules

import Creature
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlin.math.*

object TrafficReduction {
	//todo: really need to switch to a proper math lib for this
	private fun Vector3<Long>.diff(other: Vector3<Long>): Long {
		val xdiff = (x - other.x).toDouble()
		val ydiff = (y - other.y).toDouble()
		val zdiff = (z - other.z).toDouble()
		return sqrt(xdiff.pow(2) + ydiff.pow(2) + zdiff.pow(2)).toLong()
	}

	private fun Vector3<Float>.diff(other: Vector3<Float>): Float {
		val xdiff = (x - other.x)
		val ydiff = (y - other.y)
		val zdiff = (z - other.z)
		return sqrt(xdiff.pow(2) + ydiff.pow(2) + zdiff.pow(2))
	}

	fun <T : Comparable<T>> T?.nullUnlessBigger(reference: T): T? =
		this?.let { if (it > reference) it else null }
	fun <T : Comparable<T>> T?.nullUnlessSmaller(reference: T): T? =
		this?.let { if (it < reference) it else null }

	fun CreatureUpdate.filter(previousActual: Creature, previousSent: Creature) =
		copy(
			position = position?.let { if (it.diff(previousSent.position) > Utils.SIZE_BLOCK * 2) it else null },
			rotation = null,//velocity?.let { if (it.diff(previous.rotation) < 0.1f) null else it },
			velocity = velocity?.let { if (abs(it.z - previousSent.velocity.z) > 2f) it else null },
			acceleration = acceleration?.let { if (it.diff(previousSent.acceleration) < 10f) null else it },
			velocityExtra = velocityExtra?.let { //not sure
				if (abs(it.x) <= abs(previousSent.velocityExtra.x) &&
					abs(it.y) <= abs(previousSent.velocityExtra.y) &&
					abs(it.z) <= abs(previousSent.velocityExtra.z)
				) null else it
			},
			climbAnimationState = null,
			flagsPhysics = null,
			//affiliation
			//race
			//animation
			animationTime = animationTime.nullUnlessSmaller(previousActual.animationTime),
			//combo
			hitTimeOut = null,
			//appearance
			//flags
			effectTimeDodge = effectTimeDodge.nullUnlessBigger(previousActual.effectTimeDodge),
			effectTimeStun = effectTimeStun.nullUnlessBigger(previousActual.effectTimeStun),
			effectTimeFear = effectTimeFear.nullUnlessBigger(previousActual.effectTimeFear),
			effectTimeIce = effectTimeIce.nullUnlessBigger(previousActual.effectTimeIce),
			effectTimeWind = effectTimeWind.nullUnlessBigger(previousActual.effectTimeWind),
			//showPatchTime
			//combatClassMajor
			//combatClassMinor
			//manaCharge
			//unknown24
			//unknown25
			aimDisplacement = aimDisplacement?.let {
				if (previousActual.animation in setOf(Animation.StaffFireM1, Animation.StaffFireM2, Animation.StaffWaterM1, Animation.StaffWaterM2) &&
					(animationTime ?: previousActual.animationTime) < 1500 &&
					it.diff(previousSent.aimDisplacement) > 2f
				) it else null
			},
			//health
			mana = null,
			//blockingGauge = null,//todo
			//multipliers
			//unknown31
			//unknown32
			//level
			experience = null,
			//master
			//unknown36
			//powerBase
			//unknown38
			homeChunk = null,
			home = null,
			chunkToReveal = null,
			//unknown42
			//consumable
			//equipment
			//name
			skillPointDistribution = null,
			manaCubes = null
		)

	fun CreatureUpdate.isEmpty() =
		listOf(
			position,
			rotation,
			velocity,
			acceleration,
			velocityExtra,
			climbAnimationState,
			flagsPhysics,
			affiliation,
			race,
			animation,
			animationTime,
			combo,
			hitTimeOut,
			appearance,
			flags,
			effectTimeDodge,
			effectTimeStun,
			effectTimeFear,
			effectTimeIce,
			effectTimeWind,
			showPatchTime,
			combatClassMajor,
			combatClassMinor,
			manaCharge,
			unknown24,
			unknown25,
			aimDisplacement,
			health,
			mana,
			blockingGauge,
			multipliers,
			unknown31,
			unknown32,
			level,
			experience,
			master,
			unknown36,
			powerBase,
			unknown38,
			homeChunk,
			home,
			chunkToReveal,
			unknown42,
			consumable,
			equipment,
			name,
			skillPointDistribution,
			manaCubes
		).let {
			val r = it.all { it == null }
			if (!r) {
				it.also { println(it.map { if (it == null) '0' else '1' }.joinToString("")) }
			}
			r
		}
}