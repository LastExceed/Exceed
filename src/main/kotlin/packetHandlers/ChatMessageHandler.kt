package packetHandlers

import modules.ChatCommands
import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

object ChatMessageHandler : PacketHandler<ChatMessage> {
	override suspend fun handlePacket(packet: ChatMessage, source: Player) {
		if (ChatCommands.parse(packet.text, source))
			return
		source.layer.broadcast(packet.copy(sender = source.character.id))
		println("${source.character.name}: ${packet.text}")
	}
}