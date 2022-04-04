import kotlinx.coroutines.sync.*
import com.github.lastexceed.cubeworldnetworking.packets.*

object CreatureIdPool {
	private val claimed = mutableListOf<Long>()
	private val mutex = Mutex()

	suspend fun claim(): CreatureId = mutex.withLock {
		var newID = 0L
		while (claimed.contains(newID)) {
			newID++
		}
		claimed.add(newID)

		CreatureId(newID)
	}

	fun free(id: CreatureId) {
		claimed.remove(id.value)
	}
}