package exceed

import packets.*
import utils.Vector3

data class Creature(
	val id: Long,
	var position: Vector3<Long>,
	var rotation: Vector3<Float>,
	var velocity: Vector3<Float>,
	var acceleration: Vector3<Float>,
	var velocityExtra: Vector3<Float>,
	var climbAnimation: Float,
	var flagsPhysics: BooleanArray,
	var affiliation: Hostility,
	var race: CreatureType,
	var activity: Mode,
	var activityTimer: Int,
	var combo: Int,
	var hitTimeOut: Int,
	var appearance: Appearance,
	var flags: BooleanArray,
	var effectTimeDodge: Int,
	var effectTimeStun: Int,
	var effectTimeFear: Int,
	var effectTimeIce: Int,
	var effectTimeWind: Int,
	var showPatchTime: Int,
	var combatClass: EntityClass,
	var combatSpecialization: Byte,
	var manaCharge: Float,
	var unused24: Vector3<Float>,
	var unused25: Vector3<Float>,
	var aimDisplacement: Vector3<Float>,
	var health: Float,
	var mana: Float,
	var blockMeter: Float,
	var multipliers: Multipliers,
	var unused31: Byte,
	var unused32: Byte,
	var level: Int,
	var experience: Int,
	var master: Long,
	var unused36: Long,
	var powerBase: Byte,
	var unused38: Int,
	var unused39: Vector3<Int>,
	var home: Vector3<Long>,
	var unused41: Vector3<Int>,
	var unused42: Byte,
	var consumable: Item,
	var equipment: Equipment,
	var name: String,
	var skillPointDistribution: SkillDistribution,
	var manaCubes: Int
) {
	constructor(creatureUpdate: CreatureUpdate) : this(
		id = creatureUpdate.id,
		position = creatureUpdate.position!!,
		rotation = creatureUpdate.rotation!!,
		velocity = creatureUpdate.velocity!!,
		acceleration = creatureUpdate.acceleration!!,
		velocityExtra = creatureUpdate.velocityExtra!!,
		climbAnimation = creatureUpdate.climbAnimation!!,
		flagsPhysics = creatureUpdate.flagsPhysics!!,
		affiliation = creatureUpdate.affiliation!!,
		race = creatureUpdate.race!!,
		activity = creatureUpdate.activity!!,
		activityTimer = creatureUpdate.modeTimer!!,
		combo = creatureUpdate.combo!!,
		hitTimeOut = creatureUpdate.hitTimeOut!!,
		appearance = creatureUpdate.appearance!!,
		flags = creatureUpdate.flags!!,
		effectTimeDodge = creatureUpdate.effectTimeDodge!!,
		effectTimeStun = creatureUpdate.effectTimeStun!!,
		effectTimeFear = creatureUpdate.effectTimeFear!!,
		effectTimeIce = creatureUpdate.effectTimeIce!!,
		effectTimeWind = creatureUpdate.effectTimeWind!!,
		showPatchTime = creatureUpdate.showPatchTime!!,
		combatClass = creatureUpdate.entityClass!!,
		combatSpecialization = creatureUpdate.specialization!!,
		manaCharge = creatureUpdate.charge!!,
		unused24 = creatureUpdate.unused24!!,
		unused25 = creatureUpdate.unused25!!,
		aimDisplacement = creatureUpdate.aimDisplacement!!,
		health = creatureUpdate.health!!,
		mana = creatureUpdate.mana!!,
		blockMeter = creatureUpdate.blockMeter!!,
		multipliers = creatureUpdate.multipliers!!,
		unused31 = creatureUpdate.unused31!!,
		unused32 = creatureUpdate.unused32!!,
		level = creatureUpdate.level!!,
		experience = creatureUpdate.experience!!,
		master = creatureUpdate.master!!,
		unused36 = creatureUpdate.unused36!!,
		powerBase = creatureUpdate.powerBase!!,
		unused38 = creatureUpdate.unused38!!,
		unused39 = creatureUpdate.unused39!!,
		home = creatureUpdate.home!!,
		unused41 = creatureUpdate.unused41!!,
		unused42 = creatureUpdate.unused42!!,
		consumable = creatureUpdate.consumable!!,
		equipment = creatureUpdate.equipment!!,
		name = creatureUpdate.name!!,
		skillPointDistribution = creatureUpdate.skillPointDistribution!!,
		manaCubes = creatureUpdate.manaCubes!!
	)

	fun toCreatureUpdate(): CreatureUpdate {
		return CreatureUpdate(
			id,
			position,
			rotation,
			velocity,
			acceleration,
			velocityExtra,
			climbAnimation,
			flagsPhysics,
			affiliation,
			race,
			activity,
			activityTimer,
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
			combatClass,
			combatSpecialization,
			manaCharge,
			unused24,
			unused25,
			aimDisplacement,
			health,
			mana,
			blockMeter,
			multipliers,
			unused31,
			unused32,
			level,
			experience,
			master,
			unused36,
			powerBase,
			unused38,
			unused39,
			home,
			unused41,
			unused42,
			consumable,
			equipment,
			name,
			skillPointDistribution,
			manaCubes
		)
	}
}