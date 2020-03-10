package exceed

import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock
import packets.CreatureID

class CreatureIdPool {
	private val claimed = mutableListOf<Long>()
	private val mutex = Mutex()

	suspend fun claim(): CreatureID = mutex.withLock {
		var newID = 0L
		while (claimed.contains(newID)) {
			newID++
		}
		claimed.add(newID)

		CreatureID(newID)
	}

	fun free(id: CreatureID) {
		claimed.remove(id.value)
	}
}