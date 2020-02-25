package packets

data class ChatMessage(
	val sender: Long?,
	val text: String
) : Packet(Opcode.ChatMessage) {
	override suspend fun writeTo(writer: Writer) {
		if (sender != null) {
			writer.writeLong(sender)
		}
		writer.writeInt(text.length)
		writer.writeByteArray(text.toByteArray(Charsets.UTF_16LE))
	}

	companion object {
		suspend fun readFrom(reader: Reader, readSender: Boolean): ChatMessage {
			return ChatMessage(
				sender = if (readSender) reader.readLong() else null,
				text = String(reader.readByteArray(reader.readInt() * 2), Charsets.UTF_16LE)
			)
		}
	}
}