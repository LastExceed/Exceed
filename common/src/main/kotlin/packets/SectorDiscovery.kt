package packets

import utils.*

data class SectorDiscovery(
	val sector: Vector2<Int>
) : Packet(Opcode.ChunkDiscovery) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(sector)
	}

	companion object {
		suspend fun readFrom(reader: Reader): SectorDiscovery {
			return SectorDiscovery(reader.readVector2Int())
		}
	}
}