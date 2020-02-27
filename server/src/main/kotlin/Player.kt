package exceed

import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock
import packets.*

class Player(
	private var writer: Writer,
	val character: Creature,
	layer: Layer
) {
	var layer = layer
		private set

	init {
		//TODO: consider disguising a factory function as ctor for suspendability
		val p = this
		runBlocking {
			layer.addPlayer(p)
		}
	}

	private val mutex = Mutex(false)
	suspend fun send(packet: Packet) = mutex.withLock {
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
}