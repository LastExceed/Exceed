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
		if (newData.extraVel != null) {
			this.extraVel = newData.extraVel!!
		}
		if (newData.viewportPitch != null) {
			this.viewportPitch = newData.viewportPitch!!
		}
		if (newData.physicsFlags != null) {
			this.physicsFlags = newData.physicsFlags!!
		}
		if (newData.hostility != null) {
			this.hostility = newData.hostility!!
		}
		if (newData.creatureType != null) {
			this.creatureType = newData.creatureType!!
		}
		if (newData.mode != null) {
			this.mode = newData.mode!!
		}
		if (newData.modeTimer != null) {
			this.modeTimer = newData.modeTimer!!
		}
		if (newData.combo != null) {
			this.combo = newData.combo!!
		}
		if (newData.lastHitTime != null) {
			this.lastHitTime = newData.lastHitTime!!
		}
		if (newData.appearance != null) {
			this.appearance = newData.appearance!!
		}
		if (newData.creatureFlags != null) {
			this.creatureFlags = newData.creatureFlags!!
		}
		if (newData.roll != null) {
			this.roll = newData.roll!!
		}
		if (newData.stun != null) {
			this.stun = newData.stun!!
		}
		if (newData.slow != null) {
			this.slow = newData.slow!!
		}
		if (newData.ice != null) {
			this.ice = newData.ice!!
		}
		if (newData.wind != null) {
			this.wind = newData.wind!!
		}
		if (newData.showPatchTime != null) {
			this.showPatchTime = newData.showPatchTime!!
		}
		if (newData.entityClass != null) {
			this.entityClass = newData.entityClass!!
		}
		if (newData.specialization != null) {
			this.specialization = newData.specialization!!
		}
		if (newData.charge != null) {
			this.charge = newData.charge!!
		}
		if (newData.unused24 != null) {
			this.unused24 = newData.unused24!!
		}
		if (newData.unused25 != null) {
			this.unused25 = newData.unused25!!
		}
		if (newData.rayHit != null) {
			this.rayHit = newData.rayHit!!
		}
		if (newData.HP != null) {
			this.HP = newData.HP!!
		}
		if (newData.MP != null) {
			this.MP = newData.MP!!
		}
		if (newData.block != null) {
			this.block = newData.block!!
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
		if (newData.XP != null) {
			this.XP = newData.XP!!
		}
		if (newData.parentOwner != null) {
			this.parentOwner = newData.parentOwner!!
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
		if (newData.spawnPos != null) {
			this.spawnPos = newData.spawnPos!!
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
		if (newData.skillDistribution != null) {
			this.skillDistribution = newData.skillDistribution!!
		}
		if (newData.manaCubes != null) {
			this.manaCubes = newData.manaCubes!!
		}
	}
}