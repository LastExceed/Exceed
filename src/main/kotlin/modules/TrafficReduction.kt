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

		val isDrift = packet.position?.let { it.diff(lastSent.position) > Utils.SIZE_BLOCK * 2 } ?: false
		val isJump = packet.velocity?.let { it.z > 2f } ?: false
		val isMovementChange = packet.acceleration?.let { it.diff(lastSent.acceleration) > 4f } ?: false
		val isNewAnimation = packet.animationTime?.let { it < previous.animationTime } ?: false

		//pos - turn/drift/stop-moving
		//rota - accTurn/bigRotaTurn/attack
		//accel - drift

		val filtered = packet.copy(
			position = if (isMovementChange || isDrift) current.position else null,
			rotation = null,//doesnt work anyway
			velocity = if (isJump) current.velocity else null,
			acceleration = if (isMovementChange) current.acceleration else null,
			velocityExtra = packet.velocityExtra?.let { //not sure
				if (abs(it.x) <= abs(lastSent.velocityExtra.x) &&
					abs(it.y) <= abs(lastSent.velocityExtra.y) &&
					abs(it.z) <= abs(lastSent.velocityExtra.z)
				) null else it
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
			effectTimeDodge = packet.effectTimeDodge.nullUnlessBigger(previous.effectTimeDodge),
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
			aimDisplacement = packet.aimDisplacement?.let {
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
					it.diff(lastSent.aimDisplacement) > 2f
				) it else null
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
		this?.let { if (it > reference) it else null }
}