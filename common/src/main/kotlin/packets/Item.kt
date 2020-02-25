package packets

import utils.*

data class Item(
	val mainType: ItemMainType,
	val subType: Byte,
	val paddingA: Short,
	val randomSeed: Int,
	val recipe: ItemMainType,
	val rarity: Rarity,
	val material: Material,
	val adapted: Boolean,
	val paddingB: Byte,
	val level: Short,
	val paddingC: Short,
	val spirits: Array<Spirit>,
	val spiritCounter: Int
) : CwSerializable, SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeByte(mainType.value)
		writer.writeByte(subType)
		writer.writeShort(paddingA)
		writer.writeInt(randomSeed)
		writer.writeByte(recipe.value); writer.pad(3)
		writer.writeByte(rarity.value)
		writer.writeByte(material.value)
		writer.writeBoolean(adapted)
		writer.writeByte(paddingB)
		writer.writeShort(level)
		writer.writeShort(paddingC)
		spirits.forEach {
			it.writeTo(writer)
		}
		writer.writeInt(spiritCounter)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Item {
			return Item(
				mainType = ItemMainType(reader.readByte()),
				subType = reader.readByte(),
				paddingA = reader.readShort(),
				randomSeed = reader.readInt(),
				recipe = ItemMainType(reader.readInt().toByte()),//TODO: cleanup
				rarity = Rarity(reader.readByte()),
				material = Material(reader.readByte()),
				adapted = reader.readBoolean(),
				paddingB = reader.readByte(),
				level = reader.readShort(),
				paddingC = reader.readShort(),
				spirits = Array<Spirit>(32) {
					Spirit.read(reader)
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
) : CwSerializable {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector3Byte(position)
		writer.writeByte(material.value)
		writer.writeShort(level)
		writer.writeShort(padding)
	}

	companion object {
		internal suspend fun read(reader: Reader): Spirit {
			return Spirit(
				position = reader.readVector3Byte(),
				material = Material(reader.readByte()),
				level = reader.readShort(),
				padding = reader.readShort()
			)
		}
	}
}

inline class ItemMainType(val value: Byte) {
	companion object {
		var None = ItemMainType(0)
		var Food = ItemMainType(1)
		var Formula = ItemMainType(2)
		var Weapon = ItemMainType(3)
		var Chest = ItemMainType(4)
		var Gloves = ItemMainType(5)
		var Boots = ItemMainType(6)
		var Shoulder = ItemMainType(7)
		var Amulet = ItemMainType(8)
		var Ring = ItemMainType(9)
		var Block = ItemMainType(10)
		var Resource = ItemMainType(11)
		var Coin = ItemMainType(12)
		var PlatinumCoin = ItemMainType(13)
		var Leftovers = ItemMainType(14)
		var Beak = ItemMainType(15)
		var Painting = ItemMainType(16)
		var Vase = ItemMainType(17)
		var Candle = ItemMainType(18)
		var Pet = ItemMainType(19)
		var PetFood = ItemMainType(20)
		var Quest = ItemMainType(21)
		var Unknown = ItemMainType(22)
		var Special = ItemMainType(23)
		var Lamp = ItemMainType(24)
		var ManaCube = ItemMainType(25)
	}
}

inline class Rarity(val value: Byte) {
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
		//var Fire = Material(128)
		//var Unholy = Material(129)
		//var IceSpirit = Material(130)
		//var Wind = Material(131)
	}
}

inline class ItemSubTypeWeapon(val value: Byte) {
	companion object {
		val Sword = ItemSubTypeWeapon(0)
		val Axe = ItemSubTypeWeapon(1)
		val Mace = ItemSubTypeWeapon(2)
		val Dagger = ItemSubTypeWeapon(3)
		val Fist = ItemSubTypeWeapon(4)
		val Longsword = ItemSubTypeWeapon(5)
		val Bow = ItemSubTypeWeapon(6)
		val Crossbow = ItemSubTypeWeapon(7)
		val Boomerang = ItemSubTypeWeapon(8)
		val Arrow = ItemSubTypeWeapon(9)
		val Staff = ItemSubTypeWeapon(10)
		val Wand = ItemSubTypeWeapon(11)
		val Bracelet = ItemSubTypeWeapon(12)
		val Shield = ItemSubTypeWeapon(13)
		val Quiver = ItemSubTypeWeapon(14)
		val Greatsword = ItemSubTypeWeapon(15)
		val Greataxe = ItemSubTypeWeapon(16)
		val Greatmace = ItemSubTypeWeapon(17)
		val Pitchfork = ItemSubTypeWeapon(18)
		val Pickaxe = ItemSubTypeWeapon(19)
		val Torch = ItemSubTypeWeapon(20)
	}
}