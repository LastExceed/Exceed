package packets

import utils.*

data class Buff(
	val source: CreatureID,
	val target: CreatureID,
	val type: BuffType,
	val unknownA: Byte,
	val unknownB: Short,
	val modifier: Float,
	val duration: Int,
	val unknownC: Int,
	val creatureID3: CreatureID
) : Packet(Opcode.Buff), SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(source.value)
		writer.writeLong(target.value)
		writer.writeByte(type.value)
		writer.writeByte(unknownA)
		writer.writeShort(unknownB)
		writer.writeFloat(modifier)
		writer.writeInt(duration)
		writer.writeInt(unknownC)
		writer.writeLong(creatureID3.value)
	}

	companion object {
		suspend fun readFrom(reader: Reader): Buff {
			return Buff(
				source = CreatureID(reader.readLong()),
				target = CreatureID(reader.readLong()),
				type = BuffType(reader.readByte()),
				unknownA = reader.readByte(),
				unknownB = reader.readShort(),
				modifier = reader.readFloat(),
				duration = reader.readInt(),
				unknownC = reader.readInt(),
				creatureID3 = CreatureID(reader.readLong())
			)
		}
	}
}

inline class BuffType(val value: Byte) {
	companion object {
		val Bulwalk = BuffType(1)
		val WarFrenzy = BuffType(2)
		val Camouflage = BuffType(3)
		val Poison = BuffType(4)
		val UnknownA = BuffType(5)
		val ManaShield = BuffType(6)
		val UnknownB = BuffType(7)
		val UnknownC = BuffType(8)
		val FireSpark = BuffType(9) //fire passive (free insta cast)
		val Intuition = BuffType(10) //scout passive (insta charge)
		val Elusiveness = BuffType(11) //ninja passive (25MP + next hit crits
		val Swiftness = BuffType(12)
	}
}