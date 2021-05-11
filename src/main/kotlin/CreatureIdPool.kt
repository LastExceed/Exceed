import kotlinx.coroutines.sync.*
import me.lastexceed.cubeworldnetworking.packets.*

object CreatureIdPool {
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