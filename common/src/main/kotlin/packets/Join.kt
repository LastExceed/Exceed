package packets

class Join(
	val unknown: Int,
	val assignedID: CreatureID,
	val junk: ByteArray
) : Packet(Opcode.Join) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(unknown)
		writer.writeLong(assignedID.value)
		writer.writeByteArray(junk)
	}

	companion object {
		suspend fun readFrom(reader: Reader): Join {
			return Join(
				unknown = reader.readInt(),
				assignedID = CreatureID(reader.readLong()),
				junk = reader.readByteArray(0x1168)
			)
		}
	}
}