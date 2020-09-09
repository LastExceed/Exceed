package utils

data class Vector3<TT>(var x: TT, var y: TT, var z: TT)

internal suspend fun Reader.readVector3Byte(): Vector3<Byte> {
	return Vector3(this.readByte(), this.readByte(), this.readByte())
}

internal suspend fun Reader.readVector3Int(): Vector3<Int> {
	return Vector3(this.readInt(), this.readInt(), this.readInt())
}

internal suspend fun Reader.readVector3Float(): Vector3<Float> {
	return Vector3(this.readFloat(), this.readFloat(), this.readFloat())
}

internal suspend fun Reader.readVector3Long(): Vector3<Long> {
	return Vector3(this.readLong(), this.readLong(), this.readLong())
}

internal suspend fun Writer.writeVector3Byte(vector: Vector3<Byte>) {
	this.writeByte(vector.x)
	this.writeByte(vector.y)
	this.writeByte(vector.z)
}

internal suspend fun Writer.writeVector3Int(vector: Vector3<Int>) {
	this.writeInt(vector.x)
	this.writeInt(vector.y)
	this.writeInt(vector.z)
}

internal suspend fun Writer.writeVector3Float(vector: Vector3<Float>) {
	this.writeFloat(vector.x)
	this.writeFloat(vector.y)
	this.writeFloat(vector.z)
}

internal suspend fun Writer.writeVector3Long(vector: Vector3<Long>) {
	this.writeLong(vector.x)
	this.writeLong(vector.y)
	this.writeLong(vector.z)
}