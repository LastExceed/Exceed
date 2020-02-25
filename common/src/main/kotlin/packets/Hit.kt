package packets

import utils.*

data class Hit(
	val attacker: Long,
	val target: Long,
	val damage: Float,
	val critical: Boolean,
	val stuntime: Int,
	val paddingA: Int,
	val position: Vector3<Long>,
	val direction: Vector3<Float>,
	val isYellow: Boolean,
	val damageType: DamageType,
	val flash: Boolean,
	val paddingB: Byte
) : Packet(Opcode.Hit), SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(attacker)
		writer.writeLong(target)
		writer.writeFloat(damage)
		writer.writeBoolean(critical); writer.pad(3)
		writer.writeInt(stuntime)
		writer.writeInt(paddingA)
		writer.writeVector3Long(position)
		writer.writeVector3Float(direction)
		writer.writeBoolean(isYellow)
		writer.writeByte(damageType.value)
		writer.writeBoolean(flash)
		writer.writeByte(paddingB)
	}

	companion object {
		suspend fun readFrom(reader: Reader): Hit {
			return Hit(
				attacker = reader.readLong(),
				target = reader.readLong(),
				damage = reader.readFloat(),
				critical = reader.readInt() > 0,
				stuntime = reader.readInt(),
				paddingA = reader.readInt(),
				position = reader.readVector3Long(),
				direction = reader.readVector3Float(),
				isYellow = reader.readBoolean(),
				damageType = DamageType(reader.readByte()),
				flash = reader.readBoolean(),
				paddingB = reader.readByte()
			)
		}
	}
}

inline class DamageType(val value: Byte) {
	companion object {
		val Block = DamageType(1)
		val Normal = DamageType(2)
		val Miss = DamageType(3)
		val Invisible = DamageType(4)
		val Absorb = DamageType(5)
		val Invisible2 = DamageType(6)
	}
}