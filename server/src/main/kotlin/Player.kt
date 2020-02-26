package exceed

import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.sync.Mutex
import packets.*

class Player(
	private var writer: Writer,
	val character: Creature,
	layer: Layer
) {
	var layer = layer
		private set

	init {
		layer.addPlayer(this)
	}

	private val mutex: Mutex = Mutex(false)
	fun send(packet: Packet) {
		runBlocking {
			mutex.lock()
			try {
				writer.writeInt(packet.opcode.value)
				packet.writeTo(writer)
			} catch (ex: Throwable) {
				TODO("check which exception is thrown here")
				//happens when a player disconnected
				//disconnections are handled by the reading thread
				//so we dont have to do anything here
			}
			mutex.unlock()
		}
	}

	fun moveTo(destination: Layer) {
		if (destination == layer) {
			return
		}
		layer.removePlayer(this)
		clearCreatures()
		destination.addPlayer(this)
		layer = destination
	}

	fun clearCreatures() {
		send(WaveClear())
		send(WaveClear())
	}
}