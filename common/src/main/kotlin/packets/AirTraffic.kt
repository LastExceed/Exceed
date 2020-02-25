package packets

import utils.*

data class AirTraffic(
	val airships: MutableList<Airship> = mutableListOf()
) : Packet(Opcode.AirTraffic) {
	override suspend fun writeTo(writer: Writer) {
		writer.writeInt(airships.size)
		airships.forEach {
			it.writeTo(writer)
		}
	}

	companion object {
		suspend fun readFrom(reader: Reader): AirTraffic {
			val airTraffic = AirTraffic()
			repeat(reader.readInt()) {
				airTraffic.airships.add(Airship.read(reader))
			}
			return airTraffic
		}
	}
}

data class Airship(
	val id: Long,
	val unknownA: Int,
	val unknownB: Int,
	val position: Vector3<Long>,
	val velocity: Vector3<Float>,
	val rotation: Float,
	val station: Vector3<Long>,
	val pathRotation: Float,
	val unknownC: Int,
	val destination: Vector3<Long>,
	val state: AirshipState,
	val unknownD: Int
) : CwSerializable {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(id)
		writer.writeInt(unknownA)
		writer.writeInt(unknownB)
		writer.writeVector3Long(position)
		writer.writeVector3Float(velocity)
		writer.writeFloat(rotation)
		writer.writeVector3Long(station)
		writer.writeFloat(pathRotation)
		writer.writeInt(unknownC)
		writer.writeVector3Long(destination)
		writer.writeInt(state.value)
		writer.writeInt(unknownD)
	}

	companion object {
		internal suspend fun read(reader: Reader): Airship {
			return Airship(
				id = reader.readLong(),
				unknownA = reader.readInt(),
				unknownB = reader.readInt(),
				position = reader.readVector3Long(),
				velocity = reader.readVector3Float(),
				rotation = reader.readFloat(),
				station = reader.readVector3Long(),
				pathRotation = reader.readFloat(),
				unknownC = reader.readInt(),
				destination = reader.readVector3Long(),
				state = AirshipState(reader.readInt()),
				unknownD = reader.readInt()
			)
		}
	}
}

inline class AirshipState(val value: Int) {
	companion object {
		//TODO: figure out which airship states exist
	}
}