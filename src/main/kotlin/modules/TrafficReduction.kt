package modules

import Creature
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlin.math.*

object TrafficReduction {
	private val sentDatas = mutableMapOf<CreatureId, Creature>()

	fun onCreatureUpdate(packet: CreatureUpdate, source: Player): CreatureUpdate? {
		val previous = source.character
		val current = previous.copy().apply { update(packet) }
		val lastSent = sentDatas[source.character.id] ?: source.character.copy().also { sentDatas[source.character.id] = it }

		val velocityZisRelevant = packet.velocity?.run { //could be a 1-liner, but that would be unreadable
			if (current.flags[CreatureFlag.Climbing]) return@run false

			val isJump = z == 10f //when jumping, the z value is fixed at 10f for a while
			val zIncreasedWhileAirbone = !current.flagsPhysics[PhysicsFlag.OnGround] && z > previous.velocity.z

			return@run isJump || zIncreasedWhileAirbone
		} ?: false
		val isGliderHover = velocityZisRelevant && current.flags[CreatureFlag.Gliding]
		val isMovementChange = packet.acceleration?.run { diff(lastSent.acceleration) > 4f } ?: false
		val isNewAnimation = packet.animationTime?.run { this < previous.animationTime } ?: false

		val filtered = packet.copy(
			position = if (isMovementChange || isGliderHover) current.position else null,
			rotation = null,//doesnt work anyway
			velocity = if (velocityZisRelevant) packet.velocity else null,
			acceleration = if (isMovementChange) current.acceleration else null,
			velocityExtra = packet.velocityExtra?.run {
				if (abs(x) <= abs(lastSent.velocityExtra.x) &&
					abs(y) <= abs(lastSent.velocityExtra.y) &&
					abs(z) <= abs(lastSent.velocityExtra.z)
				) null else this
			},
			climbAnimationState = null,
			flagsPhysics = null,
			//affiliation
			//race
			//animation
			animationTime = if (isNewAnimation) 0 else null,
			//combo
			hitTimeOut = null,
			//appearance
			//flags
			effectTimeDodge = if (isNewAnimation && previous.effectTimeDodge != 0) 0 else packet.effectTimeDodge.nullUnlessBigger(previous.effectTimeDodge),
			effectTimeStun = packet.effectTimeStun.nullUnlessBigger(previous.effectTimeStun),
			effectTimeFear = packet.effectTimeFear.nullUnlessBigger(previous.effectTimeFear),
			effectTimeIce = packet.effectTimeIce.nullUnlessBigger(previous.effectTimeIce),
			effectTimeWind = packet.effectTimeWind.nullUnlessBigger(previous.effectTimeWind),
			//showPatchTime
			//combatClassMajor
			//combatClassMinor
			//manaCharge
			//unknown24
			//unknown25
			aimDisplacement = packet.aimDisplacement?.run {
				val charges = setOf(
					Animation.ShieldM2Charging,
					Animation.CrossbowM2Charging,
					Animation.BowM2Charging,
					Animation.BoomerangM2Charging,
					Animation.GreatweaponM2Charging,
					Animation.UnarmedM2Charging,
					Animation.DualWieldM2Charging
				)
				if ((current.animation in charges || current.animationTime < 1500) &&
					diff(lastSent.aimDisplacement) > 2f
				) this else null
			},
			//health
			mana = null,
			blockingGauge = null,
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

		lastSent.update(filtered)

		return if (filtered.isEmpty()) null else filtered
	}

	private fun CreatureUpdate.isEmpty() =
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
		).all { it == null }

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
		this?.run { if (this > reference) this else null }
}