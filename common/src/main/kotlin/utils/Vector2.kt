package utils

import packets.*

data class Vector2<TT>(var x: TT, var y: TT)

internal suspend fun Reader.readVector2Int(): Vector2<Int> {
	return Vector2<Int>(this.readInt(), this.readInt())
}

internal suspend fun Writer.writeVector2Int(vector: Vector2<Int>) {
	this.writeInt(vector.x)
	this.writeInt(vector.y)
}