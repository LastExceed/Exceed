package packets

class MapSeed(
	val seed: Int
) : Packet(Opcode.MapSeed) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(seed)
	}

	companion object {
		suspend fun readFrom(reader: Reader): MapSeed {
			return MapSeed(reader.readInt())
		}
	}
}