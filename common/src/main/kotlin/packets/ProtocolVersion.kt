package packets

data class ProtocolVersion(
	val version: Int
) : Packet(Opcode.ProtocolVersion) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(version)
	}

	companion object {
		suspend fun readFrom(reader: Reader): ProtocolVersion {
			return ProtocolVersion(reader.readInt())
		}
	}
}