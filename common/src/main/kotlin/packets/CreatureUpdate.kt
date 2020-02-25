package packets

import io.ktor.util.toByteArray
import kotlinx.coroutines.io.ByteChannel
import kotlinx.coroutines.io.close
import utils.*

data class CreatureUpdate(
	val id: Long,
	val position: Vector3<Long>? = null,
	val rotation: Vector3<Float>? = null,
	val velocity: Vector3<Float>? = null,
	val acceleration: Vector3<Float>? = null,
	val extraVel: Vector3<Float>? = null,
	val viewportPitch: Float? = null,
	val physicsFlags: BooleanArray? = null,
	val hostility: Hostility? = null,
	val creatureType: CreatureType? = null,
	val mode: Mode? = null,
	val modeTimer: Int? = null,
	val combo: Int? = null,
	val lastHitTime: Int? = null,
	val appearance: Appearance? = null,
	val creatureFlags: BooleanArray? = null,
	val roll: Int? = null,
	val stun: Int? = null,
	val slow: Int? = null,
	val ice: Int? = null,
	val wind: Int? = null,
	val showPatchTime: Int? = null,
	val entityClass: EntityClass? = null,
	val specialization: Byte? = null,
	val charge: Float? = null,
	val unused24: Vector3<Float>? = null,
	val unused25: Vector3<Float>? = null,
	val rayHit: Vector3<Float>? = null,
	val HP: Float? = null,
	val MP: Float? = null,
	val block: Float? = null,
	val multipliers: Multipliers? = null,
	val unused31: Byte? = null,
	val unused32: Byte? = null,
	val level: Int? = null,
	val XP: Int? = null,
	val parentOwner: Long? = null,
	val unused36: Long? = null,
	val powerBase: Byte? = null,
	val unused38: Int? = null,
	val unused39: Vector3<Int>? = null,
	val spawnPos: Vector3<Long>? = null,
	val unused41: Vector3<Int>? = null,
	val unused42: Byte? = null,
	val consumable: Item? = null,
	val equipment: Equipment? = null,
	val name: String? = null,
	val skillDistribution: SkillDistribution? = null,
	val manaCubes: Int? = null
) : Packet(Opcode.CreatureUpdate) {
	override suspend fun writeTo(writer: Writer) {
		val optionalDataChannel = ByteChannel(true)
		val optionalDataWriter = Writer(optionalDataChannel)
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
		extraVel?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[4] = true
		}
		viewportPitch?.let {
			optionalDataWriter.writeFloat(it)
			mask[5] = true
		}
		physicsFlags?.let {
			optionalDataWriter.writeInt(it.toInt())
			mask[6] = true
		}
		hostility?.let {
			optionalDataWriter.writeByte(it.value)
			mask[7] = true
		}
		creatureType?.let {
			optionalDataWriter.writeInt(it.value)
			mask[8] = true
		}
		mode?.let {
			optionalDataWriter.writeByte(it.value)
			mask[9] = true
		}
		modeTimer?.let {
			optionalDataWriter.writeInt(it)
			mask[10] = true
		}
		combo?.let {
			optionalDataWriter.writeInt(it)
			mask[11] = true
		}
		lastHitTime?.let {
			optionalDataWriter.writeInt(it)
			mask[12] = true
		}
		appearance?.let {
			it.writeTo(optionalDataWriter)
			mask[13] = true
		}
		creatureFlags?.let {
			optionalDataWriter.writeShort(it.toInt().toShort())
			mask[14] = true
		}
		roll?.let {
			optionalDataWriter.writeInt(it)
			mask[15] = true
		}
		stun?.let {
			optionalDataWriter.writeInt(it)
			mask[16] = true
		}
		slow?.let {
			optionalDataWriter.writeInt(it)
			mask[17] = true
		}
		ice?.let {
			optionalDataWriter.writeInt(it)
			mask[18] = true
		}
		wind?.let {
			optionalDataWriter.writeInt(it)
			mask[19] = true
		}
		showPatchTime?.let {
			optionalDataWriter.writeInt(it)
			mask[20] = true
		}
		entityClass?.let {
			optionalDataWriter.writeByte(it.value)
			mask[21] = true
		}
		specialization?.let {
			optionalDataWriter.writeByte(it)
			mask[22] = true
		}
		charge?.let {
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
		rayHit?.let {
			optionalDataWriter.writeVector3Float(it)
			mask[26] = true
		}
		HP?.let {
			optionalDataWriter.writeFloat(it)
			mask[27] = true
		}
		MP?.let {
			optionalDataWriter.writeFloat(it)
			mask[28] = true
		}
		block?.let {
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
		XP?.let {
			optionalDataWriter.writeInt(it)
			mask[34] = true
		}
		parentOwner?.let {
			optionalDataWriter.writeLong(it)
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
		spawnPos?.let {
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
		skillDistribution?.let {
			it.writeTo(optionalDataWriter)
			mask[46] = true
		}
		manaCubes?.let {
			optionalDataWriter.writeInt(it)
			mask[47] = true
		}

		optionalDataChannel.close()
		val optionalData = optionalDataChannel.toByteArray()

		val inflatedChannel = ByteChannel(true)
		val inflatedWriter = Writer(inflatedChannel)

		inflatedWriter.writeLong(id)
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

			val id = inflatedReader.readLong()
			val mask = inflatedReader.readLong().toBooleanArray()

			return CreatureUpdate(
				id = id,
				position = if (mask[0]) inflatedReader.readVector3Long() else null,
				rotation = if (mask[1]) inflatedReader.readVector3Float() else null,
				velocity = if (mask[2]) inflatedReader.readVector3Float() else null,
				acceleration = if (mask[3]) inflatedReader.readVector3Float() else null,
				extraVel = if (mask[4]) inflatedReader.readVector3Float() else null,
				viewportPitch = if (mask[5]) inflatedReader.readFloat() else null,
				physicsFlags = if (mask[6]) inflatedReader.readInt().toBooleanArray() else null,
				hostility = if (mask[7]) Hostility(inflatedReader.readByte()) else null,
				creatureType = if (mask[8]) CreatureType(inflatedReader.readInt()) else null,
				mode = if (mask[9]) Mode(inflatedReader.readByte()) else null,
				modeTimer = if (mask[10]) inflatedReader.readInt() else null,
				combo = if (mask[11]) inflatedReader.readInt() else null,
				lastHitTime = if (mask[12]) inflatedReader.readInt() else null,
				appearance = if (mask[13]) Appearance.read(inflatedReader) else null,
				creatureFlags = if (mask[14]) inflatedReader.readShort().toInt().toBooleanArray() else null,
				roll = if (mask[15]) inflatedReader.readInt() else null,
				stun = if (mask[16]) inflatedReader.readInt() else null,
				slow = if (mask[17]) inflatedReader.readInt() else null,
				ice = if (mask[18]) inflatedReader.readInt() else null,
				wind = if (mask[19]) inflatedReader.readInt() else null,
				showPatchTime = if (mask[20]) inflatedReader.readInt() else null,
				entityClass = if (mask[21]) EntityClass(inflatedReader.readByte()) else null,
				specialization = if (mask[22]) inflatedReader.readByte() else null,
				charge = if (mask[23]) inflatedReader.readFloat() else null,
				unused24 = if (mask[24]) inflatedReader.readVector3Float() else null,
				unused25 = if (mask[25]) inflatedReader.readVector3Float() else null,
				rayHit = if (mask[26]) inflatedReader.readVector3Float() else null,
				HP = if (mask[27]) inflatedReader.readFloat() else null,
				MP = if (mask[28]) inflatedReader.readFloat() else null,
				block = if (mask[29]) inflatedReader.readFloat() else null,
				multipliers = if (mask[30]) Multipliers.read(inflatedReader) else null,
				unused31 = if (mask[31]) inflatedReader.readByte() else null,
				unused32 = if (mask[32]) inflatedReader.readByte() else null,
				level = if (mask[33]) inflatedReader.readInt() else null,
				XP = if (mask[34]) inflatedReader.readInt() else null,
				parentOwner = if (mask[35]) inflatedReader.readLong() else null,
				unused36 = if (mask[36]) inflatedReader.readLong() else null,
				powerBase = if (mask[37]) inflatedReader.readByte() else null,
				unused38 = if (mask[38]) inflatedReader.readInt() else null,
				unused39 = if (mask[39]) inflatedReader.readVector3Int() else null,
				spawnPos = if (mask[40]) inflatedReader.readVector3Long() else null,
				unused41 = if (mask[41]) inflatedReader.readVector3Int() else null,
				unused42 = if (mask[42]) inflatedReader.readByte() else null,
				consumable = if (mask[43]) Item.readFrom(inflatedReader) else null,
				equipment = if (mask[44]) Equipment.read(inflatedReader) else null,
				name = if (mask[45]) inflatedReader.readName() else null,
				skillDistribution = if (mask[46]) SkillDistribution.read(inflatedReader) else null,
				manaCubes = if (mask[47]) inflatedReader.readInt() else null
			)
		}

		private suspend fun Reader.readName(): String {
			val stringBytes = this.readByteArray(16)
			val nameWithNulls = stringBytes.toString(Charsets.UTF_8)
			return nameWithNulls.subSequence(0, nameWithNulls.indexOf(Char.MIN_VALUE)) as String
		}
	}
}

data class Appearance(
	var unknownA: Byte,
	var unknownB: Byte,
	var hairColor: Vector3<Byte>,
	var flags: BooleanArray,
	var unknownC: Byte,
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
		writer.writeShort(flags.toInt().toShort())
		writer.writeByte(unknownC)
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
		internal suspend fun read(reader: Reader): Appearance {
			return Appearance(
				unknownA = reader.readByte(),
				unknownB = reader.readByte(),
				hairColor = reader.readVector3Byte(),
				flags = reader.readShort().toInt().toBooleanArray(),
				unknownC = reader.readByte(),
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
	var HP: Float,
	var attackSpeed: Float,
	var damage: Float,
	var armor: Float,
	var resi: Float
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeFloat(HP)
		writer.writeFloat(attackSpeed)
		writer.writeFloat(damage)
		writer.writeFloat(armor)
		writer.writeFloat(resi)
	}

	companion object {
		internal suspend fun read(reader: Reader): Multipliers {
			return Multipliers(
				HP = reader.readFloat(),
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
		internal suspend fun read(reader: Reader): Equipment {
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
		internal suspend fun read(reader: Reader): SkillDistribution {
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

inline class PhysicsFlagsIndex(val value: Int) {
	companion object {
		val onGround = PhysicsFlagsIndex(0)
		val swimming = PhysicsFlagsIndex(1)
		val touchingWall = PhysicsFlagsIndex(2)


		val pushingWall = PhysicsFlagsIndex(5)
		val pushingObject = PhysicsFlagsIndex(6)
	}
}

inline class CreatureFlagsIndex(val value: Int) {
	companion object {
		val climbing = CreatureFlagsIndex(0)

		val aiming = CreatureFlagsIndex(2)

		val gliding = CreatureFlagsIndex(4)
		val friendlyFire = CreatureFlagsIndex(5)
		val sprinting = CreatureFlagsIndex(6)


		val lamp = CreatureFlagsIndex(9)
		val sniping = CreatureFlagsIndex(10)
	}
}

inline class AppearanceFlagsIndex(val value: Int) {
	companion object {
		val fourLegged = AppearanceFlagsIndex(0)
		val canFly = AppearanceFlagsIndex(1)


		val stuck = AppearanceFlagsIndex(8) //used by dummies
		val bossBuff = AppearanceFlagsIndex(9)


		val invincibility = AppearanceFlagsIndex(13) //used by dummies
	}
}

inline class Hostility(val value: Byte) {
	companion object {
		val Player = Hostility(0)
		val Enemy = Hostility(1)
		val Unknown2 = Hostility(2)
		val NPC = Hostility(3)
		val Unknown4 = Hostility(4)
		val Pet = Hostility(5)
		val Neutral = Hostility(6)
	}
}

inline class CreatureType(val value: Int) {
	companion object {
		val ElfMale = CreatureType(0)
		val ElfFemale = CreatureType(1)
		val HumanMale = CreatureType(2)
		val HumanFemale = CreatureType(3)
		val GoblinMale = CreatureType(4)
		val GoblinFemale = CreatureType(5)
		val TerrierBull = CreatureType(6)
		val LizardmanMale = CreatureType(7)
		val LizardmanFemale = CreatureType(8)
		val DwarfMale = CreatureType(9)
		val DwarfFemale = CreatureType(10)
		val OrcMale = CreatureType(11)
		val OrcFemale = CreatureType(12)
		val FrogmanMale = CreatureType(13)
		val FrogmanFemale = CreatureType(14)
		val UndeadMale = CreatureType(15)
		val UndeadFemale = CreatureType(16)
		val Skeleton = CreatureType(17)
		val OldMan = CreatureType(18)
		val Collie = CreatureType(19)
		val ShepherdDog = CreatureType(20)
		val SkullBull = CreatureType(21)
		val Alpaca = CreatureType(22)
		val AlpacaBrown = CreatureType(23)
		val Egg = CreatureType(24)
		val Turtle = CreatureType(25)
		val Terrier = CreatureType(26)
		val TerrierScottish = CreatureType(27)
		val Wolf = CreatureType(28)
		val Panther = CreatureType(29)
		val Cat = CreatureType(30)
		val CatBrown = CreatureType(31)
		val CatWhite = CreatureType(32)
		val Pig = CreatureType(33)
		val Sheep = CreatureType(34)
		val Bunny = CreatureType(35)
		val Porcupine = CreatureType(36)
		val SlimeGreen = CreatureType(37)
		val SlimePink = CreatureType(38)
		val SlimeYellow = CreatureType(39)
		val SlimeBlue = CreatureType(40)
		val Frightener = CreatureType(41)
		val Sandhorror = CreatureType(42)
		val Wizard = CreatureType(43)
		val Bandit = CreatureType(44)
		val Witch = CreatureType(45)
		val Ogre = CreatureType(46)
		val Rockling = CreatureType(47)
		val Gnoll = CreatureType(48)
		val GnollPolar = CreatureType(49)
		val Monkey = CreatureType(50)
		val Gnobold = CreatureType(51)
		val Insectoid = CreatureType(52)
		val Hornet = CreatureType(53)
		val InsectGuard = CreatureType(54)
		val Crow = CreatureType(55)
		val Chicken = CreatureType(56)
		val Seagull = CreatureType(57)
		val Parrot = CreatureType(58)
		val Bat = CreatureType(59)
		val Fly = CreatureType(60)
		val Midge = CreatureType(61)
		val Mosquito = CreatureType(62)
		val RunnerPlain = CreatureType(63)
		val RunnerLeaf = CreatureType(64)
		val RunnerSnow = CreatureType(65)
		val RunnerDesert = CreatureType(66)
		val Peacock = CreatureType(67)
		val Frog = CreatureType(68)
		val CreaturePlant = CreatureType(69)
		val CreatureRadish = CreatureType(70)
		val Onionling = CreatureType(71)
		val OnionlingDesert = CreatureType(72)
		val Devourer = CreatureType(73)
		val Duckbill = CreatureType(74)
		val Crocodile = CreatureType(75)
		val CreatureSpike = CreatureType(76)
		val Anubis = CreatureType(77)
		val Horus = CreatureType(78)
		val Jester = CreatureType(79)
		val Spectrino = CreatureType(80)
		val Djinn = CreatureType(81)
		val Minotaur = CreatureType(82)
		val NomadMale = CreatureType(83)
		val NomadFemale = CreatureType(84)
		val Imp = CreatureType(85)
		val Spitter = CreatureType(86)
		val Mole = CreatureType(87)
		val Biter = CreatureType(88)
		val Koala = CreatureType(89)
		val Squirrel = CreatureType(90)
		val Raccoon = CreatureType(91)
		val Owl = CreatureType(92)
		val Penguin = CreatureType(93)
		val Werewolf = CreatureType(94)
		val Santa = CreatureType(95)
		val Zombie = CreatureType(96)
		val Vampire = CreatureType(97)
		val Horse = CreatureType(98)
		val Camel = CreatureType(99)
		val Cow = CreatureType(100)
		val Dragon = CreatureType(101)
		val BeetleDark = CreatureType(102)
		val BeetleFire = CreatureType(103)
		val BeetleSnout = CreatureType(104)
		val BeetleLemon = CreatureType(105)
		val Crab = CreatureType(106)
		val CrabSea = CreatureType(107)
		val Troll = CreatureType(108)
		val TrollDark = CreatureType(109)
		val Helldemon = CreatureType(110)
		val Golem = CreatureType(111)
		val GolemEmber = CreatureType(112)
		val GolemSnow = CreatureType(113)
		val Yeti = CreatureType(114)
		val Cyclops = CreatureType(115)
		val Mammoth = CreatureType(116)
		val Lich = CreatureType(117)
		val Runegiant = CreatureType(118)
		val Saurian = CreatureType(119)
		val Bush = CreatureType(120)
		val BushSnow = CreatureType(121)
		val BushSnowberry = CreatureType(122)
		val PlantCotton = CreatureType(123)
		val Scrub = CreatureType(124)
		val ScrubCobweg = CreatureType(125)
		val ScrubFire = CreatureType(126)
		val Ginseng = CreatureType(127)
		val Cactus = CreatureType(128)
		val ChristmasTree = CreatureType(129)
		val Thorntree = CreatureType(130)
		val DepositGold = CreatureType(131)
		val DepositIron = CreatureType(132)
		val DepositSilver = CreatureType(133)
		val DepositSandstone = CreatureType(134)
		val DepositEmerald = CreatureType(135)
		val DepositSapphire = CreatureType(136)
		val DepositRuby = CreatureType(137)
		val DepositDiamond = CreatureType(138)
		val DepositIcecrystal = CreatureType(139)
		val Scarecrow = CreatureType(140)
		val Aim = CreatureType(141)
		val Dummy = CreatureType(142)
		val Vase = CreatureType(143)
		val Bomb = CreatureType(144)
		val FishSapphire = CreatureType(145)
		val FishLemon = CreatureType(146)
		val Seahorse = CreatureType(147)
		val Mermaid = CreatureType(148)
		val Merman = CreatureType(149)
		val Shark = CreatureType(150)
		val Bumblebee = CreatureType(151)
		val Lanternfish = CreatureType(152)
		val Mawfish = CreatureType(153)
		val Piranha = CreatureType(154)
		val Blowfish = CreatureType(155)
	}
}

inline class Mode(val value: Byte) {
	companion object {
		val Idle = Mode(0)
		val War_Dual_Wield001 = Mode(1)
		val War_dual_Wield002 = Mode(2)
		val Unknown3 = Mode(3)
		val Unknown4 = Mode(4)
		val Carge_After_Longsword = Mode(5)
		val Fists1 = Mode(6)
		val Fists2 = Mode(7)
		val Charge_During_Shield = Mode(8)
		val Shield2 = Mode(9)
		val Shield1 = Mode(10)
		val Charge_After_NoWeapon = Mode(11)
		val Unknown012 = Mode(12)
		val Longsword2 = Mode(13)
		val Longsword1 = Mode(14)
		val Unknown015 = Mode(15)
		val Unknown016 = Mode(16)
		val Charge_After_Daggers = Mode(17)
		val Daggers2 = Mode(18)
		val Daggers1 = Mode(19)
		val Fists_After_Kick = Mode(20)
		val Unknown021 = Mode(21)
		val Shoot_Arrow = Mode(22)
		val Charge_After_Crossbow = Mode(23)
		val Charge_During_Crossbow = Mode(24)
		val Charge_During_Bow = Mode(25)
		val Boomerang = Mode(26)
		val Charge_During_Boomerang = Mode(27)
		val Beam_Instant = Mode(28)
		val Unknown029 = Mode(29)
		val Staff_Fire = Mode(30)
		val Charge_After_Staff_Fire = Mode(31)
		val Staff_Water = Mode(32)
		val Charge_After_Staff_Water = Mode(33)
		val After_Healing_Stream = Mode(34)
		val Unknown035 = Mode(35)
		val Unknown036 = Mode(36)
		val Charge_After_Bracelet = Mode(37)
		val Wand = Mode(38)
		val Bracelets2_Fire = Mode(39)
		val Bracelets1_Fire = Mode(40)
		val Bracelets2_Water = Mode(41)
		val Bracelets1_Water = Mode(42)
		val Charge_After_Wand_C = Mode(43)
		val Wand_B = Mode(44)
		val Charge_After_Wand_B = Mode(45)
		val Charge_After_Wand = Mode(46)
		val Smash = Mode(47)
		val After_Intercept = Mode(48)
		val After_Teleport = Mode(49)
		val Shuriken = Mode(50)
		val Unknown051 = Mode(51)
		val Unknown052 = Mode(52)
		val Unknown053 = Mode(53)
		val After_Smash = Mode(54)
		val Charge_After_Bow = Mode(55)
		val Unknown056 = Mode(56)
		val Greatweapon1 = Mode(57)
		val Greatweapon3 = Mode(58)
		val Charge_During_Greatweapon = Mode(59)
		val Unknown060 = Mode(60)
		val Charge_After_Greatweapon = Mode(61)
		val Unknown062 = Mode(62)
		val Charge_During_NoWeapon = Mode(63)
		val Charge_During_Dual_Wield = Mode(64)
		val SpinA = Mode(65)
		val SpinB = Mode(66)
		val Greatweapon2 = Mode(67)
		val Boss_Skill_Knockdown1 = Mode(68)
		val Boss_Skill_Knockdown22 = Mode(69)
		val Boss_Skill_Spinkick = Mode(70)
		val Boss_Skill_Block = Mode(71)
		val Boss_Skill_Spin = Mode(72)
		val Boss_Skill_Cry = Mode(73)
		val Boss_Skill_Stomp = Mode(74)
		val Boss_Skill_Kick = Mode(75)
		val Boss_Skill_Knockdown3 = Mode(76)
		val Boss_Skill_Knockdown4 = Mode(77)
		val Boss_Skill_Knockdown5 = Mode(78)
		val Stealth = Mode(79)
		val Drinking_Potion = Mode(80)
		val Eat = Mode(81)
		val Pet_Food = Mode(82)
		val Sitting = Mode(83)
		val Sleeping = Mode(84)
		val Unknown085 = Mode(85)
		val Cyclone = Mode(86)
		val FireExplosion_Big = Mode(87)
		val FireExplosion_After = Mode(88)
		val Lava = Mode(89)
		val Splash = Mode(90)
		val Earth_Shatter = Mode(91)
		val Clone = Mode(92)
		val Spin_Run = Mode(93)
		val FireBeam = Mode(94)
		val FireRay = Mode(95)
		val After_Shuriken = Mode(96)
		val Invalid097 = Mode(97)
		val Unknown098 = Mode(98)
		val Invalid099 = Mode(99)
		val Invalid100 = Mode(100)
		val SuperBulwalk = Mode(101)
		val Invalid102 = Mode(102)
		val SuperManaShield = Mode(103)
		val Charge_After_Shield = Mode(104)
		val Teleport_To_City = Mode(105)
		val Riding = Mode(106)
		val Boat = Mode(107)
		val Boulder_Toss = Mode(108)
		val ManaCube = Mode(109)
		val Unknown110 = Mode(110)
	}
}

inline class EntityClass(val value: Byte) {
	companion object {
		val Warrior = EntityClass(1)
		val Ranger = EntityClass(2)
		val Mage = EntityClass(3)
		val Rogue = EntityClass(4)
	}
}