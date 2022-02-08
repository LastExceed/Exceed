package modules

import Creature
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlin.math.*

object AntiCheat {
	class CheaterException(message: String) : Exception(message)

	private val accelLimitXY = sqrt(2 * 80f.pow(2)) + 0.00001f //113,1370849898476
	private val allowedRaces = setOf(
		Race.ElfMale,
		Race.ElfFemale,
		Race.HumanMale,
		Race.HumanFemale,
		Race.GoblinMale,
		Race.GoblinFemale,
		Race.LizardmanMale,
		Race.LizardmanFemale,
		Race.DwarfMale,
		Race.DwarfFemale,
		Race.OrcMale,
		Race.OrcFemale,
		Race.FrogmanMale,
		Race.FrogmanFemale,
		Race.UndeadMale,
		Race.UndeadFemale
	)
	private val timelessAnimations = setOf(
		Animation.Idle,
		Animation.Stealth,
		Animation.Boat,
		Animation.Sitting,
		Animation.PetFoodPresent,
		Animation.Sleeping
	)
	private val allowedMaterialsAccessories = setOf(Material.Gold, Material.Silver)
	private val allowedRarities = setOf(
		Rarity.Normal,
		Rarity.Uncommon,
		Rarity.Rare,
		Rarity.Epic,
		Rarity.Legendary
	)
	private val subTypesSpecial = setOf(
		ItemTypeMinor.Special.Boat,
		ItemTypeMinor.Special.HangGlider
	)
	private val allowedWeaponMaterials = mapOf(
		ItemTypeMinor.Weapon.Sword to setOf(Material.Iron, Material.Obsidian, Material.Bone),
		ItemTypeMinor.Weapon.Axe to setOf(Material.Iron),
		ItemTypeMinor.Weapon.Mace to setOf(Material.Iron),
		ItemTypeMinor.Weapon.Dagger to setOf(Material.Iron),
		ItemTypeMinor.Weapon.Fist to setOf(Material.Iron),
		ItemTypeMinor.Weapon.Longsword to setOf(Material.Iron),
		ItemTypeMinor.Weapon.Bow to setOf(Material.Wood),
		ItemTypeMinor.Weapon.Crossbow to setOf(Material.Wood),
		ItemTypeMinor.Weapon.Boomerang to setOf(Material.Wood),
		ItemTypeMinor.Weapon.Staff to setOf(Material.Wood),
		ItemTypeMinor.Weapon.Wand to setOf(Material.Wood),
		ItemTypeMinor.Weapon.Bracelet to setOf(Material.Gold, Material.Silver),
		ItemTypeMinor.Weapon.Shield to setOf(Material.Iron, Material.Wood),
		ItemTypeMinor.Weapon.Greatsword to setOf(Material.Iron),
		ItemTypeMinor.Weapon.Greataxe to setOf(Material.Iron, Material.Saurian),
		ItemTypeMinor.Weapon.Greatmace to setOf(Material.Iron, Material.Wood),

		ItemTypeMinor.Weapon.Arrow to setOf(Material.None),
		ItemTypeMinor.Weapon.Quiver to setOf(Material.None),
		ItemTypeMinor.Weapon.Pitchfork to setOf(Material.None),
		ItemTypeMinor.Weapon.Pickaxe to setOf(Material.None),
		ItemTypeMinor.Weapon.Torch to setOf(Material.None),
	)
	private val allowedAppearance = mapOf(
		Race.ElfMale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			1236..1239,
			1280..1289,
			430..430,
			432,
			1,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.ElfFemale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			1240..1245,
			1290..1299,
			430..430,
			432,
			0,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.HumanMale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			1246..1251,
			1252..1266,
			430..431,
			432,
			1,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.HumanFemale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			1267..1272,
			1273..1279,
			430..431,
			432,
			1,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.GoblinMale to AppearanceReference(
			Vector3(0.80f, 0.80f, 1.80f),
			75..79,
			80..85,
			97..97,
			432,
			0,
			1.01f,
			1.00f,
			1.00f,
			1.20f
		),
		Race.GoblinFemale to AppearanceReference(
			Vector3(0.80f, 0.80f, 1.80f),
			86..90,
			91..96,
			97..97,
			432,
			0,
			1.01f,
			1.00f,
			1.00f,
			1.20f
		),
		Race.LizardmanMale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			98..99,
			100..105,
			111..111,
			113,
			112,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.LizardmanFemale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			106..111,
			100..105,
			111..111,
			113,
			112,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.DwarfMale to AppearanceReference(
			Vector3(0.80f, 0.80f, 1.80f),
			282..286,
			287..289,
			430..431,
			432,
			300,
			0.90f,
			1.00f,
			1.00f,
			1.20f
		),
		Race.DwarfFemale to AppearanceReference(
			Vector3(0.80f, 0.80f, 1.80f),
			290..294,
			295..299,
			430..431,
			432,
			301,
			0.90f,
			1.00f,
			1.00f,
			1.20f
		),
		Race.OrcMale to AppearanceReference(
			Vector3(1.04f, 1.04f, 2.34f),
			1300..1304,
			1310..1319,
			302..302,
			432,
			0,
			0.90f,
			1.00f,
			1.20f,
			0.95f
		),
		Race.OrcFemale to AppearanceReference(
			Vector3(1.04f, 1.04f, 2.34f),
			1305..1309,
			1320..1323,
			302..302,
			432,
			0,
			0.80f,
			0.95f,
			1.10f,
			0.95f
		),
		Race.FrogmanMale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			1324..1328,
			1329..1333,
			1342..1342,
			432,
			1,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.FrogmanFemale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			1334..1337,
			1338..1341,
			1342..1342,
			432,
			1,
			1.01f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.UndeadMale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			303..308,
			309..314,
			327..327,
			432,
			0,
			0.90f,
			1.00f,
			1.00f,
			0.95f
		),
		Race.UndeadFemale to AppearanceReference(
			Vector3(0.96000004f, 0.96000004f, 2.16f),
			315..320,
			321..326,
			327..327,
			432,
			0,
			0.90f,
			1.00f,
			1.00f,
			0.95f
		)
	)

	data class AppearanceReference(
		val creatureSize: Vector3<Float>,
		val headModel: IntRange,
		val hairModel: IntRange,
		val handModel: IntRange,
		val footModel: Short,
		val bodyModel: Short,
		val headSize: Float,
		val bodySize: Float,
		val shoulder1Size: Float,
		val weaponSize: Float
	)

	private fun getAllowedAnimations(classMajor: CombatClassMajor, classMinor: CombatClassMinor) = setOf(
		Animation.Idle,
		Animation.UnarmedM1a,
		Animation.UnarmedM1b,
		Animation.UnarmedM2,
		Animation.UnarmedM2Charging,
		Animation.Drinking,
		Animation.Eating,
		Animation.PetFoodPresent,
		Animation.Sitting,
		Animation.Sleeping,
		Animation.Riding,
		Animation.Boat
	) + when (classMajor) {
		CombatClassMajor.Warrior -> setOf(
			Animation.DualWieldM1a,
			Animation.DualWieldM1b,
			Animation.DualWieldM2Charging,
			Animation.GreatweaponM1a,
			Animation.GreatweaponM1b,
			Animation.GreatweaponM1c,
			Animation.GreatweaponM2Charging,
			Animation.ShieldM1a,
			Animation.ShieldM1b,
			Animation.ShieldM2,
			Animation.ShieldM2Charging,
			Animation.Smash,
			Animation.Cyclone
		) + when (classMinor) {
			CombatClassMinor.Warrior.Berserker -> setOf(
				Animation.GreatweaponM2Berserker
			)
			CombatClassMinor.Warrior.Guardian -> setOf(
				Animation.GreatweaponM2Guardian
			)
			else -> error("subclass should be sanity checked already")
		}
		CombatClassMajor.Ranger -> setOf(
			Animation.ShootArrow,
			Animation.BowM2,
			Animation.BowM2Charging,
			Animation.CrossbowM2,
			Animation.CrossbowM2Charging,
			Animation.BoomerangM1,
			Animation.BoomerangM2Charging,
			Animation.Kick
		)
		CombatClassMajor.Mage -> setOf(
			Animation.Teleport
		) + when (classMinor) {
			CombatClassMinor.Mage.Fire -> setOf(
				Animation.StaffFireM1,
				Animation.StaffFireM2,
				Animation.WandFireM1,
				Animation.WandFireM2,
				Animation.BraceletsFireM1a,
				Animation.BraceletsFireM1b,
				Animation.BraceletFireM2,
				Animation.FireExplosionShort
			)
			CombatClassMinor.Mage.Water -> setOf(
				Animation.StaffWaterM1,
				Animation.StaffWaterM2,
				Animation.WandWaterM1,
				Animation.WandWaterM2,
				Animation.BraceletsWaterM1a,
				Animation.BraceletsWaterM1b,
				Animation.BraceletWaterM2,
				Animation.HealingStream
			)
			else -> error("subclass should be sanity checked already")
		}
		CombatClassMajor.Rogue -> setOf(
			Animation.LongswordM1a,
			Animation.LongswordM1b,
			Animation.LongswordM2,
			Animation.DaggersM1a,
			Animation.DaggersM1b,
			Animation.DaggersM2,
			Animation.FistsM2,
			Animation.Intercept,
			Animation.Stealth
		) + when (classMinor) {
			CombatClassMinor.Rogue.Assassin -> setOf(

			)
			CombatClassMinor.Rogue.Ninja -> setOf(
				Animation.Shuriken
			)
			else -> error("subclass should be sanity checked already")
		}
		else -> error("class should be sanity checked already")
	}

	private fun getAllowedMaterialsArmor(combatClassMajor: CombatClassMajor) = when (combatClassMajor) {
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

	private fun getAllowedWeaponTypes(combatClassMajor: CombatClassMajor) = when (combatClassMajor) {
		CombatClassMajor.Warrior -> setOf(
			ItemTypeMinor.Weapon.Sword,
			ItemTypeMinor.Weapon.Axe,
			ItemTypeMinor.Weapon.Mace,
			ItemTypeMinor.Weapon.Shield,
			ItemTypeMinor.Weapon.Greatsword,
			ItemTypeMinor.Weapon.Greataxe,
			ItemTypeMinor.Weapon.Greatmace
		)
		CombatClassMajor.Ranger -> setOf(
			ItemTypeMinor.Weapon.Bow,
			ItemTypeMinor.Weapon.Crossbow,
			ItemTypeMinor.Weapon.Boomerang,
			ItemTypeMinor.Weapon.Quiver
		)
		CombatClassMajor.Mage -> setOf(
			ItemTypeMinor.Weapon.Staff,
			ItemTypeMinor.Weapon.Wand,
			ItemTypeMinor.Weapon.Bracelet,
		)
		CombatClassMajor.Rogue -> setOf(
			ItemTypeMinor.Weapon.Dagger,
			ItemTypeMinor.Weapon.Fist,
			ItemTypeMinor.Weapon.Longsword,
		)
		else -> error("class should be sanitized")
	} + setOf(//class agnostic
		ItemTypeMinor.Weapon.Pitchfork,
		ItemTypeMinor.Weapon.Pickaxe,
		ItemTypeMinor.Weapon.Torch,
		ItemTypeMinor.Weapon.Arrow
	)

	private fun getWeaponHandCount(weaponType: ItemTypeMinor) = when (weaponType) {
		ItemTypeMinor.Weapon.Longsword,
		ItemTypeMinor.Weapon.Bow,
		ItemTypeMinor.Weapon.Crossbow,
		ItemTypeMinor.Weapon.Boomerang,
		ItemTypeMinor.Weapon.Staff,
		ItemTypeMinor.Weapon.Wand,
		ItemTypeMinor.Weapon.Greatsword,
		ItemTypeMinor.Weapon.Greataxe,
		ItemTypeMinor.Weapon.Greatmace,
		ItemTypeMinor.Weapon.Pitchfork -> 2

//		ItemTypeMinor.Weapon.Bracelet,
//		ItemTypeMinor.Weapon.Shield,
//		ItemTypeMinor.Weapon.Quiver,
//		ItemTypeMinor.Weapon.Arrow,
//		ItemTypeMinor.Weapon.Sword,
//		ItemTypeMinor.Weapon.Axe,
//		ItemTypeMinor.Weapon.Mace,
//		ItemTypeMinor.Weapon.Dagger,
//		ItemTypeMinor.Weapon.Fist,
//		ItemTypeMinor.Weapon.Pickaxe,
//		ItemTypeMinor.Weapon.Torch -> 1

		else -> 1
	}

	private val lastFireSpam = mutableMapOf<CreatureID, Long>()
	private val lastAnimation = mutableMapOf<CreatureID, Long>(

	)

	fun inspect(creatureUpdate: CreatureUpdate, previous: Creature): String? {
		val current = previous.copy().apply { update(creatureUpdate) } //TODO: optimize
		try {
			with(creatureUpdate) {
				id.expect(previous.id, "id")

				position?.let {}
				rotation?.let {
					//TODO: swimming
					//it.x.expectIn(-0.1f..0.1f, "rotation.pitch")
					it.y.expectIn(-90f..90f, "rotation.roll")
					//rotation.z.expectIn(-180f..180f, "rotation.yaw")
					//TODO: overflows while attacking
				}
				velocity?.let {}//can change with abilites
				acceleration?.let {
					val actualXY = sqrt(it.x.pow(2) + it.y.pow(2))
					//TODO: glider fucks this up
					//actualXY.expectIn(0f..accelLimitXY, "acceleration.XY")
					//TODO: swimming up
					//it.z.expect(0f, "acceleration.Z")

				}
				velocityExtra?.let {
					var limitXY = 0.1f //game doesnt reset all the way to 0
					var limitZ = 0f
					if (current.combatClassMajor == CombatClassMajor.Ranger) {
						limitXY = 33.500004f
						limitZ = 15.8f
					}

					val actualXY = sqrt(it.x.pow(2) + it.y.pow(2))
					//TODO: numbers stucks when dead
					actualXY.expectIn(0f..limitXY, "velocityExtra.XY")
					it.z.expectIn(0f..limitZ, "velocityExtra.Z")
				}
				climbAnimationState?.expectMaximum(45f, "climbAnimationState")
				flagsPhysics?.let {}
				affiliation?.expect(Affiliation.Player, "affiliation")
				race?.expectIn(allowedRaces, "race")
				animation?.expectIn(
					getAllowedAnimations(
						current.combatClassMajor,
						current.combatClassMinor
					),
					"animation"
				)
				animationTime?.let {
					it.expectMinimum(0, "animationTime")
					val animationCurrent = current.animation

					if (animationCurrent !in timelessAnimations) {
						//TODO: incomplete
						//it.expectMaximum(10_000, "animationTime")
					}

					if (it < previous.animationTime) {
						if (animationCurrent == Animation.FireExplosionShort) {
							val currentTime = System.currentTimeMillis()
							val lastTime = lastFireSpam[id]
							if (lastTime != null) {
								(currentTime - lastTime).expectMinimum(5000, "firespamming")
							}
							lastFireSpam[id] = currentTime
						}
					}
				}
				combo?.expectMinimum(0, "combo")
				hitTimeOut?.expectMinimum(0, "hitTimeOut")
				appearance?.let {
					//unknownA
					//unknownB
					//hairColor
					//unknownC
					it.flags.expect(FlagSet(0.toShort().toBooleanArray()), "appearance.flags")

					it.tailModel.expect(-1, "appearance.tailModel")
					it.shoulder2Model.expect(-1, "appearance.shoulder2Model")
					it.wingModel.expect(-1, "appearance.wingModel")

					it.handSize.expect(1f, "appearance.handSize")
					it.footSize.expect(0.98f, "appearance.footSize")
					it.tailSize.expect(0.8f, "appearance.tailSize")
					it.shoulder2Size.expect(1f, "appearance.shoulder1Size")
					it.wingSize.expect(1f, "appearance.wingSize")

					it.bodyOffset.expect(Vector3(0f, 0f, -5f), "appearance.bodyOffset")
					it.headOffset.expect(
						if (current.race == Race.OrcFemale) Vector3(0f, 1.5f, 4f) else Vector3(0f, 0.5f, 5f),
						"appearance.headOffset"
					)
					it.handOffset.expect(Vector3(6f, 0f, 0f), "appearance.handOffset")
					it.footOffset.expect(Vector3(3f, 1f, -10.5f), "appearance.footOffset")
					it.tailOffset.expect(Vector3(0f, -8f, 2f), "appearance.tailOffset")
					it.wingOffset.expect(Vector3(0f, 0f, 0f), "appearance.wingOffset")

					it.bodyRotation.expect(0f, "appearance.bodyRotation")
					it.handRotation.expect(Vector3(0f, 0f, 0f), "appearance.handRotation")
					it.feetRotation.expect(0f, "appearance.feetRotation")
					it.wingRotation.expect(0f, "appearance.wingRotation")
					it.tail_rotation.expect(0f, "appearance.tail_rotation")

					val reference = allowedAppearance[current.race]!!
					it.creatureSize.expect(reference.creatureSize, "appearance.creature.Size")
					it.headModel.toInt().expectIn(reference.headModel, "appearance.headModel")
					it.hairModel.toInt().expectIn(reference.hairModel, "appearance.hairModel")
					it.handModel.toInt().expectIn(reference.handModel, "appearance.handModel")
					it.footModel.expect(reference.footModel, "appearance.footModel")
					it.bodyModel.expect(reference.bodyModel, "appearance.bodyModel")
					it.headSize.expect(reference.headSize, "appearance.headSize")
					it.bodySize.expect(reference.bodySize, "appearance.bodySize")
					it.shoulder1Size.expect(reference.shoulder1Size, "appearance.shoulder2Size")
					it.weaponSize.expect(reference.weaponSize, "appearance.weaponSize")
				}
				flags?.let {
					it[CreatureFlag.friendlyFire].expect(false, "flags.friendlyFire")
					val isRanger = current.combatClassMajor == CombatClassMajor.Ranger
					val isSniper = current.combatClassMinor == CombatClassMinor.Ranger.Sniper
					if (!isRanger || !isSniper) {
						it[CreatureFlag.sniping].expect(false, "flags.sniping")
					}
					if (current.equipment.lamp.typeMajor == ItemTypeMajor.None) {
						it[CreatureFlag.lamp].expect(false, "flags.lamp")
					}
				}
				effectTimeDodge?.expectIn(0..600, "effectTimeDodge")
				effectTimeStun?.let {}
				effectTimeFear?.expectMinimum(0, "effectTimeFear")
				effectTimeIce?.expectMinimum(0, "effectTimeIce")
				effectTimeWind?.expectMaximum(5000, "effectTimeWind")
				showPatchTime?.let {}
				combatClassMajor?.value?.toInt()?.expectIn(1..4, "combatClassMajor")
				combatClassMinor?.value?.toInt()?.expectIn(0..1, "combatClassMinor")
				manaCharge?.expectMaximum(current.mana, "manaCharge") //might go negative with blocking bug
				unused24?.let {}
				unused25?.let {}
				aimDisplacement?.let {
					val distance = sqrt(it.x.pow(2) + it.y.pow(2) + it.z.pow(2))
					//TODO: exceeds when moving
					//distance.expectIn(0f..62f, "aimDisplacement")
				}
				health?.expectMaximum(current.healthMaximum * 1.01f, "health")//TODO: fix rounding errors
				mana?.let {
					it.expectMaximum(1f, "mana")//m1, ninja dodge, block
					if (current.combatClassMajor != CombatClassMajor.Mage
						&& current.animationTime > 1000
						&& current.animation !in setOf(Animation.Stealth, Animation.ShieldM2Charging)
						&& !current.flags[CreatureFlag.sniping]) {//TODO: assassin camo
						//TODO: leaving stealth still generates mp for some time
						//it.expectMaximum(previous.mana, "mana")
					}
				}
				blockMeter?.let {
					it.expectMaximum(1f, "blockMeter")
					if (current.animation in setOf(Animation.ShieldM2Charging, Animation.DualWieldM2Charging, Animation.GreatweaponM2Charging, Animation.UnarmedM2Charging)) {
						//TODO: quick release and recharge breaks this
						//it.expectMaximum(previous.blockMeter, "blockMeter")
					}
				}
				multipliers?.let {
					it.health.expect(100f, "multipliers.health")
					it.attackSpeed.expect(1f, "multipliers.attackSpeed")
					it.damage.expect(1f, "multipliers.damage")
					it.resi.expect(1f, "multipliers.resi")
					it.armor.expect(1f, "multipliers.armor")
				}
				unused31?.let {}
				unused32?.let {}
				level?.expectIn(1..500, "level")
				experience?.expectIn(0..Utils.computeMaxExperience(current.level), "experience")
				master?.expect(CreatureID(0), "master")
				unused36?.let {}
				powerBase?.let {}
				unused38?.let {}
				unused39?.let {}
				home?.let {}
				unused41?.let {}
				unused42?.let {}
				consumable?.let {
					if (it.typeMajor == ItemTypeMajor.None) return@let

					it.typeMajor.expect(ItemTypeMajor.Food, "consumable.typeMajor")
					it.rarity.expect(Rarity.Normal, "consumable.rarity")

					val powerAllowed = Utils.computePower(current.level)
					val powerActual = Utils.computePower(it.level.toInt())

					powerActual.expectIn(1..powerAllowed, "consumable.level")
					it.spiritCounter.expect(0, "consumable.spiritCounter")
				}
				equipment?.let {
					mapOf(
						it::unknown to ItemTypeMajor.None,
						it::neck to ItemTypeMajor.Amulet,
						it::chest to ItemTypeMajor.Chest,
						it::feet to ItemTypeMajor.Boots,
						it::hands to ItemTypeMajor.Gloves,
						it::shoulder to ItemTypeMajor.Shoulder,
						it::leftWeapon to ItemTypeMajor.Weapon,
						it::rightWeapon to ItemTypeMajor.Weapon,
						it::leftRing to ItemTypeMajor.Ring,
						it::rightRing to ItemTypeMajor.Ring,
						it::lamp to ItemTypeMajor.Lamp,
						it::special to ItemTypeMajor.Special,
						it::pet to ItemTypeMajor.Pet
					).filterNot { it.key.get().typeMajor == ItemTypeMajor.None }.forEach {
						val item = it.key.get()
						val prefix = "equipment." + it.key.name

						item.typeMajor.expect(it.value, "$prefix.typeMajor")

						val classMajor = current.combatClassMajor
						val allowedMaterials = when (it.value) {
							ItemTypeMajor.Weapon -> {
								item.typeMinor.expectIn(getAllowedWeaponTypes(classMajor), "$prefix.typeMinor")
								allowedWeaponMaterials[item.typeMinor]!!
							}
							ItemTypeMajor.Chest,
							ItemTypeMajor.Boots,
							ItemTypeMajor.Gloves,
							ItemTypeMajor.Shoulder -> getAllowedMaterialsArmor(classMajor)

							ItemTypeMajor.Amulet,
							ItemTypeMajor.Ring -> allowedMaterialsAccessories

							ItemTypeMajor.Special -> {
								item.typeMinor.expectIn(subTypesSpecial, "$prefix.typeMinor")
								setOf(Material.Wood)
							}
							ItemTypeMajor.Lamp -> setOf(Material.Iron)
							else -> setOf(Material.None)
						}
						item.material.expectIn(allowedMaterials, "$prefix.material")
						//item.randomSeed.expectMinimum(0, "$prefix.randomSeed")
						item.recipe.expect(ItemTypeMajor.None, "$prefix.recipe")
						item.rarity.expectIn(allowedRarities, "$prefix.rarity")

						val powerAllowed = Utils.computePower(current.level)
						val powerActual = Utils.computePower(item.level.toInt())
						powerActual.expectIn(1..powerAllowed, "$prefix.level")

						val spiritLimit =
							32//if (item.typeMajor == ItemTypeMajor.Weapon) getWeaponHandCount(item.typeMinor) * 16 else 0
						item.spiritCounter.expectIn(0..spiritLimit, "$prefix.spiritCounter")

						item.spirits.take(item.spiritCounter).forEach {
							@Suppress("NAME_SHADOWING") //intentional
							val allowedMaterials = setOf(
								Material.Fire,
								Material.Unholy,
								Material.IceSpirit,
								Material.Wind,
								item.material
							)
							//it.material.expectIn(allowedMaterials, "$prefix.spirits[?].material")
							//it.level.expect(1..item.level, "$prefix.spirits[?].level")
						}
					}
					val r =
						if (it.rightWeapon.typeMajor == ItemTypeMajor.None) 0 else getWeaponHandCount(it.rightWeapon.typeMinor)
					val l =
						if (it.leftWeapon.typeMajor == ItemTypeMajor.None) 0 else getWeaponHandCount(it.leftWeapon.typeMinor)
					(r + l).expectMaximum(2, "equipment.dualwield")
					//ranger can hold 2h weapon in either hand

				}
				name?.length?.expectIn(1..15, "name")
				skillPointDistribution?.let {
					//TODO: must not exceed available points
				}
				manaCubes?.expectMinimum(0, "manaCubes")
			}
		} catch (ex: CheaterException) {
			return ex.message
		}
		return null
	}

	private fun <T : Comparable<T>> T.expectMinimum(minimum: T, message: String) {
		if (this < minimum) {
			throw CheaterException(message)
		}
	}

	private fun <T : Comparable<T>> T.expectMaximum(maximum: T, message: String) {
		if (this > maximum) {
			throw CheaterException("$message was $this, expected <=$maximum")
		}
	}

	private fun <T> T.expect(expected: T, message: String) {
		if (this != expected) {
			throw CheaterException("$message was $this, expected $expected")
		}
	}

	private fun <T> T.expectIn(expected: Set<T>, message: String) {
		if (this !in expected) {
			throw CheaterException("$message was $this, expected $expected")
		}
	}

	private fun <T : Comparable<T>> T.expectIn(expected: ClosedRange<T>, message: String) {
		if (this !in expected) {
			throw CheaterException("$message was $this, expected $expected")
		}
	}
}