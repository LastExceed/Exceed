package packetHandlers

import modules.ChatCommands
import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

object ChatMessageHandler : PacketHandler<ChatMessage> {
	override suspend fun handlePacket(packet: ChatMessage, source: Player) {
		if (ChatCommands.parse(packet.text, source))
			return
		val packet2 = packet.copy(sender = source.character.id)
		source.layer.broadcast(packet2)
		println(packet.text)
	}
}