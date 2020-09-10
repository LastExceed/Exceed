package packets

import utils.*

data class Shot(
	val attacker: Long,
	val chunk: Vector2<Int>,
	val unknownA: Int,
	val paddingA: Int,
	val position: Vector3<Long>,
	val unknownV: Vector3<Int>,
	val velocity: Vector3<Float>,
	val legacyDamage: Float,
	val unknownB: Float,
	val scale: Float,
	val mana: Float,
	val particles: Float,
	val skill: Int,
	val projectile: Projectile,
	val paddingB: Int,
	val unknownC: Float,
	val unknownD: Float
) : Packet(Opcode.Shot), SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(attacker)
		writer.writeVector2Int(chunk)
		writer.writeInt(unknownA)
		writer.writeInt(paddingA)
		writer.writeVector3Long(position)
		writer.writeVector3Int(unknownV)
		writer.writeVector3Float(velocity)
		writer.writeFloat(legacyDamage)
		writer.writeFloat(unknownB)
		writer.writeFloat(scale)
		writer.writeFloat(mana)
		writer.writeFloat(particles)
		writer.writeInt(skill)
		writer.writeInt(projectile.value)
		writer.writeInt(paddingB)
		writer.writeFloat(unknownC)
		writer.writeFloat(unknownD)
	}

	companion object : CwDeserializer<Shot> {
		override suspend fun readFrom(reader: Reader): Shot {
			return Shot(
				attacker = reader.readLong(),
				chunk = reader.readVector2Int(),
				unknownA = reader.readInt(),
				paddingA = reader.readInt(),
				position = reader.readVector3Long(),
				unknownV = reader.readVector3Int(),
				velocity = reader.readVector3Float(),
				legacyDamage = reader.readFloat(),
				unknownB = reader.readFloat(),
				scale = reader.readFloat(),
				mana = reader.readFloat(),
				particles = reader.readFloat(),
				skill = reader.readInt(),
				projectile = Projectile(reader.readInt()),
				paddingB = reader.readInt(),
				unknownC = reader.readFloat(),
				unknownD = reader.readFloat()
			)
		}
	}
}

inline class Projectile(val value: Int) {
	companion object {
		val Arrow = Projectile(1)
		val Boomerang = Projectile(2)
		val Unknown = Projectile(3)
		val Boulder = Projectile(4)
	}
}