package exceed.packetHandlers

import Modules.ChatCommands
import packets.ChatMessage
import exceed.Player

object ChatMessageHandler : PacketHandler<ChatMessage> {
	override fun handlePacket(packet: ChatMessage, source: Player) {
		if (ChatCommands.parse(packet.text, source)) {
			return
		}
		val packet2 = packet.copy(sender = source.character.id)
		source.layer.broadcast(packet2)
		println(packet.text)
	}
}