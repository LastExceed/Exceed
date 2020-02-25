package packets

class ServerFull() : Packet(Opcode.ServerFull) {
	override suspend fun writeTo(writer: Writer) {}


	companion object {
		suspend fun readFrom(reader: Reader): ServerFull {
			return ServerFull()
		}
	}
}