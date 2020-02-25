package packets

class Join(
	val unknown: Int,
	val creatureID: Long,
	val junk: ByteArray
) : Packet(Opcode.Join) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(unknown)
		writer.writeLong(creatureID)
		writer.writeByteArray(junk)
	}

	companion object {
		suspend fun readFrom(reader: Reader): Join {
			return Join(
				unknown = reader.readInt(),
				creatureID = reader.readLong(),
				junk = reader.readByteArray(0x1168)
			)
		}
	}
}