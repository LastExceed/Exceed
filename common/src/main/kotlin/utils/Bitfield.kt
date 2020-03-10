package utils

interface FlagSetKey {
	val value: Int
}

inline class FlagSet<KeyType : FlagSetKey>(val inner: BooleanArray) {
	operator fun get(index: KeyType): Boolean = inner[index.value]

	operator fun set(index: KeyType, value: Boolean) {
		inner[index.value] = value
	}
}

internal fun Byte.toBooleanArray(): BooleanArray {
	return BooleanArray(Byte.SIZE_BITS) {
		(this.toInt() shr it) and 1 != 0
	}
}

internal fun Short.toBooleanArray(): BooleanArray {
	return BooleanArray(Short.SIZE_BITS) {
		(this.toInt() shr it) and 1 != 0
	}
}

internal fun Int.toBooleanArray(): BooleanArray {
	return BooleanArray(Int.SIZE_BITS) {
		(this shr it) and 1 != 0
	}
}

internal fun Long.toBooleanArray(): BooleanArray {
	return BooleanArray(Long.SIZE_BITS) {
		(this shr it) and 1 != 0L
	}
}


internal fun BooleanArray.toByte(): Byte {
	return this.foldIndexed(0) { i, a, b ->
		a + (if (b) 1 shl i else 0)
	}.toByte()
}

internal fun BooleanArray.toShort(): Short {
	return this.foldIndexed(0) { i, a, b ->
		a + (if (b) 1 shl i else 0)
	}.toShort()
}

internal fun BooleanArray.toInt(): Int {
	return this.foldIndexed(0) { i, a, b ->
		a + (if (b) 1 shl i else 0)
	}
}

internal fun BooleanArray.toLong(): Long {
	return this.foldIndexed(0L) { i, a, b ->
		a + (if (b) 1L shl i else 0)
	}
}