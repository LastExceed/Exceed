package modules

import dev.kord.core.*
import dev.kord.core.entity.channel.TextChannel
import dev.kord.core.event.message.MessageCreateEvent
import dev.kord.gateway.*
import kotlinx.coroutines.flow.first
import java.io.File

object DiscordBot {
	private const val PUBLIC_CHANNEL_NAME = "ingame-chat"
	private const val STAFF_CHANNEL_NAME = "irc"
	private lateinit var publicChannel: TextChannel
	suspend fun run() {
		val token = File("discord_bot_token").let {
			if (it.createNewFile()) {
				error("token not found")
			} else it.readText()
		}

		Kord(token) {
		}.run {
			publicChannel = guilds.first().channels.first { it.name == PUBLIC_CHANNEL_NAME } as TextChannel

			on<MessageCreateEvent> {
				val channel = (message.channel.asChannel() as? TextChannel ?: return@on)
				val author = message.author ?: return@on

				if (message.content == ".who") {
					val header = "${Server.mainLayer.players.size} player(s) currently connected:\n"
					channel.createMessage(header + Server.mainLayer.players.values.joinToString("\n") {
						"#${it.character.id.value} ${it.character.name}" + if (channel.name == STAFF_CHANNEL_NAME) " ${it.ipAdress}" else ""
					})
					return@on
				}

				when (channel.name) {
					PUBLIC_CHANNEL_NAME -> {
						if (author.isBot) return@on
						Server.mainLayer.announce("> ${author.username}: " + message.content, true)
					}
					STAFF_CHANNEL_NAME -> {
						val splits = message.content.split(" ").listIterator()
						if ((splits.nextOrNull() ?: return@on) == ".kick") {
							val target = Server.mainLayer.findPlayerByNameOrId((splits.nextOrNull() ?: return@on)) ?: return@on
							var reason = ""
							splits.forEachRemaining { reason += " $it" }
							target.kick(reason.trim())
						}
					}
				}

			}
			login {
				@OptIn(PrivilegedIntent::class)
				intents += Intent.MessageContent
			}
		}
	}

	suspend fun post(text: String) {
		publicChannel.createMessage(text)
	}
}