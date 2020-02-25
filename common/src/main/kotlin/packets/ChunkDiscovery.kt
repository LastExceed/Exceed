package packets

import utils.*

data class ChunkDiscovery(
	val chunk: Vector2<Int>
) : Packet(Opcode.ChunkDiscovery) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(chunk)
	}

	companion object {
		suspend fun readFrom(reader: Reader): ChunkDiscovery {
			return ChunkDiscovery(reader.readVector2Int())
		}
	}
}