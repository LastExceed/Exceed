package packets

interface CwSerializable {
	suspend fun writeTo(writer: Writer)
}