package exceed

import packets.*
import utils.Vector3

data class Creature(
	val id: Long,
	var position: Vector3<Long>,
	var rotation: Vector3<Float>,
	var velocity: Vector3<Float>,
	var acceleration: Vector3<Float>,
	var extraVel: Vector3<Float>,
	var viewportPitch: Float,
	var physicsFlags: BooleanArray,
	var hostility: Hostility,
	var creatureType: CreatureType,
	var mode: Mode,
	var modeTimer: Int,
	var combo: Int,
	var lastHitTime: Int,
	var appearance: Appearance,
	var creatureFlags: BooleanArray,
	var roll: Int,
	var stun: Int,
	var slow: Int,
	var ice: Int,
	var wind: Int,
	var showPatchTime: Int,
	var entityClass: EntityClass,
	var specialization: Byte,
	var charge: Float,
	var unused24: Vector3<Float>,
	var unused25: Vector3<Float>,
	var rayHit: Vector3<Float>,
	var HP: Float,
	var MP: Float,
	var block: Float,
	var multipliers: Multipliers,
	var unused31: Byte,
	var unused32: Byte,
	var level: Int,
	var XP: Int,
	var parentOwner: Long,
	var unused36: Long,
	var powerBase: Byte,
	var unused38: Int,
	var unused39: Vector3<Int>,
	var spawnPos: Vector3<Long>,
	var unused41: Vector3<Int>,
	var unused42: Byte,
	var consumable: Item,
	var equipment: Equipment,
	var name: String,
	var skillDistribution: SkillDistribution,
	var manaCubes: Int
) {
	constructor(creatureUpdate: CreatureUpdate) : this(
		id = creatureUpdate.id,
		position = creatureUpdate.position!!,
		rotation = creatureUpdate.rotation!!,
		velocity = creatureUpdate.velocity!!,
		acceleration = creatureUpdate.acceleration!!,
		extraVel = creatureUpdate.extraVel!!,
		viewportPitch = creatureUpdate.viewportPitch!!,
		physicsFlags = creatureUpdate.physicsFlags!!,
		hostility = creatureUpdate.hostility!!,
		creatureType = creatureUpdate.creatureType!!,
		mode = creatureUpdate.mode!!,
		modeTimer = creatureUpdate.modeTimer!!,
		combo = creatureUpdate.combo!!,
		lastHitTime = creatureUpdate.lastHitTime!!,
		appearance = creatureUpdate.appearance!!,
		creatureFlags = creatureUpdate.creatureFlags!!,
		roll = creatureUpdate.roll!!,
		stun = creatureUpdate.stun!!,
		slow = creatureUpdate.slow!!,
		ice = creatureUpdate.ice!!,
		wind = creatureUpdate.wind!!,
		showPatchTime = creatureUpdate.showPatchTime!!,
		entityClass = creatureUpdate.entityClass!!,
		specialization = creatureUpdate.specialization!!,
		charge = creatureUpdate.charge!!,
		unused24 = creatureUpdate.unused24!!,
		unused25 = creatureUpdate.unused25!!,
		rayHit = creatureUpdate.rayHit!!,
		HP = creatureUpdate.HP!!,
		MP = creatureUpdate.MP!!,
		block = creatureUpdate.block!!,
		multipliers = creatureUpdate.multipliers!!,
		unused31 = creatureUpdate.unused31!!,
		unused32 = creatureUpdate.unused32!!,
		level = creatureUpdate.level!!,
		XP = creatureUpdate.XP!!,
		parentOwner = creatureUpdate.parentOwner!!,
		unused36 = creatureUpdate.unused36!!,
		powerBase = creatureUpdate.powerBase!!,
		unused38 = creatureUpdate.unused38!!,
		unused39 = creatureUpdate.unused39!!,
		spawnPos = creatureUpdate.spawnPos!!,
		unused41 = creatureUpdate.unused41!!,
		unused42 = creatureUpdate.unused42!!,
		consumable = creatureUpdate.consumable!!,
		equipment = creatureUpdate.equipment!!,
		name = creatureUpdate.name!!,
		skillDistribution = creatureUpdate.skillDistribution!!,
		manaCubes = creatureUpdate.manaCubes!!
	)

	fun toCreatureUpdate(): CreatureUpdate {
		return CreatureUpdate(
			id,
			position,
			rotation,
			velocity,
			acceleration,
			extraVel,
			viewportPitch,
			physicsFlags,
			hostility,
			creatureType,
			mode,
			modeTimer,
			combo,
			lastHitTime,
			appearance,
			creatureFlags,
			roll,
			stun,
			slow,
			ice,
			wind,
			showPatchTime,
			entityClass,
			specialization,
			charge,
			unused24,
			unused25,
			rayHit,
			HP,
			MP,
			block,
			multipliers,
			unused31,
			unused32,
			level,
			XP,
			parentOwner,
			unused36,
			powerBase,
			unused38,
			unused39,
			spawnPos,
			unused41,
			unused42,
			consumable,
			equipment,
			name,
			skillDistribution,
			manaCubes
		)
	}
}