package Modules

import exceed.Player
import packets.*
import packets.utils.*
import kotlin.math.*

object AntiCheat {
	fun <T> T.expect(expected: T) {
		if (this != expected) {
			//TODO
		}
	}

	fun <T> T.expect(expected: Set<T>) {
		if (!expected.contains(this)) {
			//TODO
		}
	}

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
				if (it.z !in -0.1f..0.1f) {
					return "rotation.pitch"
				}
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
				val limitXY = sqrt(2 * 80f.pow(2)) //113,1370849898476
				val actualXY = sqrt(it.x.pow(2) + it.y.pow(2))

				if (actualXY > limitXY) {
					return "acceleration.XY"
				}
				if (it.z != 0f) {
					return "acceleration.Z"
				}
			}
			velocityExtra?.let {
				var limitXY = 0.1f //game doesnt reset all the way to 0
				var limitZ = 0f
				if (combatClassMajor ?: previous.combatClassMajor == CombatClassMajor.Ranger) {
					limitXY = 33.5f
					limitZ = 15.8f
				}

				val actualXY = sqrt(it.x.pow(2) + it.y.pow(2))
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
				if (it.value !in Race.ElfMale.value..Race.UndeadFemale.value || it == Race.TerrierBull) {
					//terrierBull is inside that range but not valid
					return "race"
				}
			}
			motion?.let {
				val allowedMotions = setOf(
					Motion.Idle,
					Motion.UnarmedM1a,
					Motion.UnarmedM1b,
					Motion.UnarmedM2,
					Motion.UnarmedM2Charging,
					Motion.Drinking,
					Motion.Eating,
					Motion.PetFoodPresent,
					Motion.Sitting,
					Motion.Sleeping,
					Motion.Riding,
					Motion.Boat
				) + when (combatClassMajor ?: previous.combatClassMajor) {
					CombatClassMajor.Warrior -> setOf(
						Motion.DualWieldM1a,
						Motion.DualWieldM1b,
						Motion.DualWieldM2Charging,
						Motion.GreatweaponM1a,
						Motion.GreatweaponM1b,
						Motion.GreatweaponM1c,
						Motion.GreatweaponM2Charging,
						Motion.ShieldM1a,
						Motion.ShieldM1b,
						Motion.ShieldM2,
						Motion.ShieldM2Charging,
						Motion.Smash,
						Motion.Cyclone
					) + when (combatClassMinor ?: previous.combatClassMinor) {
						CombatClassMinor.Warrior.Berserker -> setOf(
							Motion.GreatweaponM2Berserker
						)
						CombatClassMinor.Warrior.Guardian -> setOf(
							Motion.GreatweaponM2Guardian
						)
						else -> error("subclass should be sanity checked already")
					}
					CombatClassMajor.Ranger -> setOf(
						Motion.ShootArrow,
						Motion.BowM2,
						Motion.BowM2Charging,
						Motion.CrossbowM2,
						Motion.CrossbowM2Charging,
						Motion.BoomerangM1,
						Motion.BoomerangM2Charging,
						Motion.Kick
					)
					CombatClassMajor.Mage -> setOf(
						Motion.Teleport
					) + when (combatClassMinor ?: previous.combatClassMinor) {
						CombatClassMinor.Mage.Fire -> setOf(
							Motion.StaffFireM1,
							Motion.StaffFireM2,
							Motion.WandFireM1,
							Motion.WandFireM2,
							Motion.BraceletsFireM1a,
							Motion.BraceletsFireM1b,
							Motion.BraceletFireM2,
							Motion.FireExplosionShort
						)
						CombatClassMinor.Mage.Water -> setOf(
							Motion.StaffWaterM1,
							Motion.StaffWaterM2,
							Motion.WandWaterM1,
							Motion.WandWaterM2,
							Motion.BraceletsWaterM1a,
							Motion.BraceletsWaterM1b,
							Motion.BraceletWaterM2,
							Motion.HealingStream
						)
						else -> error("subclass should be sanity checked already")
					}
					CombatClassMajor.Rogue -> setOf(
						Motion.LongswordM1a,
						Motion.LongswordM1b,
						Motion.LongswordM2,
						Motion.DaggersM1a,
						Motion.DaggersM1b,
						Motion.DaggersM2,
						Motion.FistsM2,
						Motion.Intercept,
						Motion.Stealth
					) + when (combatClassMinor ?: previous.combatClassMinor) {
						CombatClassMinor.Rogue.Assassin -> setOf(

						)
						CombatClassMinor.Rogue.Ninja -> setOf(
							Motion.Shuriken
						)
						else -> error("subclass should be sanity checked already")
					}
					else -> error("class should be sanity checked already")
				}

				if (!allowedMotions.contains(it)) {
				}
			}
			motionTime?.let {
				if (it < 0 || (it > 10_000 && motion ?: previous.motion != Motion.Idle)) {
					return "animation time"
				}
			}
			combo?.let {
				if (it < 0) {
					return "combo (negative)"
				}
			}
			hitTimeOut?.let {
				if (it < 0) {
					return "hitTimeOut"
				}
			}
			appearance?.let {
//				for (value in 0..15) {
//					flags[AppearanceFlag(value)].expect(false)
//				}

				//unknownA
				//unknownB
				//hairColor
				//unknownC
			}
			flags?.let {
				if (it[CreatureFlag.friendlyFire]) {
					return "flags.friendlyFire"
				}

				if (it[CreatureFlag.sniping]) {
					val isRanger = combatClassMajor ?: previous.combatClassMajor == CombatClassMajor.Ranger
					val isSubclass0 = combatClassMinor ?: previous.combatClassMinor == CombatClassMinor.Ranger.Sniper
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
			combatClassMajor?.let {
				if (it.value !in 1..4) {
					return "class"
				}
			}
			combatClassMinor?.let {
				if (it.value !in 0..1) {
					return "subclass"
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
				if (sqrt(it.x.pow(2) + it.y.pow(2) + it.z.pow(2)) > 60f) {
					return "aimDisplacement"
				}
			}
			health?.let {
			}
			mana?.let {
				if (it > 1f) {
					return "mana"
				}
			}
			blockMeter?.let {
				if (it > 1f) {
					return "blockMeter"
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
					if (it !in 1..500) {
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
				fun Item.inspect(
					allowedTypeMajor: ItemTypeMajor,
					maxSpirits: Int,
					allowedMaterials: Set<Material>
				) {
					if (typeMajor != allowedTypeMajor) {
					}
					if (randomSeed < 0) {
					}
					if (recipe != ItemTypeMajor.None) {
					}
					if (rarity > Rarity.Legendary) {
					}
					if (Utils.computePower(this.level.toInt()) > Utils.computePower(source.character.level)) {
					}
					spirits.forEach { spirit ->
						if (spirit.level > level) {
						}
						val allowedMaterials = setOf( //intentional name shadowing
							Material.Fire,
							Material.Unholy,
							Material.IceSpirit,
							Material.Wind,
							material
						)
						if (allowedMaterials.contains(spirit.material)) {
						}
						//padding
						//position
					}
					if (spiritCounter > maxSpirits) {
					}
					//paddingA
					//adapted
					//paddingB
					//paddingC

					//TODO: typeMinor & material
				}

				val allowedMaterialsAccessories = setOf(Material.Gold, Material.Silver)
				val allowedMaterialArmor = when (combatClassMajor) {
					CombatClassMajor.Warrior -> setOf(
						Material.Iron,
						Material.Obsidian,
						Material.Saurian,
						Material.Ice
					)
					CombatClassMajor.Ranger -> setOf(
						Material.Parrot,
						Material.Linen
					)
					CombatClassMajor.Mage -> setOf(
						Material.Licht,
						Material.Silk
					)
					CombatClassMajor.Rogue -> setOf(
						Material.Cotton
					)
					else -> setOf()
				} + setOf( //these can be worn by any class
					Material.Bone,
					Material.Mammoth,
					Material.Gold
				)

				it.neck.inspect(ItemTypeMajor.Amulet, 0, allowedMaterialsAccessories)
				it.leftRing.inspect(ItemTypeMajor.Ring, 0, allowedMaterialsAccessories)
				it.rightRing.inspect(ItemTypeMajor.Ring, 0, allowedMaterialsAccessories)

				it.chest.inspect(ItemTypeMajor.Chest, 32, allowedMaterialArmor)
				it.feet.inspect(ItemTypeMajor.Boots, 32, allowedMaterialArmor)
				it.hands.inspect(ItemTypeMajor.Gloves, 32, allowedMaterialArmor)
				it.shoulder.inspect(ItemTypeMajor.Shoulder, 32, allowedMaterialArmor)

				it.lamp.inspect(ItemTypeMajor.Lamp, 0, setOf(Material.Iron))
				it.special.inspect(ItemTypeMajor.Special, 0, setOf(Material.Wood))
				it.pet.inspect(ItemTypeMajor.Pet, 0, setOf(Material.None))

				//TODO

				//it.unknown.inspect()

				//it.leftWeapon.inspect(ItemMainType.Weapon)
				//it.rightWeapon.inspect(ItemMainType.Weapon)

				//- if righthand is 2hand weapon, lefthand must be empty
				//- lefthand cannot be 2hand weapon
				//- 2hand weapon can hold 32 spirits, 1hand 16
				//- allowed material depends on weapon type

			}
			name?.let {
				if (it.length !in 1..15) {

				}
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