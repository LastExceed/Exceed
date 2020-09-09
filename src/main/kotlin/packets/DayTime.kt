package packets

import utils.*

data class DayTime(
	val day: Int,
	val time: Int
) : Packet(Opcode.Time) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(day)
		writer.writeInt(time)
	}

	companion object {
		suspend fun readFrom(reader: Reader): DayTime {
			return DayTime(
				day = reader.readInt(),
				time = reader.readInt()
			)
		}
	}
}