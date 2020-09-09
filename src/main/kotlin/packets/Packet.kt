package packets

abstract class Packet(val opcode: Opcode) : CwSerializable

inline class Opcode(val value: Int) {
	companion object {
		val CreatureUpdate = Opcode(0)
		val MultiEntityUpdate = Opcode(1)
		val WaveClear = Opcode(2)
		val AirTraffic = Opcode(3)
		val ServerUpdate = Opcode(4)
		val Time = Opcode(5)
		val CreatureAction = Opcode(6)
		val Hit = Opcode(7)
		val Buff = Opcode(8)
		val Shot = Opcode(9)
		val ChatMessage = Opcode(10)
		val ChunkDiscovery = Opcode(11)
		val SectorDiscovery = Opcode(12)
		val Unknown13 = Opcode(13)
		val Unknown14 = Opcode(14)
		val MapSeed = Opcode(15)
		val Join = Opcode(16)
		val ProtocolVersion = Opcode(17)
		val ServerFull = Opcode(18)
	}
}