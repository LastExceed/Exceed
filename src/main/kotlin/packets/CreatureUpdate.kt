package packets

import io.ktor.util.toByteArray
import io.ktor.utils.io.ByteChannel
import io.ktor.utils.io.close
import utils.*

data class CreatureUpdate(
	val id: CreatureID,
	val position: Vector3<Long>? = null,
	val rotation: Vector3<Float>? = null,
	val velocity: Vector3<Float>? = null,
	val acceleration: Vector3<Float>? = null,
	val velocityExtra: Vector3<Float>? = null,
	val climbAnimationState: Float? = null,
	val flagsPhysics: FlagSet<PhysicsFlag>? = null,
	val affiliation: Affiliation? = null,
	val race: Race? = null,
	val motion: Motion? = null,
	val motionTime: Int? = null,
	val combo: Int? = null,
	val hitTimeOut: Int? = null,
	val appearance: Appearance? = null,
	val flags: FlagSet<CreatureFlag>? = null,
	val effectTimeDodge: Int? = null,
	val effectTimeStun: Int? = null,
	val effectTimeFear: Int? = null,
	val effectTimeIce: Int? = null,
	val effectTimeWind: Int? = null,
	val showPatchTime: Int? = null,
	val combatClassMajor: CombatClassMajor? = null,
	val combatClassMinor: CombatClassMinor? = null,
	val manaCharge: Float? = null,
	val unused24: Vector3<Float>? = null,
	val unused25: Vector3<Float>? = null,
	val aimDisplacement: Vector3<Float>? = null,
	val health: Float? = null,
	val mana: Float? = null,
	val blockMeter: Float? = null,
	val multipliers: Multipliers? = null,
	val unused31: Byte? = null,
	val unused32: Byte? = null,
	val level: Int? = null,
	val experience: Int? = null,
	val master: CreatureID? = null,
	val unused36: Long? = null,
	val powerBase: Byte? = null,
	val unused38: Int? = null,
	val unused39: Vector3<Int>? = null,
	val home: Vector3<Long>? = null,
	val unused41: Vector3<Int>? = null,
	val unused42: Byte? = null,
	val consumable: Item? = null,
	val equipment: Equipment? = null,
	val name: String? = null,
	val skillPointDistribution: SkillDistribution? = null,
	val manaCubes: Int? = null
) : Packet(Opcode.CreatureUpdate) {
	override suspend fun writeTo(writer: Writer) {
		val optionalChannel = ByteChannel(true)
		val optionalDataWriter = Writer(optionalChannel)
		val mask = BooleanArray(Long.SIZE_BITS)

		position?.let {
			optionalDataWriter.writeVector3Long(it)
			mask[0] = true
		}
		rotation?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[1] = true
		}
		velocity?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[2] = true
		}
		acceleration?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[3] = true
		}
		velocityExtra?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[4] = true
		}
		climbAnimationState?.let {
			optionalDataWriter.writeFloat(it)
			mask[5] = true
		}
		flagsPhysics?.let {
			optionalDataWriter.writeInt(it.inner.toInt())
			mask[6] = true
		}
		affiliation?.let {
			optionalDataWriter.writeByte(it.value)
			mask[7] = true
		}
		race?.let {
			optionalDataWriter.writeInt(it.value)
			mask[8] = true
		}
		motion?.let {
			optionalDataWriter.writeByte(it.value)
			mask[9] = true
		}
		motionTime?.let {
			optionalDataWriter.writeInt(it)
			mask[10] = true
		}
		combo?.let {
			optionalDataWriter.writeInt(it)
			mask[11] = true
		}
		hitTimeOut?.let {
			optionalDataWriter.writeInt(it)
			mask[12] = true
		}
		appearance?.let {
			it.writeTo(optionalDataWriter)
			mask[13] = true
		}
		flags?.let {
			optionalDataWriter.writeShort(it.inner.toShort())
			mask[14] = true
		}
		effectTimeDodge?.let {
			optionalDataWriter.writeInt(it)
			mask[15] = true
		}
		effectTimeStun?.let {
			optionalDataWriter.writeInt(it)
			mask[16] = true
		}
		effectTimeFear?.let {
			optionalDataWriter.writeInt(it)
			mask[17] = true
		}
		effectTimeIce?.let {
			optionalDataWriter.writeInt(it)
			mask[18] = true
		}
		effectTimeWind?.let {
			optionalDataWriter.writeInt(it)
			mask[19] = true
		}
		showPatchTime?.let {
			optionalDataWriter.writeInt(it)
			mask[20] = true
		}
		combatClassMajor?.let {
			optionalDataWriter.writeByte(it.value)
			mask[21] = true
		}
		combatClassMinor?.let {
			optionalDataWriter.writeByte(it.value)
			mask[22] = true
		}
		manaCharge?.let {
			optionalDataWriter.writeFloat(it)
			mask[23] = true
		}
		unused24?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[24] = true
		}
		unused25?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[25] = true
		}
		aimDisplacement?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[26] = true
		}
		health?.let {
			optionalDataWriter.writeFloat(it)
			mask[27] = true
		}
		mana?.let {
			optionalDataWriter.writeFloat(it)
			mask[28] = true
		}
		blockMeter?.let {
			optionalDataWriter.writeFloat(it)
			mask[29] = true
		}
		multipliers?.let {
			it.writeTo(optionalDataWriter)
			mask[30] = true
		}
		unused31?.let {
			optionalDataWriter.writeByte(it)
			mask[31] = true
		}
		unused32?.let {
			optionalDataWriter.writeByte(it)
			mask[32] = true
		}
		level?.let {
			optionalDataWriter.writeInt(it)
			mask[33] = true
		}
		experience?.let {
			optionalDataWriter.writeInt(it)
			mask[34] = true
		}
		master?.let {
			optionalDataWriter.writeLong(it.value)
			mask[35] = true
		}
		unused36?.let {
			optionalDataWriter.writeLong(it)
			mask[36] = true
		}
		powerBase?.let {
			optionalDataWriter.writeByte(it)
			mask[37] = true
		}
		unused38?.let {
			optionalDataWriter.writeInt(it)
			mask[38] = true
		}
		unused39?.let {
			optionalDataWriter.writeVector3Int(it)
			mask[39] = true
		}
		home?.let {
			optionalDataWriter.writeVector3Long(it)
			mask[40] = true
		}
		unused41?.let {
			optionalDataWriter.writeVector3Int(it)
			mask[41] = true
		}
		unused42?.let {
			optionalDataWriter.writeByte(it)
			mask[42] = true
		}
		consumable?.let {
			it.writeTo(optionalDataWriter)
			mask[43] = true
		}
		equipment?.let {
			it.writeTo(optionalDataWriter)
			mask[44] = true
		}
		name?.let {
			val nameBytes = ByteArray(16)
			it.toByteArray(Charsets.UTF_8).copyInto(nameBytes)
			optionalDataWriter.writeByteArray(nameBytes)
			mask[45] = true
		}
		skillPointDistribution?.let {
			it.writeTo(optionalDataWriter)
			mask[46] = true
		}
		manaCubes?.let {
			optionalDataWriter.writeInt(it)
			mask[47] = true
		}

		optionalChannel.close()
		val optionalData = optionalChannel.toByteArray()

		val inflatedChannel = ByteChannel(true)
		val inflatedWriter = Writer(inflatedChannel)

		inflatedWriter.writeLong(id.value)
		inflatedWriter.writeLong(mask.toLong())
		inflatedWriter.writeByteArray(optionalData)

		inflatedChannel.close()
		val deflated = Zlib.deflate(inflatedChannel.toByteArray())

		writer.writeInt(deflated.size)
		writer.writeByteArray(deflated)
	}

	companion object {
		suspend fun readFrom(reader: Reader): CreatureUpdate {
			val length = reader.readInt()
			val deflated = reader.readByteArray(length)
			val inflated = Zlib.inflate(deflated)
			val inflatedReader = Reader(inflated)

			val id = CreatureID(inflatedReader.readLong())
			val mask = inflatedReader.readLong().toBooleanArray()

			return CreatureUpdate(
				id = id,
				position = if (mask[0]) inflatedReader.readVector3Long() else null,
				rotation = if (mask[1]) inflatedReader.readVector3Float() else null,
				velocity = if (mask[2]) inflatedReader.readVector3Float() else null,
				acceleration = if (mask[3]) inflatedReader.readVector3Float() else null,
				velocityExtra = if (mask[4]) inflatedReader.readVector3Float() else null,
				climbAnimationState = if (mask[5]) inflatedReader.readFloat() else null,
				flagsPhysics = if (mask[6]) FlagSet(inflatedReader.readInt().toBooleanArray()) else null,
				affiliation = if (mask[7]) Affiliation(inflatedReader.readByte()) else null,
				race = if (mask[8]) Race(inflatedReader.readInt()) else null,
				motion = if (mask[9]) Motion(inflatedReader.readByte()) else null,
				motionTime = if (mask[10]) inflatedReader.readInt() else null,
				combo = if (mask[11]) inflatedReader.readInt() else null,
				hitTimeOut = if (mask[12]) inflatedReader.readInt() else null,
				appearance = if (mask[13]) Appearance.readFrom(inflatedReader) else null,
				flags = if (mask[14]) FlagSet(inflatedReader.readShort().toBooleanArray()) else null,
				effectTimeDodge = if (mask[15]) inflatedReader.readInt() else null,
				effectTimeStun = if (mask[16]) inflatedReader.readInt() else null,
				effectTimeFear = if (mask[17]) inflatedReader.readInt() else null,
				effectTimeIce = if (mask[18]) inflatedReader.readInt() else null,
				effectTimeWind = if (mask[19]) inflatedReader.readInt() else null,
				showPatchTime = if (mask[20]) inflatedReader.readInt() else null,
				combatClassMajor = if (mask[21]) CombatClassMajor(inflatedReader.readByte()) else null,
				combatClassMinor = if (mask[22]) CombatClassMinor(inflatedReader.readByte())else null,
				manaCharge = if (mask[23]) inflatedReader.readFloat() else null,
				unused24 = if (mask[24]) inflatedReader.readVector3Float() else null,
				unused25 = if (mask[25]) inflatedReader.readVector3Float() else null,
				aimDisplacement = if (mask[26]) inflatedReader.readVector3Float() else null,
				health = if (mask[27]) inflatedReader.readFloat() else null,
				mana = if (mask[28]) inflatedReader.readFloat() else null,
				blockMeter = if (mask[29]) inflatedReader.readFloat() else null,
				multipliers = if (mask[30]) Multipliers.readFrom(inflatedReader) else null,
				unused31 = if (mask[31]) inflatedReader.readByte() else null,
				unused32 = if (mask[32]) inflatedReader.readByte() else null,
				level = if (mask[33]) inflatedReader.readInt() else null,
				experience = if (mask[34]) inflatedReader.readInt() else null,
				master = if (mask[35]) CreatureID(inflatedReader.readLong()) else null,
				unused36 = if (mask[36]) inflatedReader.readLong() else null,
				powerBase = if (mask[37]) inflatedReader.readByte() else null,
				unused38 = if (mask[38]) inflatedReader.readInt() else null,
				unused39 = if (mask[39]) inflatedReader.readVector3Int() else null,
				home = if (mask[40]) inflatedReader.readVector3Long() else null,
				unused41 = if (mask[41]) inflatedReader.readVector3Int() else null,
				unused42 = if (mask[42]) inflatedReader.readByte() else null,
				consumable = if (mask[43]) Item.readFrom(inflatedReader) else null,
				equipment = if (mask[44]) Equipment.readFrom(inflatedReader) else null,
				name = if (mask[45]) inflatedReader.readByteArray(16).toString(Charsets.UTF_8).trimEnd(Char.MIN_VALUE) else null,
				skillPointDistribution = if (mask[46]) SkillDistribution.readFrom(inflatedReader) else null,
				manaCubes = if (mask[47]) inflatedReader.readInt() else null
			)
		}
	}
}

inline class CreatureID(val value: Long)

data class Appearance(
	var unknownA: Byte,
	var unknownB: Byte,
	var hairColor: Vector3<Byte>,
	var unknownC: Byte,
	var flags: FlagSet<AppearanceFlag>,
	var creatureSize: Vector3<Float>,
	var headModel: Short,
	var hairModel: Short,
	var handModel: Short,
	var footModel: Short,
	var bodyModel: Short,
	var tailModel: Short,
	var shoulder2Model: Short,
	var wingModel: Short,
	var headSize: Float,
	var bodySize: Float,
	var handSize: Float,
	var footSize: Float,
	var shoulder2Size: Float,
	var weaponSize: Float,
	var tailSize: Float,
	var shoulder1Size: Float,
	var wingSize: Float,
	var bodyRotation: Float,
	var handRotation: Vector3<Float>,
	var feetRotation: Float,
	var wingRotation: Float,
	var tail_rotation: Float,
	var bodyOffset: Vector3<Float>,
	var headOffset: Vector3<Float>,
	var handOffset: Vector3<Float>,
	var footOffset: Vector3<Float>,
	var tailOffset: Vector3<Float>,
	var wingOffset: Vector3<Float>
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeByte(unknownA)
		writer.writeByte(unknownB)
		writer.writeVector3Byte(hairColor)
		writer.writeByte(unknownC)
		writer.writeShort(flags.inner.toShort())
		writer.writeVector3Float(creatureSize)
		writer.writeShort(headModel)
		writer.writeShort(hairModel)
		writer.writeShort(handModel)
		writer.writeShort(footModel)
		writer.writeShort(bodyModel)
		writer.writeShort(tailModel)
		writer.writeShort(shoulder2Model)
		writer.writeShort(wingModel)
		writer.writeFloat(headSize)
		writer.writeFloat(bodySize)
		writer.writeFloat(handSize)
		writer.writeFloat(footSize)
		writer.writeFloat(shoulder2Size)
		writer.writeFloat(weaponSize)
		writer.writeFloat(tailSize)
		writer.writeFloat(shoulder1Size)
		writer.writeFloat(wingSize)
		writer.writeFloat(bodyRotation)
		writer.writeVector3Float(handRotation)
		writer.writeFloat(feetRotation)
		writer.writeFloat(wingRotation)
		writer.writeFloat(tail_rotation)
		writer.writeVector3Float(bodyOffset)
		writer.writeVector3Float(headOffset)
		writer.writeVector3Float(handOffset)
		writer.writeVector3Float(footOffset)
		writer.writeVector3Float(tailOffset)
		writer.writeVector3Float(wingOffset)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Appearance {
			return Appearance(
				unknownA = reader.readByte(),
				unknownB = reader.readByte(),
				hairColor = reader.readVector3Byte(),
				unknownC = reader.readByte(),
				flags = FlagSet(reader.readShort().toBooleanArray()),
				creatureSize = reader.readVector3Float(),
				headModel = reader.readShort(),
				hairModel = reader.readShort(),
				handModel = reader.readShort(),
				footModel = reader.readShort(),
				bodyModel = reader.readShort(),
				tailModel = reader.readShort(),
				shoulder2Model = reader.readShort(),
				wingModel = reader.readShort(),
				headSize = reader.readFloat(),
				bodySize = reader.readFloat(),
				handSize = reader.readFloat(),
				footSize = reader.readFloat(),
				shoulder2Size = reader.readFloat(),
				weaponSize = reader.readFloat(),
				tailSize = reader.readFloat(),
				shoulder1Size = reader.readFloat(),
				wingSize = reader.readFloat(),
				bodyRotation = reader.readFloat(),
				handRotation = reader.readVector3Float(),
				feetRotation = reader.readFloat(),
				wingRotation = reader.readFloat(),
				tail_rotation = reader.readFloat(),
				bodyOffset = reader.readVector3Float(),
				headOffset = reader.readVector3Float(),
				handOffset = reader.readVector3Float(),
				footOffset = reader.readVector3Float(),
				tailOffset = reader.readVector3Float(),
				wingOffset = reader.readVector3Float()
			)
		}
	}
}

data class Multipliers(
	var health: Float,
	var attackSpeed: Float,
	var damage: Float,
	var armor: Float,
	var resi: Float
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeFloat(health)
		writer.writeFloat(attackSpeed)
		writer.writeFloat(damage)
		writer.writeFloat(armor)
		writer.writeFloat(resi)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Multipliers {
			return Multipliers(
				health = reader.readFloat(),
				attackSpeed = reader.readFloat(),
				damage = reader.readFloat(),
				armor = reader.readFloat(),
				resi = reader.readFloat()
			)
		}
	}
}

data class Equipment(
	var unknown: Item,
	var neck: Item,
	var chest: Item,
	var feet: Item,
	var hands: Item,
	var shoulder: Item,
	var leftWeapon: Item,
	var rightWeapon: Item,
	var leftRing: Item,
	var rightRing: Item,
	var lamp: Item,
	var special: Item,
	var pet: Item
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		unknown.writeTo(writer)
		neck.writeTo(writer)
		chest.writeTo(writer)
		feet.writeTo(writer)
		hands.writeTo(writer)
		shoulder.writeTo(writer)
		leftWeapon.writeTo(writer)
		rightWeapon.writeTo(writer)
		leftRing.writeTo(writer)
		rightRing.writeTo(writer)
		lamp.writeTo(writer)
		special.writeTo(writer)
		pet.writeTo(writer)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Equipment {
			return Equipment(
				unknown = Item.readFrom(reader),
				neck = Item.readFrom(reader),
				chest = Item.readFrom(reader),
				feet = Item.readFrom(reader),
				hands = Item.readFrom(reader),
				shoulder = Item.readFrom(reader),
				leftWeapon = Item.readFrom(reader),
				rightWeapon = Item.readFrom(reader),
				leftRing = Item.readFrom(reader),
				rightRing = Item.readFrom(reader),
				lamp = Item.readFrom(reader),
				special = Item.readFrom(reader),
				pet = Item.readFrom(reader)
			)
		}
	}
}

data class SkillDistribution(
	var petMaster: Int,
	var petRiding: Int,
	var sailing: Int,
	var climbing: Int,
	var hangGliding: Int,
	var swimming: Int,
	var ability1: Int,
	var ability2: Int,
	var ability3: Int,
	var ability4: Int,
	var ability5: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(petMaster)
		writer.writeInt(petRiding)
		writer.writeInt(sailing)
		writer.writeInt(climbing)
		writer.writeInt(hangGliding)
		writer.writeInt(swimming)
		writer.writeInt(ability1)
		writer.writeInt(ability2)
		writer.writeInt(ability3)
		writer.writeInt(ability4)
		writer.writeInt(ability5)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): SkillDistribution {
			return SkillDistribution(
				petMaster = reader.readInt(),
				petRiding = reader.readInt(),
				sailing = reader.readInt(),
				climbing = reader.readInt(),
				hangGliding = reader.readInt(),
				swimming = reader.readInt(),
				ability1 = reader.readInt(),
				ability2 = reader.readInt(),
				ability3 = reader.readInt(),
				ability4 = reader.readInt(),
				ability5 = reader.readInt()
			)
		}
	}
}

inline class PhysicsFlag(override val value: Int) : FlagSetIndex {
	companion object {
		val onGround = PhysicsFlag(0)
		val swimming = PhysicsFlag(1)
		val touchingWall = PhysicsFlag(2)


		val pushingWall = PhysicsFlag(5)
		val pushingObject = PhysicsFlag(6)
	}
}

inline class CreatureFlag(override val value: Int) : FlagSetIndex {
	companion object {
		val climbing = CreatureFlag(0)

		val aiming = CreatureFlag(2)

		val gliding = CreatureFlag(4)
		val friendlyFire = CreatureFlag(5)
		val sprinting = CreatureFlag(6)


		val lamp = CreatureFlag(9)
		val sniping = CreatureFlag(10)
	}
}

inline class AppearanceFlag(override val value: Int) : FlagSetIndex {
	companion object {
		val fourLegged = AppearanceFlag(0)
		val canFly = AppearanceFlag(1)


		val stuck = AppearanceFlag(8) //used by dummies
		val bossBuff = AppearanceFlag(9)


		val invincible = AppearanceFlag(13) //used by dummies
	}
}

inline class Affiliation(val value: Byte) {
	companion object {
		val Player = Affiliation(0)
		val Enemy = Affiliation(1)
		val Unknown2 = Affiliation(2)
		val NPC = Affiliation(3)
		val Unknown4 = Affiliation(4)
		val Pet = Affiliation(5)
		val Neutral = Affiliation(6)
	}
}

inline class Race(val value: Int) {
	companion object {
		val ElfMale = Race(0)
		val ElfFemale = Race(1)
		val HumanMale = Race(2)
		val HumanFemale = Race(3)
		val GoblinMale = Race(4)
		val GoblinFemale = Race(5)
		val TerrierBull = Race(6)
		val LizardmanMale = Race(7)
		val LizardmanFemale = Race(8)
		val DwarfMale = Race(9)
		val DwarfFemale = Race(10)
		val OrcMale = Race(11)
		val OrcFemale = Race(12)
		val FrogmanMale = Race(13)
		val FrogmanFemale = Race(14)
		val UndeadMale = Race(15)
		val UndeadFemale = Race(16)
		val Skeleton = Race(17)
		val OldMan = Race(18)
		val Collie = Race(19)
		val ShepherdDog = Race(20)
		val SkullBull = Race(21)
		val Alpaca = Race(22)
		val AlpacaBrown = Race(23)
		val Egg = Race(24)
		val Turtle = Race(25)
		val Terrier = Race(26)
		val TerrierScottish = Race(27)
		val Wolf = Race(28)
		val Panther = Race(29)
		val Cat = Race(30)
		val CatBrown = Race(31)
		val CatWhite = Race(32)
		val Pig = Race(33)
		val Sheep = Race(34)
		val Bunny = Race(35)
		val Porcupine = Race(36)
		val SlimeGreen = Race(37)
		val SlimePink = Race(38)
		val SlimeYellow = Race(39)
		val SlimeBlue = Race(40)
		val Frightener = Race(41)
		val Sandhorror = Race(42)
		val Wizard = Race(43)
		val Bandit = Race(44)
		val Witch = Race(45)
		val Ogre = Race(46)
		val Rockling = Race(47)
		val Gnoll = Race(48)
		val GnollPolar = Race(49)
		val Monkey = Race(50)
		val Gnobold = Race(51)
		val Insectoid = Race(52)
		val Hornet = Race(53)
		val InsectGuard = Race(54)
		val Crow = Race(55)
		val Chicken = Race(56)
		val Seagull = Race(57)
		val Parrot = Race(58)
		val Bat = Race(59)
		val Fly = Race(60)
		val Midge = Race(61)
		val Mosquito = Race(62)
		val RunnerPlain = Race(63)
		val RunnerLeaf = Race(64)
		val RunnerSnow = Race(65)
		val RunnerDesert = Race(66)
		val Peacock = Race(67)
		val Frog = Race(68)
		val CreaturePlant = Race(69)
		val CreatureRadish = Race(70)
		val Onionling = Race(71)
		val OnionlingDesert = Race(72)
		val Devourer = Race(73)
		val Duckbill = Race(74)
		val Crocodile = Race(75)
		val CreatureSpike = Race(76)
		val Anubis = Race(77)
		val Horus = Race(78)
		val Jester = Race(79)
		val Spectrino = Race(80)
		val Djinn = Race(81)
		val Minotaur = Race(82)
		val NomadMale = Race(83)
		val NomadFemale = Race(84)
		val Imp = Race(85)
		val Spitter = Race(86)
		val Mole = Race(87)
		val Biter = Race(88)
		val Koala = Race(89)
		val Squirrel = Race(90)
		val Raccoon = Race(91)
		val Owl = Race(92)
		val Penguin = Race(93)
		val Werewolf = Race(94)
		val Santa = Race(95)
		val Zombie = Race(96)
		val Vampire = Race(97)
		val Horse = Race(98)
		val Camel = Race(99)
		val Cow = Race(100)
		val Dragon = Race(101)
		val BeetleDark = Race(102)
		val BeetleFire = Race(103)
		val BeetleSnout = Race(104)
		val BeetleLemon = Race(105)
		val Crab = Race(106)
		val CrabSea = Race(107)
		val Troll = Race(108)
		val TrollDark = Race(109)
		val Helldemon = Race(110)
		val Golem = Race(111)
		val GolemEmber = Race(112)
		val GolemSnow = Race(113)
		val Yeti = Race(114)
		val Cyclops = Race(115)
		val Mammoth = Race(116)
		val Lich = Race(117)
		val Runegiant = Race(118)
		val Saurian = Race(119)
		val Bush = Race(120)
		val BushSnow = Race(121)
		val BushSnowberry = Race(122)
		val PlantCotton = Race(123)
		val Scrub = Race(124)
		val ScrubCobweg = Race(125)
		val ScrubFire = Race(126)
		val Ginseng = Race(127)
		val Cactus = Race(128)
		val ChristmasTree = Race(129)
		val Thorntree = Race(130)
		val DepositGold = Race(131)
		val DepositIron = Race(132)
		val DepositSilver = Race(133)
		val DepositSandstone = Race(134)
		val DepositEmerald = Race(135)
		val DepositSapphire = Race(136)
		val DepositRuby = Race(137)
		val DepositDiamond = Race(138)
		val DepositIcecrystal = Race(139)
		val Scarecrow = Race(140)
		val Aim = Race(141)
		val Dummy = Race(142)
		val Vase = Race(143)
		val Bomb = Race(144)
		val FishSapphire = Race(145)
		val FishLemon = Race(146)
		val Seahorse = Race(147)
		val Mermaid = Race(148)
		val Merman = Race(149)
		val Shark = Race(150)
		val Bumblebee = Race(151)
		val Lanternfish = Race(152)
		val Mawfish = Race(153)
		val Piranha = Race(154)
		val Blowfish = Race(155)
	}
}

inline class Motion(val value: Byte) {
	companion object {
		val Idle = Motion(0)
		val DualWieldM1a = Motion(1)
		val DualWieldM1b = Motion(2)
		val Unknown_003 = Motion(3) //like daggers
		val Unknown_004 = Motion(4)
		val LongswordM2 = Motion(5)
		val UnarmedM1a = Motion(6) //fists use these
		val UnarmedM1b = Motion(7)
		val ShieldM2Charging = Motion(8)
		val ShieldM1a = Motion(9)
		val ShieldM1b = Motion(10)
		val UnarmedM2 = Motion(11)
		val Unknown_012 = Motion(12) //swords rip apart
		val LongswordM1a = Motion(13)
		val LongswordM1b = Motion(14)
		val Unknown_015 = Motion(15) //probably for greatweapon A1
		val Unknown_016 = Motion(16) //same as 17
		val DaggersM2 = Motion(17)
		val DaggersM1a = Motion(18)
		val DaggersM1b = Motion(19)
		val FistsM2 = Motion(20)
		val Kick = Motion(21) //same as 20
		val ShootArrow = Motion(22)
		val CrossbowM2 = Motion(23)
		val CrossbowM2Charging = Motion(24)
		val BowM2Charging = Motion(25)
		val BoomerangM1 = Motion(26)
		val BoomerangM2Charging = Motion(27)
		val BeamDraining = Motion(28)
		val Unknown_029 = Motion(29) //nothing
		val StaffFireM1 = Motion(30)
		val StaffFireM2 = Motion(31)
		val StaffWaterM1 = Motion(32)
		val StaffWaterM2 = Motion(33)
		val HealingStream = Motion(34)
		val Unknown035 = Motion(35) //summon animation
		val Unknown036 = Motion(36) //wand charging?
		val BraceletFireM2 = Motion(37)
		val WandFireM1 = Motion(38)
		val BraceletsFireM1a = Motion(39)
		val BraceletsFireM1b = Motion(40)
		val BraceletsWaterM1a = Motion(41)
		val BraceletsWaterM1b = Motion(42)
		val BraceletWaterM2 = Motion(43)
		val WandWaterM1 = Motion(44)
		val WandWaterM2 = Motion(45)
		val WandFireM2 = Motion(46)
		val Unknown_047 = Motion(47) //same as 54
		val Intercept = Motion(48)
		val Teleport = Motion(49)
		val Unknown_050 = Motion(50)
		val Unknown_051 = Motion(51) //mob attack?
		val Unknown_052 = Motion(52) //nothing, immediately switches to 0
		val Unknown_053 = Motion(53) //nothing
		val Smash = Motion(54)
		val BowM2 = Motion(55)
		val Unknown_056 = Motion(56) //nothing, causes rotation lock
		val GreatweaponM1a = Motion(57)
		val GreatweaponM1c = Motion(58)
		val GreatweaponM2Charging = Motion(59)
		val GreatweaponM2Berserker = Motion(60)
		val GreatweaponM2Guardian = Motion(61)
		val Unknown_062 = Motion(62) //probably for greatweapon A2
		val UnarmedM2Charging = Motion(63)
		val DualWieldM2Charging = Motion(64)
		val Unknown_065 = Motion(65) //probably for greatweapon B1
		val Unknown_066 = Motion(66) //probably for greatweapon B2
		val GreatweaponM1b = Motion(67)
		val BossCharge1 = Motion(68)
		val BossCharge2 = Motion(69)
		val BossSpinkick = Motion(70)
		val BossBlock = Motion(71)
		val BossSpin = Motion(72)
		val BossCry = Motion(73)
		val BossStomp = Motion(74)
		val BossKick = Motion(75)
		val BossKnockdownForward = Motion(76)
		val BossKnockdownLeft = Motion(77)
		val BossKnockdownRight = Motion(78)
		val Stealth = Motion(79)
		val Drinking = Motion(80)
		val Eating = Motion(81)
		val PetFoodPresent = Motion(82)
		val Sitting = Motion(83)
		val Sleeping = Motion(84)
		val Unknown_085 = Motion(85) //nothing
		val Cyclone = Motion(86)
		val FireExplosionLong = Motion(87)
		val FireExplosionShort = Motion(88)
		val Lava = Motion(89)
		val Splash = Motion(90)
		val EarthQuake = Motion(91)
		val Clone = Motion(92)
		val Unknown_093 = Motion(93) //same as 48
		val FireBeam = Motion(94)
		val FireRay = Motion(95)
		val Shuriken = Motion(96)
		val Unknown_097 = Motion(97) //nothing, rotation lock
		val Unknown_098 = Motion(98) //parry? causes blocking
		val Unknwon_099 = Motion(99) //nothing, rotation lock
		val Unknown_100 = Motion(100) //nothing
		val SuperBulwalk = Motion(101) //casues bulwalk
		val Unknown_102 = Motion(102) //nothing
		val SuperManaShield = Motion(103) //causes manashield
		val ShieldM2 = Motion(104)
		val TeleportToCity = Motion(105)
		val Riding = Motion(106)
		val Boat = Motion(107)
		val Boulder = Motion(108)
		val ManaCubePickup = Motion(109)
		val Unknown_110 = Motion(110) //mob attack?
	}
}

inline class CombatClassMajor(val value: Byte) {
	companion object {
		val Warrior = CombatClassMajor(1)
		val Ranger = CombatClassMajor(2)
		val Mage = CombatClassMajor(3)
		val Rogue = CombatClassMajor(4)
	}
}

inline class CombatClassMinor(val value: Byte) {
	object Warrior {
		val Berserker = CombatClassMinor(0)
		val Guardian = CombatClassMinor(1)
	}

	object Ranger {
		val Sniper = CombatClassMinor(0)
		val Scout = CombatClassMinor(1)
	}

	object Mage {
		val Water = CombatClassMinor(0)
		val Fire = CombatClassMinor(1)
	}

	object Rogue {
		val Assassin = CombatClassMinor(0)
		val Ninja = CombatClassMinor(1)
	}
}