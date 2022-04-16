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
	private val allowedMaterialsAccessories = setOf(Item.Material.Gold, Item.Material.Silver)
	private val allowedRarities = setOf(
		Item.Rarity.Normal,
		Item.Rarity.Uncommon,
		Item.Rarity.Rare,
		Item.Rarity.Epic,
		Item.Rarity.Legendary
	)
	private val subTypesSpecial = setOf(
		Item.Type.Minor.Special.Boat,
		Item.Type.Minor.Special.HangGlider
	)
	private val allowedWeaponMaterials = mapOf(
		Item.Type.Minor.Weapon.Sword to setOf(Item.Material.Iron, Item.Material.Obsidian, Item.Material.Bone),
		Item.Type.Minor.Weapon.Axe to setOf(Item.Material.Iron),
		Item.Type.Minor.Weapon.Mace to setOf(Item.Material.Iron),
		Item.Type.Minor.Weapon.Dagger to setOf(Item.Material.Iron),
		Item.Type.Minor.Weapon.Fist to setOf(Item.Material.Iron),
		Item.Type.Minor.Weapon.Longsword to setOf(Item.Material.Iron),
		Item.Type.Minor.Weapon.Bow to setOf(Item.Material.Wood),
		Item.Type.Minor.Weapon.Crossbow to setOf(Item.Material.Wood),
		Item.Type.Minor.Weapon.Boomerang to setOf(Item.Material.Wood),
		Item.Type.Minor.Weapon.Staff to setOf(Item.Material.Wood, Item.Material.Obsidian),
		Item.Type.Minor.Weapon.Wand to setOf(Item.Material.Wood),
		Item.Type.Minor.Weapon.Bracelet to setOf(Item.Material.Gold, Item.Material.Silver),
		Item.Type.Minor.Weapon.Shield to setOf(Item.Material.Iron, Item.Material.Wood),
		Item.Type.Minor.Weapon.Greatsword to setOf(Item.Material.Iron),
		Item.Type.Minor.Weapon.Greataxe to setOf(Item.Material.Iron, Item.Material.Saurian),
		Item.Type.Minor.Weapon.Greatmace to setOf(Item.Material.Iron, Item.Material.Wood),

		Item.Type.Minor.Weapon.Arrow to setOf(Item.Material.None),
		Item.Type.Minor.Weapon.Quiver to setOf(Item.Material.None),
		Item.Type.Minor.Weapon.Pitchfork to setOf(Item.Material.None),
		Item.Type.Minor.Weapon.Pickaxe to setOf(Item.Material.None),
		Item.Type.Minor.Weapon.Torch to setOf(Item.Material.None),
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

	private fun getAllowedAnimations(weaponTypeMinor: Item.Type.Minor?, classMajor: CombatClassMajor, classMinor: CombatClassMinor) =
		setOf(
			Animation.Idle,
			Animation.Drinking,
			Animation.Eating,
			Animation.PetFoodPresent,
			Animation.Sitting,
			Animation.Sleeping,
			Animation.Riding,
			Animation.Boat
		) + when (classMajor) {
			CombatClassMajor.Warrior ->
				setOf(
					Animation.Smash,
					Animation.Cyclone
				)
			CombatClassMajor.Ranger ->
				setOf(
					Animation.Kick
				)
			CombatClassMajor.Mage ->
				setOf(
					Animation.Teleport
				) + if (classMinor == CombatClassMinor.Mage.Fire)
					Animation.FireExplosionShort
				else
					Animation.HealingStream
			CombatClassMajor.Rogue ->
				setOf(
					Animation.Intercept,
					Animation.Stealth
				) + if (classMinor == CombatClassMinor.Rogue.Ninja)
					setOf(Animation.Shuriken)
				else
					setOf()
			else -> error("class should be sanity checked already")
		} + when (weaponTypeMinor) {
			null ->
				setOf(
					Animation.UnarmedM2,
					Animation.UnarmedM2Charging
				) + if (classMajor == CombatClassMajor.Mage) {
					if (classMinor == CombatClassMinor.Mage.Water) setOf(
						Animation.BraceletsWaterM1a,
						Animation.BraceletsWaterM1b,
						Animation.BraceletWaterM2
					) else setOf(
						Animation.BraceletsFireM1a,
						Animation.BraceletsFireM1b,
						Animation.BraceletFireM2
					)
				} else setOf(
					Animation.UnarmedM1a,
					Animation.UnarmedM1b
				)
			Item.Type.Minor.Weapon.Dagger ->
				setOf(
					Animation.DaggersM1a,
					Animation.DaggersM1b,
					Animation.DaggersM2
				)
			Item.Type.Minor.Weapon.Fist ->
				setOf(
					Animation.UnarmedM1a,
					Animation.UnarmedM1b,
					Animation.FistsM2
				)
			Item.Type.Minor.Weapon.Longsword ->
				setOf(
					Animation.LongswordM1a,
					Animation.LongswordM1b,
					Animation.LongswordM2
				)
			Item.Type.Minor.Weapon.Bow ->
				setOf(
					Animation.ShootArrow,
					Animation.BowM2,
					Animation.BowM2Charging
				)
			Item.Type.Minor.Weapon.Crossbow ->
				setOf(
					Animation.ShootArrow,
					Animation.CrossbowM2,
					Animation.CrossbowM2Charging
				)
			Item.Type.Minor.Weapon.Boomerang ->
				setOf(
					Animation.BoomerangM1,
					Animation.BoomerangM2Charging
				)
			Item.Type.Minor.Weapon.Staff ->
				if (classMinor == CombatClassMinor.Mage.Water) setOf(
					Animation.StaffWaterM1,
					Animation.StaffWaterM2
				) else setOf(
					Animation.StaffFireM1,
					Animation.StaffFireM2
				)
			Item.Type.Minor.Weapon.Wand ->
				if (classMinor == CombatClassMinor.Mage.Water) setOf(
					Animation.WandWaterM1,
					Animation.WandWaterM2,
				) else setOf(
					Animation.WandFireM1,
					Animation.WandFireM2
				)
			Item.Type.Minor.Weapon.Bracelet ->
				if (classMinor == CombatClassMinor.Mage.Water) setOf(
					Animation.BraceletsWaterM1a,
					Animation.BraceletsWaterM1b,
					Animation.BraceletWaterM2
				) else setOf(
					Animation.BraceletsFireM1a,
					Animation.BraceletsFireM1b,
					Animation.BraceletFireM2
				)
			Item.Type.Minor.Weapon.Shield ->
				setOf(
					Animation.ShieldM1a,
					Animation.ShieldM1b,
					Animation.ShieldM2,
					Animation.ShieldM2Charging
				)
			Item.Type.Minor.Weapon.Greatsword,
			Item.Type.Minor.Weapon.Greataxe,
			Item.Type.Minor.Weapon.Greatmace,
			Item.Type.Minor.Weapon.Pitchfork ->
				setOf(
					Animation.GreatweaponM1a,
					Animation.GreatweaponM1b,
					Animation.GreatweaponM1c,
					Animation.GreatweaponM2Charging,
				) + if (classMinor == CombatClassMinor.Warrior.Guardian)
					Animation.GreatweaponM2Guardian
				else
					Animation.GreatweaponM2Berserker
//		Item.Type.Minor.Weapon.Sword,
//		Item.Type.Minor.Weapon.Axe,
//		Item.Type.Minor.Weapon.Mace,
//		Item.Type.Minor.Weapon.Arrow,
//		Item.Type.Minor.Weapon.Quiver,
//		Item.Type.Minor.Weapon.Pickaxe,
//		Item.Type.Minor.Weapon.Torch,
			else -> setOf(
				Animation.DualWieldM1a,
				Animation.DualWieldM1b,
				Animation.UnarmedM2,
				Animation.UnarmedM2Charging
			)
		}

	private fun getAllowedMaterialsArmor(combatClassMajor: CombatClassMajor) =
		when (combatClassMajor) {
			CombatClassMajor.Warrior -> setOf(
				Item.Material.Iron,
				Item.Material.Obsidian,
				Item.Material.Saurian,
				Item.Material.Ice
			)
			CombatClassMajor.Ranger -> setOf(
				Item.Material.Parrot,
				Item.Material.Linen
			)
			CombatClassMajor.Mage -> setOf(
				Item.Material.Licht,
				Item.Material.Silk
			)
			CombatClassMajor.Rogue -> setOf(
				Item.Material.Cotton
			)
			else -> setOf()
		} + setOf( //these can be worn by any class
			Item.Material.Bone,
			Item.Material.Mammoth,
			Item.Material.Gold
		)

	private fun getAllowedWeaponTypes(combatClassMajor: CombatClassMajor) =
		when (combatClassMajor) {
			CombatClassMajor.Warrior -> setOf(
				Item.Type.Minor.Weapon.Sword,
				Item.Type.Minor.Weapon.Axe,
				Item.Type.Minor.Weapon.Mace,
				Item.Type.Minor.Weapon.Shield,
				Item.Type.Minor.Weapon.Greatsword,
				Item.Type.Minor.Weapon.Greataxe,
				Item.Type.Minor.Weapon.Greatmace
			)
			CombatClassMajor.Ranger -> setOf(
				Item.Type.Minor.Weapon.Bow,
				Item.Type.Minor.Weapon.Crossbow,
				Item.Type.Minor.Weapon.Boomerang,
				Item.Type.Minor.Weapon.Quiver
			)
			CombatClassMajor.Mage -> setOf(
				Item.Type.Minor.Weapon.Staff,
				Item.Type.Minor.Weapon.Wand,
				Item.Type.Minor.Weapon.Bracelet,
			)
			CombatClassMajor.Rogue -> setOf(
				Item.Type.Minor.Weapon.Dagger,
				Item.Type.Minor.Weapon.Fist,
				Item.Type.Minor.Weapon.Longsword,
			)
			else -> error("class should be sanity checked already")
		} + setOf(//class agnostic
			Item.Type.Minor.Weapon.Pitchfork,
			Item.Type.Minor.Weapon.Pickaxe,
			Item.Type.Minor.Weapon.Torch,
			Item.Type.Minor.Weapon.Arrow
		)

	private fun getWeaponHandCount(weaponType: Item.Type.Minor) =
		when (weaponType) {
			Item.Type.Minor.Weapon.Longsword,
			Item.Type.Minor.Weapon.Bow,
			Item.Type.Minor.Weapon.Crossbow,
			Item.Type.Minor.Weapon.Boomerang,
			Item.Type.Minor.Weapon.Staff,
			Item.Type.Minor.Weapon.Wand,
			Item.Type.Minor.Weapon.Greatsword,
			Item.Type.Minor.Weapon.Greataxe,
			Item.Type.Minor.Weapon.Greatmace,
			Item.Type.Minor.Weapon.Pitchfork -> 2

//			Item.Type.Minor.Weapon.Bracelet,
//			Item.Type.Minor.Weapon.Shield,
//			Item.Type.Minor.Weapon.Quiver,
//			Item.Type.Minor.Weapon.Arrow,
//			Item.Type.Minor.Weapon.Sword,
//			Item.Type.Minor.Weapon.Axe,
//			Item.Type.Minor.Weapon.Mace,
//			Item.Type.Minor.Weapon.Dagger,
//			Item.Type.Minor.Weapon.Fist,
//			Item.Type.Minor.Weapon.Pickaxe,
//			Item.Type.Minor.Weapon.Torch -> 1

			else -> 1
		}

	private val lastFireSpam = mutableMapOf<CreatureId, Long>()
	private val lastAnimation = mutableMapOf<CreatureId, Long>()

	fun inspect(creatureUpdate: CreatureUpdate, previous: Creature): String? {
		val current = previous.copy().apply { update(creatureUpdate) } //TODO: optimize
		try {
			with(creatureUpdate) {
				id.expect(previous.id, "id")

				position?.let {}
				rotation?.let {
					//usually 0, except
					//- rounding errors
					//- 60f..0 when swimming (or shortly afterwards)
					//- 20f when teleporting
					it.x.expectIn(-Float.MAX_VALUE..Float.MAX_VALUE, "rotation.pitch")
					it.y.expectIn(-90f..90f, "rotation.roll")
					it.z.isFinite().expect(true, "rotation.yaw.isFinite")
//					if (current.animationTime > 10_000) { //overflows while attacking
//						it.z.expectIn(-180f..180f, "rotation.yaw")
//					}
				}
				velocity?.let {}//can change with abilites
				acceleration?.let {
//					val actualXY = sqrt(it.x.pow(2) + it.y.pow(2))
//					if (!current.flags[CreatureFlag.Gliding]) {
//						actualXY.expectIn(0f..accelLimitXY, "acceleration.XY")
//					}
					if (current.flagsPhysics[PhysicsFlag.Swimming]) {
						it.z.expectIn(-80f..80f, "acceleration.Z")
					} else if (current.flags[CreatureFlag.Climbing]){
						it.z.expectIn(setOf(0f, -16f, 16f), "acceleration.Z")
					} else {
						it.z.expect(0f, "acceleration.Z")
					}


				}
				velocityExtra?.let {
					var limitXY = 0.1f //game doesnt reset all the way to 0
					var limitZ = 0f
					if (current.combatClassMajor == CombatClassMajor.Ranger) {
						limitXY = 35f
						limitZ = 17f
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
				animation?.let {
					val leftWeaponTypeMinor = current.equipment.leftWeapon.typeMinor
					val weaponTypeMinor = when {
						leftWeaponTypeMinor in setOf(
							Item.Type.Minor.Weapon.Bow,
							Item.Type.Minor.Weapon.Crossbow,
							Item.Type.Minor.Weapon.Shield
						) -> leftWeaponTypeMinor

						current.equipment.rightWeapon.typeMajor == Item.Type.Major.None -> null

						else -> current.equipment.rightWeapon.typeMinor
					}
					it.expectIn(
						getAllowedAnimations(
							weaponTypeMinor,
							current.combatClassMajor,
							current.combatClassMinor
						),
						"animation"
					)
				}
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
					it.tailRotation.expect(0f, "appearance.tail_rotation")

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
					it[CreatureFlag.FriendlyFire].expect(false, "flags.friendlyFire")
					val isRanger = current.combatClassMajor == CombatClassMajor.Ranger
					val isSniper = current.combatClassMinor == CombatClassMinor.Ranger.Sniper
					if (!isRanger || !isSniper) {
						it[CreatureFlag.Sniping].expect(false, "flags.sniping")
					}
					if (current.equipment.lamp.typeMajor == Item.Type.Major.None) {
						it[CreatureFlag.Lamp].expect(false, "flags.lamp")
					}
				}
				effectTimeDodge?.expectIn(0..600, "effectTimeDodge")
				effectTimeStun?.let {}
				effectTimeFear?.expectMinimum(0, "effectTimeFear")
				effectTimeIce?.expectMinimum(0, "effectTimeIce")
				effectTimeWind?.expectMaximum(5000, "effectTimeWind")
				showPatchTime?.let {}
				combatClassMajor?.ordinal?.expectIn(1..4, "combatClassMajor")
				combatClassMinor?.ordinal?.expectIn(0..1, "combatClassMinor")
				manaCharge?.expectMaximum(current.mana, "manaCharge") //might go negative with blocking bug
				unknown24?.let {}
				unknown25?.let {}
				aimDisplacement?.let {
					val distance = sqrt(it.x.pow(2) + it.y.pow(2) + it.z.pow(2))
					//maximum is 60 + some rounding errors by default, exceeds when moving
					//distance.expectIn(0f..63f, "aimDisplacement")
				}
				health?.expectMaximum(current.healthMaximum * 1.01f, "health")//TODO: fix rounding errors
				mana?.let {
					it.expectMaximum(1f, "mana")//m1, ninja dodge, block
					if (current.combatClassMajor != CombatClassMajor.Mage
						&& current.animationTime > 1000
						&& current.animation !in setOf(Animation.Stealth, Animation.ShieldM2Charging)
						&& !current.flags[CreatureFlag.Sniping]
					) {//TODO: assassin camo
						//TODO: leaving stealth still generates mp for some time
						//it.expectMaximum(previous.mana, "mana")
					}
				}
				blockingGauge?.let {
					it.expectMaximum(1f, "blockingGauge")
					val chargables = setOf(
						Animation.ShieldM2Charging,
						Animation.DualWieldM2Charging,
						Animation.GreatweaponM2Charging,
						Animation.UnarmedM2Charging
					)
					if (current.animation in chargables && current.animationTime > 100) {
						//TODO: quick release and recharge breaks this
						it.expectMaximum(previous.blockMeter, "blockingGauge")
					}
				}
				multipliers?.let {
					it.health.expect(100f, "multipliers.health")
					it.attackSpeed.expect(1f, "multipliers.attackSpeed")
					it.damage.expect(1f, "multipliers.damage")
					it.resi.expect(1f, "multipliers.resi")
					it.armor.expect(1f, "multipliers.armor")
				}
				unknown31?.let {}
				unknown32?.let {}
				level?.expectIn(1..500, "level")
				experience?.expectIn(0..Utils.computeMaxExperience(current.level), "experience")
				master?.expect(CreatureId(0), "master")
				unknown36?.let {}
				powerBase?.let {}
				unknown38?.let {}
				homeChunk?.let {}
				home?.let {}
				chunkToReveal?.let {}
				unknown42?.let {}
				consumable?.let {
					if (it.typeMajor == Item.Type.Major.None) return@let

					it.typeMajor.expect(Item.Type.Major.Food, "consumable.typeMajor")
					it.rarity.expect(Item.Rarity.Normal, "consumable.rarity")

					val powerAllowed = Utils.computePower(current.level)
					val powerActual = Utils.computePower(it.level.toInt())

					powerActual.expectIn(1..powerAllowed, "consumable.level")
					//it.spiritCounter.expect(0, "consumable.spiritCounter")
				}
				equipment?.let {
					mapOf(
						it::unknown to Item.Type.Major.None,
						it::neck to Item.Type.Major.Amulet,
						it::chest to Item.Type.Major.Chest,
						it::feet to Item.Type.Major.Boots,
						it::hands to Item.Type.Major.Gloves,
						it::shoulder to Item.Type.Major.Shoulder,
						it::leftWeapon to Item.Type.Major.Weapon,
						it::rightWeapon to Item.Type.Major.Weapon,
						it::leftRing to Item.Type.Major.Ring,
						it::rightRing to Item.Type.Major.Ring,
						it::lamp to Item.Type.Major.Lamp,
						it::special to Item.Type.Major.Special,
						it::pet to Item.Type.Major.Pet
					).filterNot { it.key.get().typeMajor == Item.Type.Major.None }.forEach {
						val item = it.key.get()
						val prefix = "equipment." + it.key.name

						item.typeMajor.expect(it.value, "$prefix.typeMajor")

						val classMajor = current.combatClassMajor
						val allowedItemMaterials = when (it.value) {
							Item.Type.Major.Weapon -> {
								item.typeMinor.expectIn(getAllowedWeaponTypes(classMajor), "$prefix.typeMinor")
								allowedWeaponMaterials[item.typeMinor]!!
							}
							Item.Type.Major.Chest,
							Item.Type.Major.Boots,
							Item.Type.Major.Gloves,
							Item.Type.Major.Shoulder -> getAllowedMaterialsArmor(classMajor)

							Item.Type.Major.Amulet,
							Item.Type.Major.Ring -> allowedMaterialsAccessories

							Item.Type.Major.Special -> {
								item.typeMinor.expectIn(subTypesSpecial, "$prefix.typeMinor")
								setOf(Item.Material.Wood)
							}
							Item.Type.Major.Lamp -> setOf(Item.Material.Iron)
							else -> setOf(Item.Material.None)
						}
						item.material.expectIn(allowedItemMaterials, "$prefix.material")
						//item.randomSeed.expectMinimum(0, "$prefix.randomSeed")
						item.recipe.expect(Item.Type.Major.None, "$prefix.recipe")
						item.rarity.expectIn(allowedRarities, "$prefix.rarity")

						val powerAllowed = Utils.computePower(current.level)
						val powerActual = Utils.computePower(item.level.toInt())
						powerActual.expectIn(1..powerAllowed, "$prefix.level")

						val spiritLimit = 32//if (item.typeMajor == Item.Type.Major.Weapon) getWeaponHandCount(item.typeMinor) * 16 else 0
						item.spiritCounter.expectIn(0..spiritLimit, "$prefix.spiritCounter")

//						val allowedSpiritMaterials = setOf(
//							Item.Material.Fire,
//							Item.Material.Unholy,
//							Item.Material.IceSpirit,
//							Item.Material.Wind,
//							item.material
//						)
//						item.spirits.take(item.spiritCounter).forEachIndexed { index, spirit ->
//							spirit.material.expectIn(allowedSpiritMaterials, "$prefix.spirit#$index.material")
//							spirit.level.toInt().expectIn(1..item.level, "$prefix.spirit#$index.level")
//						}
					}
					val r = if (it.rightWeapon == Item.void) 0 else getWeaponHandCount(it.rightWeapon.typeMinor)
					val l = if (it.leftWeapon == Item.void) 0 else getWeaponHandCount(it.leftWeapon.typeMinor)
					(r + l).expectMaximum(2, "equipment.dualwield")
					//ranger can hold 2h weapon in either hand

				}
				name?.length?.expectIn(1..15, "name.length")
				skillPointDistribution?.let {
					listOf(
						it::petMaster,
						it::petRiding,
						it::sailing,
						it::climbing,
						it::hangGliding,
						it::swimming,
						it::ability1,
						it::ability2,
						it::ability3,
						it::ability4,
						it::ability5
					).run {
						val available = (current.level - 1) * 2
						forEach {
							it.get().expectMinimum(0, "skillPoints.${it.name}")
						}
						map { it.get() }.sum().expectMaximum(available, "skillPoints.total")
					}
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