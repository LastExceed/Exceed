package packets

import kotlinx.coroutines.io.*

class Reader(val inner: ByteReadChannel) {
	constructor(data: ByteArray) : this(ByteReadChannel(data))

	suspend fun readByte(): Byte = inner.readByte()
	suspend fun readShort(): Short = inner.readShortLittleEndian()
	suspend fun readInt(): Int = inner.readIntLittleEndian()
	suspend fun readFloat(): Float = inner.readFloatLittleEndian()
	suspend fun readLong(): Long = inner.readLongLittleEndian()
	suspend fun readByteArray(count: Int): ByteArray {
		val data = ByteArray(count)
		inner.readFully(data, 0, count)
		return data
	}

	suspend fun readBoolean(): Boolean = inner.readBoolean()
	suspend fun readChar(): Char = inner.readByte().toChar()
}