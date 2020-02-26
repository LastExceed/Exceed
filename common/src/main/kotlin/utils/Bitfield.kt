package utils

import java.util.*

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

internal fun BooleanArray.toLong(): Long {
	return this.foldIndexed(0L) { i, a, b ->
		a + (if (b) 1L shl i else 0)
	}
}

internal fun BooleanArray.toInt(): Int {
	return this.foldIndexed(0) { i, a, b ->
		a + (if (b) 1 shl i else 0)
	}
}