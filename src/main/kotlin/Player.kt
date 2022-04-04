import io.ktor.network.sockets.*
import kotlinx.coroutines.sync.*
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.Writer
import kotlinx.coroutines.delay
import java.io.*

class Player private constructor(
	private val socket: Socket,
	private val writer: Writer,
	val character: Creature,
	layer: Layer
) {
	var layer = layer
		private set
	private val mutex = Mutex(false)

	suspend fun send(packet: Packet) {
		mutex.withLock {//TODO: IllegalStateException: Already locked
			try {
				packet.packetId.writeTo(writer)
				packet.writeTo(writer)
			} catch (ex: IOException) {
				//happens when a player disconnected
				//disconnections are handled by the reading thread
				//so we dont have to do anything here
			}
		}
	}

	suspend fun moveTo(destination: Layer) {
		if (destination == layer) {
			return
		}
		layer.removePlayer(this)
		clearCreatures()
		destination.addPlayer(this)
		layer = destination
	}

	suspend fun clearCreatures() {
		send(ServerTick())
		send(ServerTick())
	}

	suspend fun kick(reason: String) {
		layer.announce("kicked ${character.name} because of $reason")
		delay(100)
		socket.dispose()
	}

	companion object {
		suspend fun create(socket: Socket, writer: Writer, character: Creature, layer: Layer) =
			Player(socket, writer, character, layer).also {
				layer.addPlayer(it)
			}
	}

	suspend fun notify(message: String) {
		send(ChatMessage.FromServer(CreatureId(0), message))
	}
}