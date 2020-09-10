package packets

import utils.*
import utils.readVector3Byte
import utils.writeVector3Byte
import kotlin.math.*

data class Item(
	val typeMajor: ItemTypeMajor,
	val typeMinor: ItemTypeMinor,
	val paddingA: Short,
	val randomSeed: Int,
	val recipe: ItemTypeMajor,
	val paddingB: Byte,
	val paddingC: Short,
	val rarity: Rarity,
	val material: Material,
	val flags: FlagSet<ItemFlag>,
	val paddingD: Byte,
	val level: Short,
	val paddingE: Short,
	val spirits: Array<Spirit>,
	val spiritCounter: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeByte(typeMajor.value)
		writer.writeByte(typeMinor.value)
		writer.writeShort(paddingA)
		writer.writeInt(randomSeed)
		writer.writeByte(recipe.value)
		writer.writeByte(paddingB)
		writer.writeShort(paddingC)
		writer.writeByte(rarity.value)
		writer.writeByte(material.value)
		writer.writeByte(flags.inner.toByte())
		writer.writeByte(paddingD)
		writer.writeShort(level)
		writer.writeShort(paddingE)
		spirits.forEach {
			it.writeTo(writer)
		}
		writer.writeInt(spiritCounter)
	}

	val hp: Float
		get() {
			if (typeMajor.value !in 3..7) return 0f

			val multiplierTypeMajor = if (typeMajor == ItemTypeMajor.Chest) 1f else 0.5f

			var hp_mod = 8L * randomSeed % 21
			if (hp_mod < 0) {
				hp_mod += 4294967296
			}
			val multiplierRandom = 2f - (hp_mod / 20f) + when (material) {
				Material.Iron -> 1f
				Material.Linen -> 0.5f
				Material.Cotton -> 0.75f
				else -> 0f
			}

			val multiplierRarity = 2f.pow(rarity.value * 0.25f)

			val lvl = level + spiritCounter * 0.1f
			val multiplierLevel = 2f.pow((1f - (1f / Utils.levelScalingFactor(lvl))) * 3f)

			return setOf(
				multiplierLevel,
				multiplierRandom,
				multiplierRarity,
				multiplierTypeMajor
			).fold(5f) { accumulator, multiplier ->
				accumulator * multiplier
			}
		}

	companion object {
		internal suspend fun readFrom(reader: Reader): Item {

			return Item(
				typeMajor = ItemTypeMajor(reader.readByte()),
				typeMinor = ItemTypeMinor(reader.readByte()),
				paddingA = reader.readShort(),
				randomSeed = reader.readInt(),
				recipe = ItemTypeMajor(reader.readByte()),
				paddingB = reader.readByte(),
				paddingC = reader.readShort(),
				rarity = Rarity(reader.readByte()),
				material = Material(reader.readByte()),
				flags = FlagSet(reader.readByte().toBooleanArray()),
				paddingD = reader.readByte(),
				level = reader.readShort(),
				paddingE = reader.readShort(),
				spirits = Array(32) {
					Spirit.readFrom(reader)
				},
				spiritCounter = reader.readInt()
			)
		}
	}
}

data class Spirit(
	val position: Vector3<Byte>,
	val material: Material,
	val level: Short,
	val padding: Short
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector3Byte(position)
		writer.writeByte(material.value)
		writer.writeShort(level)
		writer.writeShort(padding)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Spirit {
			return Spirit(
				position = reader.readVector3Byte(),
				material = Material(reader.readByte()),
				level = reader.readShort(),
				padding = reader.readShort()
			)
		}
	}
}

inline class ItemTypeMajor(val value: Byte) {
	companion object {
		var None = ItemTypeMajor(0)
		var Food = ItemTypeMajor(1)
		var Formula = ItemTypeMajor(2)
		var Weapon = ItemTypeMajor(3)
		var Chest = ItemTypeMajor(4)
		var Gloves = ItemTypeMajor(5)
		var Boots = ItemTypeMajor(6)
		var Shoulder = ItemTypeMajor(7)
		var Amulet = ItemTypeMajor(8)
		var Ring = ItemTypeMajor(9)
		var Block = ItemTypeMajor(10)
		var Resource = ItemTypeMajor(11)
		var Coin = ItemTypeMajor(12)
		var PlatinumCoin = ItemTypeMajor(13)
		var Leftovers = ItemTypeMajor(14)
		var Beak = ItemTypeMajor(15)
		var Painting = ItemTypeMajor(16)
		var Vase = ItemTypeMajor(17)
		var Candle = ItemTypeMajor(18)
		var Pet = ItemTypeMajor(19)
		var PetFood = ItemTypeMajor(20)
		var Quest = ItemTypeMajor(21)
		var Unknown = ItemTypeMajor(22)
		var Special = ItemTypeMajor(23)
		var Lamp = ItemTypeMajor(24)
		var ManaCube = ItemTypeMajor(25)
	}
}

inline class Rarity(val value: Byte) {
	operator fun compareTo(other: Rarity) = this.value.compareTo(other.value)

	companion object {
		var Normal = Rarity(0)
		var Uncommon = Rarity(1)
		var Rare = Rarity(2)
		var Epic = Rarity(3)
		var Legendary = Rarity(4)
		var Mythic = Rarity(5)
	}
}

inline class Material(val value: Byte) {
	companion object {
		var None = Material(0)
		var Iron = Material(1)
		var Wood = Material(2)

		//
		//
		var Obsidian = Material(5)
		var Unknown = Material(6)
		var Bone = Material(7)

		//
		//
		//
		var Gold = Material(11)
		var Silver = Material(12)
		var Emerald = Material(13)
		var Sapphire = Material(14)
		var Ruby = Material(15)
		var Diamond = Material(16)
		var Sandstone = Material(17)
		var Saurian = Material(18)
		var Parrot = Material(19)
		var Mammoth = Material(20)
		var Plant = Material(21)
		var Ice = Material(22)
		var Licht = Material(23)
		var Glass = Material(24)
		var Silk = Material(25)
		var Linen = Material(26)
		var Cotton = Material(27)
		//...
		var Fire = Material(-128)
		var Unholy = Material(-127)
		var IceSpirit = Material(-126)
		var Wind = Material(-125)
	}
}

inline class ItemTypeMinor(val value: Byte) {
	object Food {
		val Cookie = ItemTypeMinor(0)
		val LifePotion = ItemTypeMinor(1)
		val CactusPotion = ItemTypeMinor(2)
		val ManaPotion = ItemTypeMinor(3)
		val GinsengSoup = ItemTypeMinor(4)
		val SnowberryMash = ItemTypeMinor(5)
		val MushroomSpit = ItemTypeMinor(6)
		val Bomb = ItemTypeMinor(7)
		val PineappleSlice = ItemTypeMinor(8)
		val PumpkinMuffin = ItemTypeMinor(9)
	}

	object Weapon {
		val Sword = ItemTypeMinor(0)
		val Axe = ItemTypeMinor(1)
		val Mace = ItemTypeMinor(2)
		val Dagger = ItemTypeMinor(3)
		val Fist = ItemTypeMinor(4)
		val Longsword = ItemTypeMinor(5)
		val Bow = ItemTypeMinor(6)
		val Crossbow = ItemTypeMinor(7)
		val Boomerang = ItemTypeMinor(8)
		val Arrow = ItemTypeMinor(9)
		val Staff = ItemTypeMinor(10)
		val Wand = ItemTypeMinor(11)
		val Bracelet = ItemTypeMinor(12)
		val Shield = ItemTypeMinor(13)
		val Quiver = ItemTypeMinor(14)
		val Greatsword = ItemTypeMinor(15)
		val Greataxe = ItemTypeMinor(16)
		val Greatmace = ItemTypeMinor(17)
		val Pitchfork = ItemTypeMinor(18)
		val Pickaxe = ItemTypeMinor(19)
		val Torch = ItemTypeMinor(20)
	}

	object Resource {
		val Nugget = ItemTypeMinor(0)
		val Log = ItemTypeMinor(1)
		val Feather = ItemTypeMinor(2)
		val Horn = ItemTypeMinor(3)
		val Claw = ItemTypeMinor(4)
		val Fiber = ItemTypeMinor(5)
		val Cobweb = ItemTypeMinor(6)
		val Hair = ItemTypeMinor(7)
		val Crystal = ItemTypeMinor(8)
		val Yarn = ItemTypeMinor(9)
		val Cube = ItemTypeMinor(10)
		val Capsule = ItemTypeMinor(11)
		val Flask = ItemTypeMinor(12)
		val Orb = ItemTypeMinor(13)
		val Spirit = ItemTypeMinor(14)
		val Mushroom = ItemTypeMinor(15)
		val Pumpkin = ItemTypeMinor(16)
		val Pineapple = ItemTypeMinor(17)
		val RadishSlice = ItemTypeMinor(18)
		val ShimmerMushroom = ItemTypeMinor(19)
		val GinsengRoot = ItemTypeMinor(20)
		val OnionSlice = ItemTypeMinor(21)
		val Heartflower = ItemTypeMinor(22)
		val PricklyPear = ItemTypeMinor(23)
		val Iceflower = ItemTypeMinor(24)
		val Soulflower = ItemTypeMinor(25)
		val WaterFlask = ItemTypeMinor(26)
		val Snowberry = ItemTypeMinor(27)
	}

	object Candle {
		val Red = ItemTypeMinor(0)
		val Green = ItemTypeMinor(1)
	}

	object Quest {
		val AmuletYellow = ItemTypeMinor(0)
		val AmuletBlue = ItemTypeMinor(1)
		val Jewelcase = ItemTypeMinor(2)
		val Key = ItemTypeMinor(3)
		val Medicine = ItemTypeMinor(4)
		val Anitvenom = ItemTypeMinor(5)
		val Bandaid = ItemTypeMinor(6)
		val Crutch = ItemTypeMinor(7)
		val Bandage = ItemTypeMinor(8)
		val Salve = ItemTypeMinor(9)
	}

	object Special {
		val HangGlider = ItemTypeMinor(0)
		val Boat = ItemTypeMinor(1)
	}
}

inline class ItemFlag(override val value: Int) : FlagSetIndex {
	companion object {
		val adapted = ItemFlag(0)
	}
}