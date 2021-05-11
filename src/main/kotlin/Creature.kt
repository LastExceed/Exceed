import me.lastexceed.cubeworldnetworking.packets.*
import me.lastexceed.cubeworldnetworking.utils.*
import kotlin.math.*

data class Creature(
	val id: CreatureID,
	var position: Vector3<Long>,
	var rotation: Vector3<Float>,
	var velocity: Vector3<Float>,
	var acceleration: Vector3<Float>,
	var velocityExtra: Vector3<Float>,
	var climbAnimationState: Float,
	var flagsPhysics: FlagSet<PhysicsFlag>,
	var affiliation: Affiliation,
	var race: Race,
	var animation: Animation,
	var animationTime: Int,
	var combo: Int,
	var hitTimeOut: Int,
	var appearance: Appearance,
	var flags: FlagSet<CreatureFlag>,
	var effectTimeDodge: Int,
	var effectTimeStun: Int,
	var effectTimeFear: Int,
	var effectTimeIce: Int,
	var effectTimeWind: Int,
	var showPatchTime: Int,
	var combatClassMajor: CombatClassMajor,
	var combatClassMinor: CombatClassMinor,
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
	var master: CreatureID,
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
		climbAnimationState = creatureUpdate.climbAnimationState!!,
		flagsPhysics = creatureUpdate.flagsPhysics!!,
		affiliation = creatureUpdate.affiliation!!,
		race = creatureUpdate.race!!,
		animation = creatureUpdate.animation!!,
		animationTime = creatureUpdate.animationTime!!,
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
		combatClassMajor = creatureUpdate.combatClassMajor!!,
		combatClassMinor = creatureUpdate.combatClassMinor!!,
		manaCharge = creatureUpdate.manaCharge!!,
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

	fun update(newData: CreatureUpdate) {
		newData.position?.let {
			position = it
		}
		newData.rotation?.let {
			rotation = it
		}
		newData.velocity?.let {
			velocity = it
		}
		newData.acceleration?.let {
			acceleration = it
		}
		newData.velocityExtra?.let {
			velocityExtra = it
		}
		newData.climbAnimationState?.let {
			climbAnimationState = it
		}
		newData.flagsPhysics?.let {
			flagsPhysics = it
		}
		newData.affiliation?.let {
			affiliation = it
		}
		newData.race?.let {
			race = it
		}
		newData.animation?.let {
			animation = it
		}
		newData.animationTime?.let {
			animationTime = it
		}
		newData.combo?.let {
			combo = it
		}
		newData.hitTimeOut?.let {
			hitTimeOut = it
		}
		newData.appearance?.let {
			appearance = it
		}
		newData.flags?.let {
			flags = it
		}
		newData.effectTimeDodge?.let {
			effectTimeDodge = it
		}
		newData.effectTimeStun?.let {
			effectTimeStun = it
		}
		newData.effectTimeFear?.let {
			effectTimeFear = it
		}
		newData.effectTimeIce?.let {
			effectTimeIce = it
		}
		newData.effectTimeWind?.let {
			effectTimeWind = it
		}
		newData.showPatchTime?.let {
			showPatchTime = it
		}
		newData.combatClassMajor?.let {
			combatClassMajor = it
		}
		newData.combatClassMinor?.let {
			combatClassMinor = it
		}
		newData.manaCharge?.let {
			manaCharge = it
		}
		newData.unused24?.let {
			unused24 = it
		}
		newData.unused25?.let {
			unused25 = it
		}
		newData.aimDisplacement?.let {
			aimDisplacement = it
		}
		newData.health?.let {
			health = it
		}
		newData.mana?.let {
			mana = it
		}
		newData.blockMeter?.let {
			blockMeter = it
		}
		newData.multipliers?.let {
			multipliers = it
		}
		newData.unused31?.let {
			unused31 = it
		}
		newData.unused32?.let {
			unused32 = it
		}
		newData.level?.let {
			level = it
		}
		newData.experience?.let {
			experience = it
		}
		newData.master?.let {
			master = it
		}
		newData.unused36?.let {
			unused36 = it
		}
		newData.powerBase?.let {
			powerBase = it
		}
		newData.unused38?.let {
			unused38 = it
		}
		newData.unused39?.let {
			unused39 = it
		}
		newData.home?.let {
			home = it
		}
		newData.unused41?.let {
			unused41 = it
		}
		newData.unused42?.let {
			unused42 = it
		}
		newData.consumable?.let {
			consumable = it
		}
		newData.equipment?.let {
			equipment = it
		}
		newData.name?.let {
			name = it
		}
		newData.skillPointDistribution?.let {
			skillPointDistribution = it
		}
		newData.manaCubes?.let {
			manaCubes = it
		}
	}

	val chunk: Vector2<Int>
		get() = Vector2(
			(position.x / Utils.SIZE_CHUNK).toInt(),
			(position.y / Utils.SIZE_CHUNK).toInt()
		)

	val healthMaximum: Float
		get() {
			val multiplierLevel = 2f.pow((1f - Utils.levelScalingFactor(level)) * 3f)

			val multiplierAffiliation =
				if (affiliation == Affiliation.Player) 2f
				else 2f.pow(powerBase * 0.25f)

			val multiplierCombatClass = when (combatClassMajor) {
				CombatClassMajor.Warrior -> 1.3f * if (combatClassMinor == CombatClassMinor.Warrior.Guardian) 1.25f else 1f
				CombatClassMajor.Ranger -> 1.1f
				CombatClassMajor.Mage -> 1f
				CombatClassMajor.Rogue -> 1.2f
				else -> error("this should be sanitized")
			}

			val baseMaximum = listOf(
				multiplierLevel,
				multipliers.health,
				multiplierAffiliation,
				multiplierCombatClass
			).fold(1f) { accumulator, multiplier ->
				accumulator * multiplier
			}

			return baseMaximum + listOf(
				equipment.chest.hp,
				equipment.feet.hp,
				equipment.hands.hp,
				equipment.shoulder.hp,
				equipment.leftWeapon.hp,
				equipment.rightWeapon.hp,

				equipment.unknown.hp,
				equipment.neck.hp,
				equipment.leftRing.hp,
				equipment.rightRing.hp,
				equipment.lamp.hp,
				equipment.special.hp,
				equipment.pet.hp
			).sum()
		}
}