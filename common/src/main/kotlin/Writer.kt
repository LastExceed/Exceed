package packets

import io.ktor.utils.io.*

class Writer(private val inner: ByteWriteChannel) {
	suspend fun writeByte(value: Byte) = inner.writeByte(value)
	suspend fun writeShort(value: Short) = inner.writeShortLittleEndian(value)
	suspend fun writeInt(value: Int) = inner.writeIntLittleEndian(value)
	suspend fun writeFloat(value: Float) = inner.writeFloatLittleEndian(value)
	suspend fun writeLong(value: Long) = inner.writeLongLittleEndian(value)
	suspend fun writeByteArray(data: ByteArray) = inner.writeFully(data)
	suspend fun writeBoolean(value: Boolean) = inner.writeBoolean(value)

	suspend fun pad(count: Int) = writeByteArray(ByteArray(count))
}