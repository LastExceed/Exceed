package Modules

import exceed.GetRidOfThis.computePower
import exceed.Player
import packets.*
import kotlin.math.sqrt

object AntiCheat {
	private const val LEVEL_CAP = 500
	fun inspect(creatureUpdate: CreatureUpdate, source: Player): String? {
		val previous = source.character

		with(creatureUpdate) {
			if (id != source.character.id) {
				return "id"
			}

			position?.let {
				//return "position"
			}
			rotation?.let {
				if (it.y !in -90f..90f) {
					return "rotation.roll"
				}
				if (it.z !in -180f..180f) {
					return "rotation.yaw"
				}
			}
			velocity?.let {
				//can change with abilites
			}
			acceleration?.let {
				//max 80 when normal
				//max 113,1370849898476 when diagonal
			}
			velocityExtra?.let {
				var limitXY = 0.1f //game doesnt reset all the way to 0
				var limitZ = 0f
				if (combatClass ?: previous.combatClass == CombatClass.Ranger) {
					limitXY = 33.5f
					limitZ = 15.8f
				}

				val actualXY = sqrt(it.x * it.x + it.y * it.y)
				if (actualXY > limitXY) {
					return "velocityExtra.XY"
				}
				if (it.z > limitZ) {
					return "velocityExtra.Z"
				}
			}
			climbAnimationState?.let {
				if (it !in 0f..45f) {
					return "climbAnimationState"
				}
			}
			flagsPhysics?.let {

			}
			affiliation?.let {
				if (it != Affiliation.Player) {
					return "affiliation"
				}
			}
			race?.let {
				if (it !in Race.ElfMale.value..Race.UndeadFemale.value || it == Race.TerrierBull) {
					return "race"
				}
			}
			motion?.let {
				//TODO:motion AC
			}
			motionTime?.let {

			}
			combo?.let {
				if (it < 0) {
					return "combo (negative)"
				}
			}
			hitTimeOut?.let {

			}
			appearance?.let {
				//TODO: this will be alot of work
			}
			flags?.let {
				if (it[CreatureFlag.friendlyFire]) {
					return "flags.friendlyFire"
				}

				if (it[CreatureFlag.sniping]) {
					val isRanger = combatClass ?: previous.combatClass == CombatClass.Ranger
					val isSubclass0 = combatSubclass ?: previous.combatSubclass == 0.toByte()
					if (!isRanger || !isSubclass0) {
						return "flags.sniping"
					}
				}
				if (it[CreatureFlag.lamp]) {
					//check if lamp is equipped
				}
			}
			effectTimeDodge?.let {
				if (it !in 0..600) {
					return "effectTimeDodge"
				}
			}
			effectTimeStun?.let {

			}
			effectTimeFear?.let {
				if (it != 0) {
					return "effectTimeFear"
				}
			}
			effectTimeIce?.let {
				if (it < 0) {
					return "effectTimeIce (negative)"
				}
			}
			effectTimeWind?.let {
				if (it < 0) {
					return "effectTimeIce (negative)"
				}
			}
			showPatchTime?.let {

			}
			combatClass?.let {
				if (it.value !in 1..4) {
					//invalid class
				}
			}
			combatSubclass?.let {
				if (it !in 0..1) {
					//invalid subclass
				}
			}
			manaCharge?.let {
				if (it !in 0f..(mana ?: previous.mana)) {
					return "manaCharge"
				}
			}
			unused24?.let {
			}
			unused25?.let {
			}
			aimDisplacement?.let {
			}
			health?.let {
			}
			mana?.let {
				if (it > 1f) {
					//too much mana
				}
			}
			blockMeter?.let {
				if (it > 1f) {
					//impossible block meter
				}
			}
			multipliers?.let {
				if (it.health != 100f) {
					return "multipliers.health"
				}
				if (it.attackSpeed != 1f) {
					return "multipliers.attackSpeed"
				}
				if (it.damage != 1f) {
					return "multipliers.damage"
				}
				if (it.armor != 1f) {
					return "multipliers.armor"
				}
				if (it.resi != 1f) {
					return "multipliers.resi"
				}
			}
			unused31?.let {

			}
			unused32?.let {

			}
			level?.let {
				level?.let {
					if (it !in 1..LEVEL_CAP) {
						return "level"
					}
				}
			}
			experience?.let {

			}
			master?.let {
				if (it != CreatureID(0)) {

				}
			}
			unused36?.let {

			}
			powerBase?.let {

			}
			unused38?.let {

			}
			unused39?.let {

			}
			home?.let {

			}
			unused41?.let {

			}
			unused42?.let {

			}
			consumable?.let {

			}
			equipment?.let {
				fun Item.inspect(): Boolean {
					if (this.rarity.value > Rarity.Legendary.value) {
						//mythic
					}
					if (computePower(this.level.toInt()) > computePower(source.character.level)) {
						//not enough power to wear this item
					}
					return true
				}
				//maintype/subtype: based on slot
				//material: based on slot and class
			}
			name?.let {
				
			}
			skillPointDistribution?.let {

			}
			manaCubes?.let {
				if (it < 0) {
					return "manaCubes"
				}
			}
		}
		return null
	}
}