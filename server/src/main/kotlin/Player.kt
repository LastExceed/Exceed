package exceed

import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock
import packets.*

class Player private constructor(
	private var writer: Writer,
	val character: Creature,
	layer: Layer
) {
	var layer = layer
		private set
	private val mutex = Mutex(false)

	suspend fun send(packet: Packet) {
		mutex.withLock(this) {
			try {
				writer.writeInt(packet.opcode.value)
				packet.writeTo(writer)
			} catch (ex: Throwable) {
				TODO("check which exception is thrown here")
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
		send(WaveClear())
		send(WaveClear())
	}

	companion object {
		suspend fun create(writer: Writer, character: Creature, layer: Layer) =
			Player(writer, character, layer).also {
				layer.addPlayer(it)
			}
	}
}