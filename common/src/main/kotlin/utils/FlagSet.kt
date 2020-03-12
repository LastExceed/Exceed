package utils

inline class FlagSet<IndexType : FlagSetIndex>(val inner: BooleanArray) {
	operator fun get(index: IndexType): Boolean = inner[index.value]

	operator fun set(index: IndexType, value: Boolean) {
		inner[index.value] = value
	}
}


interface FlagSetIndex {
	val value: Int
}


internal fun Byte.toBooleanArray() = BooleanArray(Byte.SIZE_BITS) {
	(this.toInt() shr it) and 1 != 0
}

internal fun Short.toBooleanArray() = BooleanArray(Short.SIZE_BITS) {
	(this.toInt() shr it) and 1 != 0
}

internal fun Int.toBooleanArray() = BooleanArray(Int.SIZE_BITS) {
	(this shr it) and 1 != 0
}

internal fun Long.toBooleanArray() = BooleanArray(Long.SIZE_BITS) {
	(this shr it) and 1 != 0L
}


internal fun BooleanArray.toByte() = this.foldIndexed(0) { index, accumulator, value ->
	accumulator + (if (value) 1 shl index else 0)
}.toByte()

internal fun BooleanArray.toShort() = this.foldIndexed(0) { index, accumulator, value ->
	accumulator + (if (value) 1 shl index else 0)
}.toShort()

internal fun BooleanArray.toInt() = this.foldIndexed(0) { index, accumulator, value ->
	accumulator + (if (value) 1 shl index else 0)
}

internal fun BooleanArray.toLong() = this.foldIndexed(0L) { index, accumulator, value ->
	accumulator + (if (value) 1L shl index else 0)
}