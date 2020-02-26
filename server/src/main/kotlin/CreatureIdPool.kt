package exceed

import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock

class CreatureIdPool {
	private val ids = mutableListOf<Long>()
	private val mutex = Mutex()

	suspend fun claim(): Long = mutex.withLock {
		var newID = 0L
		while (ids.contains(newID)) {
			newID++
		}
		ids.add(newID)

		newID
	}

	fun free(id: Long) {
		ids.remove(id)
	}
}