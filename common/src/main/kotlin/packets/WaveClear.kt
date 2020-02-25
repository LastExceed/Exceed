package packets

class WaveClear() : Packet(Opcode.WaveClear) {
	override suspend fun writeTo(writer: Writer) {}

	companion object {
		suspend fun readFrom(reader: Reader): WaveClear {
			return WaveClear()
		}
	}
}