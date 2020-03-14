package exceed.packetHandlers

import Modules.Pvp
import exceed.Creature
import exceed.Modules.Neverland
import packets.*
import exceed.Player

object CreatureUpdateHandler : PacketHandler<CreatureUpdate> {
	override suspend fun handlePacket(packet: CreatureUpdate, source: Player) {
		Neverland.onCreatureUpdate(source, packet)
		source.character.update(packet)

		Pvp.enable(packet)

		source.layer.broadcast(packet, source)
	}

	private fun Creature.update(newData: CreatureUpdate) {
		if (newData.position != null) {
			this.position = newData.position!!
		}
		if (newData.rotation != null) {
			this.rotation = newData.rotation!!
		}
		if (newData.velocity != null) {
			this.velocity = newData.velocity!!
		}
		if (newData.acceleration != null) {
			this.acceleration = newData.acceleration!!
		}
		if (newData.velocityExtra != null) {
			this.velocityExtra = newData.velocityExtra!!
		}
		if (newData.climbAnimationState != null) {
			this.climbAnimationState = newData.climbAnimationState!!
		}
		if (newData.flagsPhysics != null) {
			this.flagsPhysics = newData.flagsPhysics!!
		}
		if (newData.affiliation != null) {
			this.affiliation = newData.affiliation!!
		}
		if (newData.race != null) {
			this.race = newData.race!!
		}
		if (newData.motion != null) {
			this.motion = newData.motion!!
		}
		if (newData.motionTime != null) {
			this.motionTime = newData.motionTime!!
		}
		if (newData.combo != null) {
			this.combo = newData.combo!!
		}
		if (newData.hitTimeOut != null) {
			this.hitTimeOut = newData.hitTimeOut!!
		}
		if (newData.appearance != null) {
			this.appearance = newData.appearance!!
		}
		if (newData.flags != null) {
			this.flags = newData.flags!!
		}
		if (newData.effectTimeDodge != null) {
			this.effectTimeDodge = newData.effectTimeDodge!!
		}
		if (newData.effectTimeStun != null) {
			this.effectTimeStun = newData.effectTimeStun!!
		}
		if (newData.effectTimeFear != null) {
			this.effectTimeFear = newData.effectTimeFear!!
		}
		if (newData.effectTimeIce != null) {
			this.effectTimeIce = newData.effectTimeIce!!
		}
		if (newData.effectTimeWind != null) {
			this.effectTimeWind = newData.effectTimeWind!!
		}
		if (newData.showPatchTime != null) {
			this.showPatchTime = newData.showPatchTime!!
		}
		if (newData.combatClassMajor != null) {
			this.combatClassMajor = newData.combatClassMajor!!
		}
		if (newData.combatClassMinor != null) {
			this.combatClassMinor = newData.combatClassMinor!!
		}
		if (newData.manaCharge != null) {
			this.manaCharge = newData.manaCharge!!
		}
		if (newData.unused24 != null) {
			this.unused24 = newData.unused24!!
		}
		if (newData.unused25 != null) {
			this.unused25 = newData.unused25!!
		}
		if (newData.aimDisplacement != null) {
			this.aimDisplacement = newData.aimDisplacement!!
		}
		if (newData.health != null) {
			this.health = newData.health!!
		}
		if (newData.mana != null) {
			this.mana = newData.mana!!
		}
		if (newData.blockMeter != null) {
			this.blockMeter = newData.blockMeter!!
		}
		if (newData.multipliers != null) {
			this.multipliers = newData.multipliers!!
		}
		if (newData.unused31 != null) {
			this.unused31 = newData.unused31!!
		}
		if (newData.unused32 != null) {
			this.unused32 = newData.unused32!!
		}
		if (newData.level != null) {
			this.level = newData.level!!
		}
		if (newData.experience != null) {
			this.experience = newData.experience!!
		}
		if (newData.master != null) {
			this.master = newData.master!!
		}
		if (newData.unused36 != null) {
			this.unused36 = newData.unused36!!
		}
		if (newData.powerBase != null) {
			this.powerBase = newData.powerBase!!
		}
		if (newData.unused38 != null) {
			this.unused38 = newData.unused38!!
		}
		if (newData.unused39 != null) {
			this.unused39 = newData.unused39!!
		}
		if (newData.home != null) {
			this.home = newData.home!!
		}
		if (newData.unused41 != null) {
			this.unused41 = newData.unused41!!
		}
		if (newData.unused42 != null) {
			this.unused42 = newData.unused42!!
		}
		if (newData.consumable != null) {
			this.consumable = newData.consumable!!
		}
		if (newData.equipment != null) {
			this.equipment = newData.equipment!!
		}
		if (newData.name != null) {
			this.name = newData.name!!
		}
		if (newData.skillPointDistribution != null) {
			this.skillPointDistribution = newData.skillPointDistribution!!
		}
		if (newData.manaCubes != null) {
			this.manaCubes = newData.manaCubes!!
		}
	}
}