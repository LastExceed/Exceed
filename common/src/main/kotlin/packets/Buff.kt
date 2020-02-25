package packets

data class Buff(
	val source: Long,
	val target: Long,
	val type: BuffType,
	val unknownA: Byte,
	val unknownB: Short,
	val modifier: Float,
	val duration: Int,
	val unknownC: Int,
	val creatureID3: Long
) : Packet(Opcode.Buff), SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(source)
		writer.writeLong(target)
		writer.writeByte(type.value)
		writer.writeByte(unknownA)
		writer.writeShort(unknownB)
		writer.writeFloat(modifier)
		writer.writeInt(duration)
		writer.writeInt(unknownC)
		writer.writeLong(creatureID3)
	}

	companion object {
		suspend fun readFrom(reader: Reader): Buff {
			return Buff(
				source = reader.readLong(),
				target = reader.readLong(),
				type = BuffType(reader.readByte()),
				unknownA = reader.readByte(),
				unknownB = reader.readShort(),
				modifier = reader.readFloat(),
				duration = reader.readInt(),
				unknownC = reader.readInt(),
				creatureID3 = reader.readLong()
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