package modules

import com.github.lastexceed.cubeworldnetworking.packets.CreatureUpdate

object TrafficReduction {
	fun shouldIgnore(packet: CreatureUpdate) =
		packet.run {
			setOf(
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
		}
}