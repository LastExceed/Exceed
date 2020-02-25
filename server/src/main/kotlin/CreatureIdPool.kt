package exceed

import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.sync.Mutex

class CreatureIdPool {
	private val ids = mutableListOf<Long>()

	private val mutex = Mutex()
	fun claim(): Long {
		runBlocking {
			mutex.lock()
		}
		var newID = 0L
		while (ids.contains(newID)) {
			newID++
		}
		ids.add(newID)
		runBlocking {
			mutex.unlock()
		}
		return newID
	}

	fun free(id: Long) {
		ids.remove(id)
	}
}