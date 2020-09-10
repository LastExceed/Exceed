package packets

import utils.*

data class ResidenceChunk(
	val chunk: Vector2<Int>
) : Packet(Opcode.ChunkDiscovery) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(chunk)
	}

	companion object : CwDeserializer<ResidenceChunk> {
		override suspend fun readFrom(reader: Reader): ResidenceChunk {
			return ResidenceChunk(reader.readVector2Int())
		}
	}
}