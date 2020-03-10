package packets

import utils.*

data class CreatureAction(
	val item: Item,
	val chunk: Vector2<Int>,
	val index: Int,
	val unknownA: Int,
	val type: ActionType,
	val unknownB: Byte,
	val unknownC: Short
) : Packet(Opcode.CreatureAction) {
	override suspend fun writeTo(writer: Writer) {
		item.writeTo(writer)
		writer.writeVector2Int(chunk)
		writer.writeInt(index)
		writer.writeInt(unknownA)
		writer.writeByte(type.value)
		writer.writeByte(unknownB)
		writer.writeShort(unknownC)
	}

	companion object {
		suspend fun readFrom(reader: Reader): CreatureAction {
			return CreatureAction(
				item = Item.readFrom(reader),
				chunk = reader.readVector2Int(),
				index = reader.readInt(),
				unknownA = reader.readInt(),
				type = ActionType(reader.readByte()),
				unknownB = reader.readByte(),
				unknownC = reader.readShort()
			)
		}
	}
}

inline class ActionType(val value: Byte) {
	companion object {
		val Bomb = ActionType(1)
		val Talk = ActionType(2)
		val ObjectInteraction = ActionType(3)
		val Unknown4 = ActionType(4)
		val PickUp = ActionType(5)
		val Drop = ActionType(6)
		val Unknown7 = ActionType(7)
		val CallPet = ActionType(8)
	}
}