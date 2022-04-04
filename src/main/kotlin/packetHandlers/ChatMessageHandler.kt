package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.ChatMessage
import modules.ChatCommands

object ChatMessageHandler : PacketHandler<ChatMessage.FromClient> {
	override suspend fun handlePacket(packet: ChatMessage.FromClient, source: Player) {
		if (ChatCommands.parse(packet.text, source))
			return

		source.layer.broadcast(ChatMessage.FromServer(source.character.id, packet.text))
		println("${source.character.name}: ${packet.text}")
	}
}