package packetHandlers

import Player
import com.andreapivetta.kolor.*
import com.github.lastexceed.cubeworldnetworking.packets.ChatMessage
import modules.ChatCommands

suspend fun onChatMessage(packet: ChatMessage.FromClient, source: Player) {
	if (ChatCommands.parse(source, packet.text))
		return

	source.layer.broadcast(ChatMessage.FromServer(source.character.id, packet.text))
	println("${Kolor.foreground(source.character.name + ": ", Color.CYAN)}${packet.text}")
}